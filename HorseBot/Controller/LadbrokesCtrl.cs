using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
//using WebSocketSharp;
//using WebSocket4Net;
using HtmlAgilityPack;
using Betburger.Constant;
using Betburger.Json;
using Betburger.Model;
using SuperSocket.ClientEngine.Proxy;
using System.Net.WebSockets;
using System.Threading;
using System.Diagnostics;


namespace Betburger.Controller
{
    public delegate void onParseSocketHandler(string response);
    public class LadbrokesCtrl
    {
        private HttpClient httpClient = null;
        private CookieContainer container = null;
        private ClientWebSocket ws;
        private const int ReceiveChunkSize = 1024;
        private const int SendChunkSize = 1024;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private readonly CancellationToken _cancellationToken;

        private List<TempMatchInfo> matchInfos = new List<TempMatchInfo>();
        private MatchInfoWrapper activeInfo = new MatchInfoWrapper();
        private List<MatchInfo> infos = new List<MatchInfo>();

        private List<string> groupListLive = new List<string>();
        private List<string> groupListUpcoming = new List<string>();
        private bool bUnSubscribed = false;
        private string containerId = string.Empty;

        private onWriteStatusEvent onWriteStatus = null;
        private onSendMatchEvent onSendMatchLadbrokes = null;
        private onWriteLogEvent onWriteLog = null;

        private DateTime sessionTime = DateTime.Now;

        public LadbrokesCtrl(onWriteStatusEvent _onWriteStatus, onSendMatchEvent _onSendMatchLadbrokes, onWriteLogEvent _onWriteLog)
        {
            if (httpClient == null)
                initHttpClient();

            onWriteLog = _onWriteLog;
            onWriteStatus = _onWriteStatus;
            onSendMatchLadbrokes = _onSendMatchLadbrokes;

            _cancellationToken = _cancellationTokenSource.Token;
        }

        private void initHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler();
            container = new CookieContainer();
            handler.CookieContainer = container;
            httpClient = new HttpClient(handler);

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        public void doWork()
        {
            try
            {
                onWriteStatus(getLogTitle() + "Socket openning...");

                if (!doOpenSocket())
                {
                    onWriteStatus(getLogTitle() + "Socket open failed!");
                    return;
                }
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (doWork) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
            }
        }

        private bool doOpenSocket()
        {
            try
            {
                HttpResponseMessage responseMessageMain = httpClient.GetAsync("https://sports.ladbrokes.com/en-gb/bet-in-play/").Result;
                responseMessageMain.EnsureSuccessStatusCode();

                string responseMessageMainString = responseMessageMain.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseMessageMainString))
                    return false;

                GroupCollection groups = Regex.Match(responseMessageMainString, "<script src=\"//.*/opentag-\\d+-(?<containerId>\\d+)\\.js\" async defer></script>").Groups;
                if (groups == null || groups["containerId"] == null)
                    return false;

                containerId = groups["containerId"].Value;
                if (string.IsNullOrEmpty(containerId))
                    return false;

                getWebSocketRequest();

                return true;
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (doOpenSocket) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
                return false;
            }
        }

        private void getWebSocketRequest()
        {
            infos.Clear();

            if (!Constants.bRun)
                return;

            try
            {
                string url = string.Format("wss://sports.ladbrokes.com/api/{0}/{1}/websocket", Utils.GetRandomServer(), Utils.getRandomKey().ToLower());

                ws = new ClientWebSocket();
                ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(20);
                ws.Options.Cookies = new CookieContainer();

                foreach (Cookie cookie in container.GetCookies(new Uri("https://sports.ladbrokes.com")))
                {
                    ws.Options.Cookies.Add(new Uri("https://sports.ladbrokes.com"), new System.Net.Cookie(cookie.Name, cookie.Value));
                }

                long tick = Utils.getTick();

                ws.Options.Cookies.Add(new Uri("https://sports.ladbrokes.com"), new System.Net.Cookie("_qsst_s", tick.ToString()));
                ws.Options.Cookies.Add(new Uri("https://sports.ladbrokes.com"), new System.Net.Cookie("_qst_s", "1"));
                ws.Options.Cookies.Add(new Uri("https://sports.ladbrokes.com"), new System.Net.Cookie(string.Format("x_qtag_{0}", containerId), string.Format("EYXsports.ladbrokes.com*{0}*play@*a*Qsc*Q*j1*C*B1*C*P1*5-@1-*C*R*Z*a*Idirect*Y*9-*@0-/en-gb/bet-in-@2-/*Y*A@1-*b*E*C*F*Q*@0-/en-gb/bet-in-@2-/*Y*Q__v*z", tick + 1)));

                ws.ConnectAsync(new Uri(url), CancellationToken.None).Wait();

                StartListen();
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (getWebSocketRequest) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
            }
        }

        private void StartListen()
        {
            var buffer = new byte[ReceiveChunkSize];

            try
            {
                while (ws.State == WebSocketState.Open)
                {
                    var stringResult = new StringBuilder();

                    WebSocketReceiveResult result;
                    do
                    {
                        result = ws.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationToken).Result;

                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            disconnect();
                            return;
                        }
                        else
                        {
                            var str = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            stringResult.Append(str);
                        }

                    } while (!result.EndOfMessage);

                    //onParseSocket(stringResult.ToString());
                    parseSocketResponse(stringResult.ToString());
                }
            }
            catch (Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (StartListen) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
                doOpenSocket();
            }
        }

        private void disconnect()
        {
            if(ws != null && ws.State == WebSocketState.Open)
            {
                ws.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None).Wait();
                onWriteStatus(getLogTitle() + "Socket closed, reconnecting...");

                getWebSocketRequest();
            }
        }

        private void parseSocketResponse(string message)
        {
            if (!Constants.bRun)
            {
                disconnect();
                return;
            }

            //onWriteLog("ladbrokes.txt", "->" + message);

            if(message != "o")
            {
                if(DateTime.Now.Subtract(sessionTime).TotalSeconds > 9)
                {
                    SendMessage("[\"\\n\"]");
                    sessionTime = DateTime.Now;
                }
            }

            if(message == "o")
            {
                sessionTime = DateTime.Now;
                SendMessage(Resource1.Param1);
            }
            else if(message.Contains("a[\"CONNECTED"))
            {
                SendMessage(Resource1.Param2);
                SendMessage(Resource1.Param3);
            }
            else if(message.Contains("a[\"MESSAGE\\ntype:READY"))
            {
                SendMessage(Resource1.Param4);
                SendMessage(Resource1.Param5);
            }
            else if(message.Contains("a[\"MESSAGE\\nid:/api/event-group-list/LIVE"))
            {
                groupListLive = getEventGroupList(message);
                if (groupListLive == null || groupListLive.Count < 1)
                    return;
            }
            else if(message.Contains("a[\"MESSAGE\\nid:/api/event-group-list/UPCOMING"))
            {
                foreach (string groupId in groupListLive)
                {
                    string param = string.Format(Resource1.Param6, groupId, groupId);
                    SendMessage(param);
                }

                groupListUpcoming = getEventGroupList(message);
            }
            else if(message.Contains("a[\"MESSAGE\\nid:/api/en-GB/event-groups/EventGroup-LIVE-"))
            {
                bool bEvent = false;
                if(message.Contains("a[\"MESSAGE\\nid:/api/en-GB/event-groups/EventGroup-LIVE-21"))
                {
                    // Soccer
                    parseJsonEvent(message, SPORT.Horse);
                    bEvent = true;
                }
                else if (message.Contains("a[\"MESSAGE\\nid:/api/en-GB/event-groups/EventGroup-LIVE-19"))
                {
                    // American Football
                    parseJsonEvent(message, SPORT.Dog);
                    bEvent = true;
                }

                if (bEvent)
                {
                    onWriteStatus(getLogTitle() + string.Format("There are {0} live events now...", matchInfos.Count));
                }

                //if (matchInfos.Count == 0)
                //{
                //    onWriteStatus(getLogTitle() + "There is nothing event. Sleep 5 seconds...");
                //    Thread.Sleep(5000);
                //    disconnect();
                //}
            }
            else if (!string.IsNullOrEmpty(activeInfo.matchInfo.id) && message.Contains(string.Format(Resource1.Event, activeInfo.matchInfo.id))) // && response.Contains(activeInfo.matchInfo.name)
            {
                onWriteStatus(getLogTitle() + string.Format("Response: [{0}] [{1}]", activeInfo.matchInfo.sport, activeInfo.matchInfo.name));
                parseSocketResponseMarkets(message);

                if (matchInfos.Count == 0)
                {
                    onSendMatchLadbrokes(infos);
                    disconnect();
                }

                return;
            }
            else
            {
                Console.WriteLine(message);
            }

            if (matchInfos.Count > 0 && activeInfo.response)
            {
                activeInfo.setMatchInfoWrapper(matchInfos[0]);
                matchInfos.RemoveAt(0);

                onWriteStatus(getLogTitle() + string.Format("Request: [{0}] [{1}]", activeInfo.matchInfo.sport, activeInfo.matchInfo.name));
                sendMatchRequest(activeInfo.matchInfo.id);

                activeInfo.response = false;
            }
        }

        private void sendMatchRequest(string id)
        {
            if (!bUnSubscribed)
            {
                // Unsubscribe
                foreach (string groupId in groupListLive)
                {
                    string param = string.Format(Resource1.Param10, groupId);
                    SendMessage(param);
                }

                SendMessage(Resource1.Param11);
                SendMessage(Resource1.Param12);

                bUnSubscribed = true;
            }

            SendMessage(string.Format(Resource1.Param8, id, id));
        }

        private void parseSocketResponseMarkets(string response)
        {
            string json = getJsonFromResponse(response);
            if(string.IsNullOrEmpty(json))
                return;

            List<RunnerInfo> runners = new List<RunnerInfo>();

            JsonLBMarketResponse result = JsonConvert.DeserializeObject<JsonLBMarketResponse>(json);
            if (result == null || result.markets == null || result.markets.Count < 1)
                return;

            foreach(JsonLBMarket jsonMarket in result.markets)
            {
                RunnerInfo runner = new RunnerInfo();
                runner.id = Utils.ParseToLong(jsonMarket.id);
                runner.name = jsonMarket.nameTranslations.unpiped;
                
                foreach(JsonLBSelection jsonSel in jsonMarket.selections)
                {
                    if (jsonSel.prices == null || jsonSel.prices.Count < 1)
                        continue;

                    PriceInfo price = new PriceInfo();
                    price.odds = Utils.getOdds(jsonSel.prices[0].fractionalOdds) + 1;

                    runner.prices.Add(price);
                }

                runners.Add(runner);
            }

            activeInfo.response = true;

            MatchInfo arbInfo = new MatchInfo();
            arbInfo.bookie = BOOKIE.Ladbrokes;
            arbInfo.setArbInfo(activeInfo.matchInfo, runners);

            infos.Add(arbInfo);
        }

        private void parseJsonEvent(string message, SPORT sport)
        {
            try
            {
                string json = getJsonFromResponse(message);
                if (string.IsNullOrEmpty(json))
                    return;

                JsonEventGroup result = JsonConvert.DeserializeObject<JsonEventGroup>(json);
                if (result == null || result.list == null || result.list.Count < 1)
                    return;

                foreach(JsonEventGroupList groupList in result.list)
                {
                    if (groupList.eve == null)
                        continue;

                    // check sports and leagues here
                    if (sport == SPORT.Horse || sport == SPORT.Dog)
                    {
                        TempMatchInfo matchInfo = new TempMatchInfo();
                        matchInfo.id = groupList.id;
                        matchInfo.sport = sport;
                        matchInfo.name = groupList.eve.nameTranslations.unpiped;

                        matchInfos.Add(matchInfo);

                        continue;
                    }
                }
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (parseJsonEvent) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
            }
        }

        private List<string> getEventGroupList(string message)
        {
            List<string> groupList = new List<string>();

            try
            {
                string json = getJsonFromResponse(message);

                JsonGroupList result = JsonConvert.DeserializeObject<JsonGroupList>(json);
                if (result == null || result.itemIds == null || result.itemIds.Count < 1)
                    return groupList;

                foreach (JsonGroupItem item in result.itemIds)
                {
                    groupList.Add(item.id);
                }
            }
            catch(Exception e)
            {

            }

            return groupList;
        }

        private string getJsonFromResponse(string message)
        {
            string json = string.Empty;

            GroupCollection groups = Regex.Match(message, "(?<json>{.*})").Groups;
            if (groups == null || groups["json"] == null)
                return json;

            json = groups["json"].Value;
            if (string.IsNullOrEmpty(json))
                return json;

            json = json.Replace("\\\"", "\"").Trim();

            return json;
        }

        public void SendMessage(string message)
        {
            SendMessageAsync(message);
        }

        private void SendMessageAsync(string message)
        {
            if (ws.State != WebSocketState.Open)
            {
                throw new Exception("Connection is not open.");
            }

            var messageBuffer = Encoding.UTF8.GetBytes(message);
            var messagesCount = (int)Math.Ceiling((double)messageBuffer.Length / SendChunkSize);

            for (var i = 0; i < messagesCount; i++)
            {
                var offset = (SendChunkSize * i);
                var count = SendChunkSize;
                var lastMessage = ((i + 1) == messagesCount);

                if ((count * (i + 1)) > messageBuffer.Length)
                {
                    count = messageBuffer.Length - offset;
                }

                ws.SendAsync(new ArraySegment<byte>(messageBuffer, offset, count), WebSocketMessageType.Text, lastMessage, _cancellationToken).Wait();
            }
        }

        private string getLogTitle()
        {
            return "[Ladbrokes] ";
        }
    }
}
