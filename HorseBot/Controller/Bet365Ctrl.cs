using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

using HtmlAgilityPack;

using Betburger.Constant;
using System.Text.RegularExpressions;
using WebSocket4Net;
using System.Threading;
using Betburger.Model;
using System.IO;

namespace Betburger.Controller
{
    public class Bet365Ctrl
    {
        private HttpClient httpClient = null;
        private WebSocket socket = null;
        private CookieContainer cookieContainer = null;
        private string strDomain;
        private string strPort;
        private string strSession;

        private List<TempMatchInfo> matchInfos = new List<TempMatchInfo>();
        private MatchInfoWrapper activeInfo = new MatchInfoWrapper();
        private List<MatchInfo> infos = new List<MatchInfo>();

        private onWriteStatusEvent onWriteStatus = null;
        private onSendMatchEvent onSendMatchBet365 = null;
        private onWriteLogEvent onWriteLog = null;

        public Bet365Ctrl(onWriteStatusEvent _onWriteStatus, onSendMatchEvent _onSendMatchBet365, onWriteLogEvent _onWriteLog)
        {
            onWriteStatus = _onWriteStatus;
            onSendMatchBet365 = _onSendMatchBet365;
            onWriteLog = _onWriteLog;

            httpClient = getHttpClient();
        }

        private HttpClient getHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            cookieContainer = new CookieContainer();
            handler.CookieContainer = cookieContainer;

            HttpClient httpClientEx = new HttpClient(handler);
            httpClientEx.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html, application/xhtml+xml, application/xml; q=0.9, image/webp, */*; q=0.8");
            httpClientEx.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.111 Safari/537.36");
            httpClientEx.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
            httpClientEx.DefaultRequestHeaders.ExpectContinue = false;

            return httpClientEx;
        }

        public void doWork()
        {
            try
            {
                onWriteStatus(getLogTitle() + "Socket openning...");

                if(!doOpenSocket())
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
                HttpResponseMessage response = httpClient.GetAsync("https://mobile.bet365.com").Result;
                response.EnsureSuccessStatusCode();

                string strContent = response.Content.ReadAsStringAsync().Result;

                string strPatsession = "\"sessionId\":\"(?<val>[^\"]*)\"";
                Match sessionMath = Regex.Match(strContent, strPatsession);
                strSession = sessionMath.Groups["val"].Value;
                if (string.IsNullOrEmpty(strSession))
                    return false;

                string strPat = "{\"Host\":\"wss://(?<Host>[^\"]*)\",\"Port\":(?<Port>[^\\,]*),\"TransportMethod\":(?<Transport>[^\\,]*),\"DefaultTopic\":\"(?<Default>[^\"]*)\"}";
                Match math = Regex.Match(strContent, strPat);
                strDomain = math.Groups["Host"].Value;
                strPort = math.Groups["Port"].Value;
                if (string.IsNullOrEmpty(strDomain) || string.IsNullOrEmpty(strPort))
                    return false;

                doGetEvents();

                return true;
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (doOpenSocket) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
                return false;
            }
        }

        private void doGetEvents()
        {
            infos.Clear();

            try
            {
                Thread.Sleep(1000);

                List<KeyValuePair<string, string>> customHeaders = new List<KeyValuePair<string, string>>();
                customHeaders.Add(new KeyValuePair<string, string>("Origin", "https://mobile.bet365.com"));
                customHeaders.Add(new KeyValuePair<string, string>("Sec-WebSocket-Protocol", "zap-protocol-v1"));
                customHeaders.Add(new KeyValuePair<string, string>("Sec-WebSocket-Extensions", "permessage-deflate; client_max_window_bits"));
                customHeaders.Add(new KeyValuePair<string, string>("Sec-WebSocket-Version", "13"));
                customHeaders.Add(new KeyValuePair<string, string>("Connection", "Upgrade"));
                customHeaders.Add(new KeyValuePair<string, string>("Upgrade", "websocket"));
                customHeaders.Add(new KeyValuePair<string, string>("Accept-Encoding", "gzip, deflate, sdch, br"));
                socket = new WebSocket("wss://" + strDomain + ":" + strPort + "/zap/?uid=" + Utils.getRandomNumberString(), "zap-protocol-v1", null, customHeaders);

                socket.Opened += socket_Opened;
                socket.Error += socket_Error;
                socket.Closed += socket_Closed;
                socket.MessageReceived += socket_Handshake;

                socket.Open();
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (doGetEvents) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
            }
        }

        private void socket_Handshake(object sender, MessageReceivedEventArgs e)
        {
            socket.MessageReceived -= socket_Handshake;
            socket.MessageReceived += socket_MessageReceived;

            string strParm2 = string.Format("{0}{1}CONFIG_1_3,OVInPlay_1_3{2}", ((char)0x16).ToString(), ((char)0x00).ToString(), ((char)0x01).ToString());
            socket.Send(strParm2);
        }

        private void socket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {
            if(!Constants.bRun)
            {
                disconnect();
                return;
            }

            string strMsg = e.Message.Text;

            if (e.Message.Text.Length > 200)
            {
                byte[] header = { 31, 139, 8, 0, 0, 0, 0, 0, 4, 0 };

                List<byte> arrdata = new List<byte>();
                arrdata.AddRange(header);
                arrdata.AddRange(e.Message.Data);

                try
                {
                    strMsg = Utils.Unzip(arrdata.ToArray());
                }
                catch
                {
                    strMsg = e.Message.Text;
                }
            }

            // parsing socket response
            parseSocketResponse(strMsg);
        }

        private void socket_Closed(object sender, EventArgs e)
        {
            onWriteStatus(getLogTitle() + "Socket was closed, reconnecting...");

            if(Constants.bRun)
                doGetEvents();
        }

        private void socket_Error(object sender, SuperSocket.ClientEngine.ErrorEventArgs e)
        {
            try
            {
                onWriteStatus(getLogTitle() + "Socket Error! reconnecting...");

                if (Constants.bRun)
                {
                    disconnect();
                    doOpenSocket();
                }
            }
            catch(Exception ex)
            {
                onWriteLog(string.Format("[{0}]{1} (socket_Error) ---> {2}", DateTime.Now.ToString(), getLogTitle(), ex.ToString()));
            }
        }

        private void disconnect()
        {
            try
            {
                if (socket != null)
                    socket.Close();
            }
            catch(Exception e)
            {
                onWriteLog(string.Format("[{0}]{1} (disconnect) ---> {2}", DateTime.Now.ToString(), getLogTitle(), e.ToString()));
            }
        }

        private void socket_Opened(object sender, EventArgs e)
        {
            onWriteStatus(getLogTitle() + "Socket opened successfully!");

            string strParm1 = string.Format("#{0}P{1}__time,S_{2}{3}	", ((char)0x03).ToString(), ((char)0x01).ToString(), strSession, ((char)0x00).ToString());

            socket.Send(strParm1);
        }

        private string getLogTitle()
        {
            return "[Bet365] ";
        }

        private void parseSocketResponse(string response)
        {
            //LogToFile("log.txt", response);

            if (response.Contains("OVInPlay_1_3"))
            {
                bool bEvent = false;
                if (response.Contains("NA=Horse Racing;"))
                {
                    string temp = response.Substring(response.LastIndexOf("NA=Horse Racing;"));
                    matchInfos.AddRange(parseSocketResponseHorse(temp));
                    bEvent = true;
                }

                if (response.Contains("NA=Greyhounds;"))
                {
                    string temp = response.Substring(response.LastIndexOf("NA=Greyhounds;"));
                    matchInfos.AddRange(parseSocketResponseDog(temp));
                    bEvent = true;
                }

                if(bEvent)
                    onWriteStatus(getLogTitle() + string.Format("There are {0} live events now...", matchInfos.Count));
            }
            else if (!string.IsNullOrEmpty(activeInfo.matchInfo.id) && response.Contains(activeInfo.matchInfo.id)) // && response.Contains(activeInfo.matchInfo.name)
            {
                onWriteStatus(getLogTitle() + string.Format("Response: [{0}] [{1}]", activeInfo.matchInfo.sport, activeInfo.matchInfo.name));
                parseSocketResponseMarkets(response);

                if (matchInfos.Count == 0)
                {
                    onSendMatchBet365(infos);
                    disconnect();
                }

                return;
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

        #region Parse Response
        private List<TempMatchInfo> parseSocketResponseHorse(string response)
        {
            List<TempMatchInfo> infos = new List<TempMatchInfo>();
            string temp = response.Substring(response.LastIndexOf("NA=Horse Racing;"));

            bool bSoccer = false;

            TempMatchInfo info = new TempMatchInfo();

            while(temp.Contains(";"))
            {
                string semicolonString = temp.Substring(0, temp.IndexOf(";"));
                temp = temp.Substring(temp.IndexOf(";") + 1, temp.Length - temp.IndexOf(";") - 1);

                if (semicolonString == "NA=Horse Racing")
                {
                    bSoccer = true;
                }
                else if(bSoccer)
                {
                    if(semicolonString.Contains("NA="))
                    {
                        string naString = semicolonString.Replace("NA=", string.Empty);
                        info.name = naString;
                    }
                    else if (semicolonString.Contains("TU="))
                    {
                        info.time = semicolonString.Replace("TU=", string.Empty);
                    }
                    else if (semicolonString.Contains("TD="))
                    {
                        info.td = semicolonString.Replace("TD=", string.Empty);
                    }
                    else if (semicolonString.Contains("TM="))
                    {
                        info.tm = semicolonString.Replace("TM=", string.Empty);
                    }
                    else if (semicolonString.Contains("TT="))
                    {
                        info.tt = semicolonString.Replace("TT=", string.Empty);
                    }
                    else if (semicolonString.Contains("TS="))
                    {
                        info.ts = semicolonString.Replace("TS=", string.Empty);
                    }
                    else if (semicolonString.Contains("ID="))
                    {
                        string id = semicolonString.Replace("ID=", string.Empty);
                        if (id.Contains("_"))
                        {
                            //info.id = Regex.Replace(id, "_\\d_\\d", string.Empty);
                            info.id = id;
                        }
                    }

                    if (info.isComplete())
                    {
                        info.sport = SPORT.Horse;
                        infos.Add(info);

                        info = new TempMatchInfo();
                    }
                }
            }

            return infos;
        }

        private List<TempMatchInfo> parseSocketResponseDog(string response)
        {
            List<TempMatchInfo> infos = new List<TempMatchInfo>();
            string temp = response.Substring(response.LastIndexOf("NA=Greyhounds;"));

            bool bBasketball = false;

            TempMatchInfo info = new TempMatchInfo();

            while (temp.Contains(";"))
            {
                string semicolonString = temp.Substring(0, temp.IndexOf(";"));
                temp = temp.Substring(temp.IndexOf(";") + 1, temp.Length - temp.IndexOf(";") - 1);

                if (semicolonString == "NA=Greyhounds")
                {
                    bBasketball = true;
                }
                else if (bBasketball)
                {
                    if (semicolonString.Contains("NA="))
                    {
                        string naString = semicolonString.Replace("NA=", string.Empty);
                        info.name = naString;
                    }
                    else if (semicolonString.Contains("TU="))
                    {
                        info.time = semicolonString.Replace("TU=", string.Empty);
                    }
                    else if (semicolonString.Contains("TD="))
                    {
                        info.td = semicolonString.Replace("TD=", string.Empty);
                    }
                    else if (semicolonString.Contains("TM="))
                    {
                        info.tm = semicolonString.Replace("TM=", string.Empty);
                    }
                    else if (semicolonString.Contains("TT="))
                    {
                        info.tt = semicolonString.Replace("TT=", string.Empty);
                    }
                    else if (semicolonString.Contains("TS="))
                    {
                        info.ts = semicolonString.Replace("TS=", string.Empty);
                    }
                    else if (semicolonString.Contains("ID="))
                    {
                        string id = semicolonString.Replace("ID=", string.Empty);
                        if (id.Contains("_"))
                        {
                            info.id = id;
                        }
                    }

                    if (info.isComplete())
                    {
                        info.sport = SPORT.Dog;
                        infos.Add(info);
                        info = new TempMatchInfo();
                    }
                }
            }

            return infos;
        }


        private void parseSocketResponseMarkets(string response)
        {
            List<RunnerInfo> runners = new List<RunnerInfo>();

            string temp = response;

            while (response.Contains("|PA;"))
            {
                string responseString = response.Substring(response.LastIndexOf("|PA;"));
                response = response.Replace(responseString, string.Empty);

                RunnerInfo runner = new RunnerInfo();
                PriceInfo price = new PriceInfo();

                temp = responseString;
                while (temp.Contains(";"))
                {
                    string semicolonString = temp.Substring(0, temp.IndexOf(";"));
                    temp = temp.Substring(temp.IndexOf(";") + 1, temp.Length - temp.IndexOf(";") - 1);

                    if (semicolonString.Contains("NA="))
                    {
                        string naString = semicolonString.Replace("NA=", string.Empty);
                        if (string.IsNullOrEmpty(naString))
                            continue;

                        runner.name = naString;
                    }
                    else if(semicolonString.Contains("ID="))
                    {
                        runner.id = Utils.ParseToLong(semicolonString.Replace("ID=", ""));
                    }
                    else if (semicolonString.Contains("OD="))
                    {
                        price.odds = Utils.getOdds(semicolonString.Replace("OD=", "")) + 1;

                        if (string.IsNullOrEmpty(runner.name))
                            runner.prices.Add(price);

                        break;
                    }
                }

                runners.Add(runner);
            }

            activeInfo.response = true;

            MatchInfo arbInfo = new MatchInfo();
            arbInfo.bookie = BOOKIE.Bet365;
            arbInfo.setArbInfo(activeInfo.matchInfo, runners);
            infos.Add(arbInfo);
        }

        #endregion

        private void sendMatchRequest(string id)
        {
            socket.Send(string.Format("{0}{1}{3}{2}", ((char)0x16).ToString(), ((char)0x00).ToString(), ((char)0x01).ToString(), id));
        }
    }
}
