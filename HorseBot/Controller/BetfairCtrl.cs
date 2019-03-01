using Betburger.Constant;
using Betburger.Json;
using Betburger.Model;
using BetfairNG;
using BetfairNG.Data;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Betburger.Controller
{
    public class BetfairCtrl
    {
        private onWriteStatusEvent onWriteStatus = null;
        private onSendMatchEvent onSendMatchBetfair;
        
        private HttpClient httpClient = null;
        private CookieContainer container = null;
        private BetfairClient clientDelay = null;
        private BetfairClient clientActive = null;
        private List<SelectionInfo> selectionInfoList = new List<SelectionInfo>();
        private Dictionary<string, Event> marketEvents = new Dictionary<string, Event>();
        private Dictionary<long, string> selectionRunners = new Dictionary<long, string>();

        public string sessionToken = null;
        private string token = string.Empty;
        private string merchantId = string.Empty;
        private double balance = 0;

        public BetfairCtrl(onWriteStatusEvent _onWriteStatus, onSendMatchEvent _onSendMatchBetfair)
        {
            if (httpClient == null)
                initHttpClient();

            onWriteStatus = _onWriteStatus;
            onSendMatchBetfair = _onSendMatchBetfair;
        }

        private void initApi()
        {
            clientDelay = new BetfairClient(Setting.Instance.delayKey, sessionToken);
            clientActive = new BetfairClient(Setting.Instance.realKey, sessionToken);
        }

        private void initHttpClient()
        {
            container = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = container;
            httpClient = new HttpClient(handler);

            initHttpHeader();
        }

        private void initHttpHeader()
        {
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept",
                "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            ;
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent",
                "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Upgrade-Insecure-Requests", "1");
        }

        public async Task<List<Bet>> getBetHistory(List<string> betIds)
        {
            List<Bet> bets = new List<Bet>();
            List<JsonExchangeBet> betfairBets = new List<JsonExchangeBet>();

            try
            {
                httpClient.DefaultRequestHeaders.Referrer = new Uri("https://myaccount.betfair.com/summary/accountsummary");
                HttpResponseMessage responseMessageActivity = await httpClient.GetAsync("https://myactivity.betfair.com/");
                responseMessageActivity.EnsureSuccessStatusCode();

                httpClient.DefaultRequestHeaders.Remove("Accept");
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json, text/plain, */*");

                string postParam = string.Format("{{\"status\":\"SETTLED\",\"fromRecord\":0,\"pageSize\":100,\"selectedBets\":\"MAIN\",\"state\":\"SETTLED\",\"eventTypeId\":null,\"dateFilter\":null,\"startDate\":\"{0}\",\"endDate\":\"{1}\"}}", Constants.start.ToString("yyyy-MM-dd"), DateTime.UtcNow.ToString("yyyy-MM-dd"));
                httpClient.DefaultRequestHeaders.Referrer = new Uri("https://myactivity.betfair.com/");
                HttpResponseMessage responseMessageBetHistory = await httpClient.PostAsync("https://myactivity.betfair.com/activity/exchange/settled", (HttpContent)new StringContent(postParam, Encoding.UTF8, "application/json"));
                responseMessageBetHistory.EnsureSuccessStatusCode();

                string responseMessageBetHistoryString = await responseMessageBetHistory.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseMessageBetHistoryString))
                    return bets;

                JsonExchangeResult result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonExchangeResult>(responseMessageBetHistoryString);
                if (result != null && result.bets != null && result.bets.Count > 0)
                    betfairBets.AddRange(result.bets);

                postParam = string.Format("{{\"status\":\"OPEN\",\"fromRecord\":0,\"pageSize\":100,\"selectedBets\":\"MAIN\",\"state\":\"MATCHED\",\"firstView\":false,\"dateFilter\":null,\"startDate\":\"{0}\",\"endDate\":\"{1}\"}}", Constants.start.ToString("yyyy-MM-dd"), DateTime.UtcNow.ToString("yyyy-MM-dd"));
                responseMessageBetHistory = await httpClient.PostAsync("https://myactivity.betfair.com/activity/exchange/open", (HttpContent)new StringContent(postParam, Encoding.UTF8, "application/json"));
                responseMessageBetHistory.EnsureSuccessStatusCode();

                responseMessageBetHistoryString = await responseMessageBetHistory.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseMessageBetHistoryString))
                    return bets;

                result = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonExchangeResult>(responseMessageBetHistoryString);
                if (result != null && result.bets != null && result.bets.Count > 0)
                    betfairBets.AddRange(result.bets);

                foreach (JsonExchangeBet jsonExchangeBet in betfairBets)
                {
                    if (!betIds.Contains(jsonExchangeBet.betId))
                        continue;

                    Bet bet = new Bet();
                    bet.betId = jsonExchangeBet.betId;
                    DateTime now = DateTime.Now;
                    DateTime.TryParse(jsonExchangeBet.placedDate, out now);
                    bet.datetime = now;
                    bet.time = now.ToString("yyyy-MM-dd HH:mm:ss");
                    bet.match = jsonExchangeBet.eventDescription;
                    bet.type = jsonExchangeBet.side;
                    bet.odds = jsonExchangeBet.odds;
                    bet.stake = jsonExchangeBet.stake;
                    bet.status = jsonExchangeBet.status;

                    bets.Add(bet);
                }

                List<Bet> orderedBets = bets.OrderByDescending(o => o.datetime).ToList();

                return orderedBets;
            }
            catch (Exception e)
            {
                return bets;
            }
        }

        private bool getCurrentBalance()
        {
            try
            {
                AccountFundsResponse fResp = clientDelay.GetAccountFunds(Wallet.UK).Result.Response;
                balance = fResp.AvailableToBetBalance;
            }
            catch (APINGException e)
            {
                //LogString(e.Message);
                if (e.ErrorCode.Contains("INVALID_SESSION_INFORMATION"))
                {

                }
                return false;
            }
            catch (Exception ee)
            {
                //LogString(ee.Message);
                return false;
            }

            return true;
        }

        public async Task<double> getBalance()
        {
            try
            {
                HttpResponseMessage responseMessage = await httpClient.GetAsync("https://myaccount.betfair.com/wallet-service/v3.0/wallets?walletNames=[MAIN]&alt=json");
                responseMessage.EnsureSuccessStatusCode();

                string responseMessageString = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseMessageString))
                    return -1;

                GroupCollection groups = Regex.Match(responseMessageString, "\"amount\":\"(?<balance>(\\d*\\.\\d*|\\d*))\"").Groups;
                if (groups == null || groups["balance"] == null)
                    return -1;

                double balance = -1;
                if (double.TryParse(groups["balance"].Value, out balance))
                    return balance;
                else
                    return -1;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        private bool doLogin()
        {
            try
            {
                initHttpClient();

                HttpResponseMessage responseMessageMain = httpClient.GetAsync("http://www.betfair.com/").Result;
                responseMessageMain.EnsureSuccessStatusCode();

                string mainReferer = responseMessageMain.RequestMessage.RequestUri.AbsoluteUri;
                if (string.IsNullOrEmpty(mainReferer))
                    return false;

                string responseMessageMainString = responseMessageMain.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseMessageMainString))
                    return false;

                HtmlNode.ElementsFlags.Remove("form");
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(responseMessageMainString);

                IEnumerable<HtmlNode> nodeForms = doc.DocumentNode.Descendants("form");
                if (nodeForms == null || nodeForms.LongCount() < 1)
                    return false;

                string action = nodeForms.ToArray()[0].GetAttributeValue("action", "");
                if (string.IsNullOrEmpty(action))
                    return false;

                IEnumerable<HtmlNode> nodeInputs =
                    nodeForms.ToArray()[0].Descendants("input").Where(node => node.Attributes["name"] != null);
                if (nodeInputs == null || nodeInputs.LongCount() < 1)
                    return false;

                string refererUrl = string.Empty;
                List<KeyValuePair<string, string>> inputs = new List<KeyValuePair<string, string>>();
                foreach (HtmlNode nodeInput in nodeInputs)
                {
                    string inputName = nodeInput.GetAttributeValue("name", "");
                    if (string.IsNullOrEmpty(inputName))
                        continue;

                    string inputValue = nodeInput.GetAttributeValue("value", "");
                    if (inputValue == null)
                        inputValue = string.Empty;

                    if (inputName == "username")
                        inputValue = Setting.Instance.usernameBetfair;

                    if (inputName == "password")
                        inputValue = Setting.Instance.passwordBetfair;

                    if (inputName == "ioBlackBox")
                        inputValue =
                            "0400R9HVeoYv1gsNf94lis1ztshEYOhBj8SQVGjDxyKtjEYJx9qSw/0rX3UPUavsVGNNP8qFSAZ42op95eoW86m0kwJH0I8JjLitr3JYlvaDSSoJdOHStouX5odMpSRnzNZujwZOoEVFPaadm2GNPflphIK4mjKAMqULsj69LfM7VRbla+gcYC1FQHWwCqQ/IvlFFmQWppRl6Bv4pp48B2PR0LUM6Rn3JtHEfF9hXdZ4DRRiwxmZVjl9I8/xOY8SaJdW7jj6V+dG7j9MyPpjzYuuV+VhT2JQxvTdTn3ScfrBfYTh6ImRigH/M5hg3W9g0TRnvoLZ8SpzAFLDWCmiO4sAs/qr/BWAZmps3sMxM3Tsu4+Oh9ltHZfQIggPoVjqnTR6FC6A4XHytZ7Tn6b+z24SKe+wPwB7YcyEaf1wmOpmO8iM1eyAwSCEbXPCKc6iDsD88+g/mDVDCxFKZ8U9CkoINzcLZ9/2RBpYQZsdELNYToJhO1Q2jmVgfPrA9XDC0LRFicgHXRoRWukWp8XlE1kYrSaUGABVKw5X0ikb/3h1d0Im6Fy7sPS5usVQSvGb6Q+XXiK1sQAL4CPUyKVnDKqtQrhW1HQIpGtAAfOPjzqyODIf9I+PRImeQ49qpLt2ZL85hw6+vl8ZjUN3p2VjxRMgSIkpIh3A707ln/wqsPiGBcfEpkWTlnMi5g4mmYKxVbRrSUpGCTU5RhDBmCQpzv+TwwM6wvQ9/pjGYj2YJXfnh939vTlgH8k0GP35GUqEwdlQH6AzAyX2sumr8GO9YMyocNGc3h38I509+L9GpM8mFWoWiIYYjbEvXCXVTb29yNd9DKThVP1hAyB7YMdbHsyiYBQ1h4tNGHTh8CNjY0flnyuelCnt2ypgfW/m3Zqxt3w1Bn6LxoWOnZu64rr07LwRGSM2wu7XD+lw1lMSgm8hl0E6JnXlF7diYNwTidneZ5WeqDQRJDWdY1eYOcNXGWh5eh0R9vRNj5evLzdF6XhbUceysRqfEatYu7JOeKvET7v2DVHRyVdfAWJVJqKtzBRNTqF/WnZgA3qvxiWvyGPt4xFJHNu5eG3HXPo1chRhwl6109lnHDNLhXiDaeModH3MykVCcuAzYjmBq2CObWM3m2Jxa2GEjcGvISFrwoQO0Yr3iynFSjKIXJbJoUCUMhuSPrCT16pHvpt1JeGMGYj7gJYGwcZRDWBe40N3RSizQUPUSdu3DZ2D0kFXG69EaoZ1RI0f9uYinjZJCJitlNUbN+M7DqaJyGOl8Gv+HZ+NFeJF+TjyWQJjI3NMC63Cwm7dHkusepnG3VLacD4ALLQF1a0FYqvfxn854SSXEvUuPt0pFsY23fB/+pEjrnavoBjx28jKWCbjdtkiyP5Y9ou+oQuf/feQ29ef/02xPtU9AeQHnnkJD5ghHNv+1znnh45aMwwjA2Ui9cgPlAgeaCqCy3cXjx33ffITvy2wv1MWLu4AO0fwrpVgx/siiN2XyvHCfqE2r+5inDLSN1TCyOxhTq/let4ltP27ta6LO+lD+YmKtgN4Oh10Gib60E9YGc40dTHbDZ5kt2K49+MixLQpfdYCKC/P29Tu8zwBp1GJQgbFvd7r7/TrYV9DpG+vj9JhImGqAbqHUNQ4M2FEG+0RRIU=";

                    if (inputName == "url")
                        refererUrl = inputValue;

                    inputs.Add(new KeyValuePair<string, string>(inputName, inputValue));
                }

                httpClient.DefaultRequestHeaders.Referrer = new Uri(mainReferer);
                HttpResponseMessage responseMessageLogin = httpClient.PostAsync(action, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)inputs)).Result;
                responseMessageLogin.EnsureSuccessStatusCode();

                string responseMessageLoginString = responseMessageLogin.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseMessageLoginString))
                    return false;

                doc.LoadHtml(responseMessageLoginString);

                nodeForms =
                    doc.DocumentNode.Descendants("form")
                        .Where(node => node.Attributes["name"] != null && node.Attributes["name"].Value == "postLogin");
                if (nodeForms == null || nodeForms.LongCount() < 1)
                    return false;

                action = nodeForms.ToArray()[0].GetAttributeValue("action", "");
                if (string.IsNullOrEmpty(action))
                    return false;

                nodeInputs = nodeForms.ToArray()[0].Descendants("input").Where(node => node.Attributes["name"] != null);
                if (nodeInputs == null || nodeInputs.LongCount() < 1)
                    return false;

                inputs = new List<KeyValuePair<string, string>>();
                foreach (HtmlNode nodeInput in nodeInputs)
                {
                    string name = nodeInput.GetAttributeValue("name", "");
                    if (string.IsNullOrEmpty(name))
                        continue;

                    string value = nodeInput.GetAttributeValue("value", "");
                    if (value == null)
                        value = string.Empty;

                    inputs.Add(new KeyValuePair<string, string>(name, value));
                }

                httpClient.DefaultRequestHeaders.Referrer =
                    new Uri(string.Format("https://identitysso.betfair.com/view/login?product=prospect-page&url={0}",
                        WebUtility.UrlEncode(refererUrl)));
                HttpResponseMessage responseMessagePostLogin = httpClient.PostAsync(action, (HttpContent)new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)inputs)).Result;
                responseMessagePostLogin.EnsureSuccessStatusCode();

                string responseMessagePostLoginString = responseMessagePostLogin.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(responseMessagePostLoginString))
                    return false;

                if (!responseMessagePostLoginString.Contains(Setting.Instance.usernameBetfair))
                    return false;

                CookieCollection cookies = container.GetCookies(new Uri("https://www.betfair.com/"));
                if (cookies == null || cookies.Count < 1)
                    return false;

                foreach (Cookie cookie in cookies)
                {
                    if (cookie.Name == "ssoid")
                    {
                        sessionToken = cookie.Value;
                        break;
                    }
                }

                return true;
            }
            catch (APINGException e)
            {
                //                if (e.Message.Contains(""))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void doWork()
        {
            if (string.IsNullOrEmpty(sessionToken) && !doLogin())
            {
                onWriteStatus(getLogTitle() + "Login failed!");
                return;
            }

            initApi();

            List<MatchInfo> eventList = new List<MatchInfo>();

            try
            {
                MarketFilter marketFilter = new MarketFilter();
                ISet<string> eventTypeIds = new HashSet<string>();
                eventTypeIds.Add("7");
                eventTypeIds.Add("4339");
                marketFilter.EventTypeIds = eventTypeIds;
                marketFilter.MarketStartTime = new TimeRange();
                marketFilter.MarketStartTime.From = DateTime.Now;
                marketFilter.MarketStartTime.To = DateTime.Now.AddHours(1); // 한시간후의 경주들을 얻는다.

                ISet<MarketProjection> marketProjects = new HashSet<MarketProjection>();
                marketProjects.Add(MarketProjection.RUNNER_DESCRIPTION);
                marketProjects.Add(MarketProjection.EVENT);

                IList<MarketCatalogue> marketCatalogueList = clientDelay.ListMarketCatalogue(marketFilter, marketProjects, null, 100).Result.Response;
                if (marketCatalogueList == null || marketCatalogueList.Count < 1)
                    return;

                ISet<string> marketIds = new HashSet<string>();
                foreach (MarketCatalogue marketCatalogue in marketCatalogueList)
                {
                    if (marketCatalogue == null || marketCatalogue.Runners == null || marketCatalogue.Runners.Count < 1)
                        continue;

                    marketIds.Add(marketCatalogue.MarketId);
                    addMarketEvent(marketCatalogue.MarketId, marketCatalogue.Event);

                    if (marketCatalogue.Runners == null)
                        continue;

                    foreach (RunnerCatalog runner in marketCatalogue.Runners)
                        addSelectionRunner(runner.SelectionId, runner.RunnerName);
                }

                if (marketIds.Count < 1)
                    return;

                // get listmarketbook
                PriceProjection priceProjection = new PriceProjection();
                priceProjection.PriceData = new HashSet<PriceData>();
                priceProjection.PriceData.Add(PriceData.EX_ALL_OFFERS);
                priceProjection.PriceData.Add(PriceData.EX_BEST_OFFERS);
                priceProjection.PriceData.Add(PriceData.EX_TRADED);
                priceProjection.PriceData.Add(PriceData.SP_AVAILABLE);
                priceProjection.PriceData.Add(PriceData.SP_TRADED);

                int nCnt = 0;
                ISet<string> tempMarketIds = new HashSet<string>();
                List<MarketBook> marketBookList = new List<MarketBook>();

                foreach (string marketId in marketIds)
                {
                    tempMarketIds.Add(marketId);
                    nCnt++;

                    if (nCnt >= 5)
                    {
                        List<MarketBook> tempMarketBookList = clientDelay.ListMarketBook(tempMarketIds, priceProjection).Result.Response;
                        if (tempMarketBookList != null)
                            marketBookList.AddRange(tempMarketBookList);

                        nCnt = 0;
                        tempMarketIds.Clear();
                    }
                }

                if (tempMarketIds.Count > 0)
                {
                    List<MarketBook> tempMarketBookList = clientDelay.ListMarketBook(tempMarketIds, priceProjection).Result.Response;
                    if (tempMarketBookList != null)
                        marketBookList.AddRange(tempMarketBookList);
                }

                if (marketBookList == null || marketBookList.Count < 1)
                    return;

                foreach (MarketBook marketBook in marketBookList)
                {
                    if (!Constants.bRun)
                        break;

                    if (marketBook.Runners == null)
                        continue;

                    MatchInfo matchInfo = new MatchInfo();
                    matchInfo.id = marketBook.MarketId;

                    string eveName = getEventFromMarketId(marketBook.MarketId);
                    if (string.IsNullOrEmpty(eveName))
                        continue;

                    matchInfo.name = eveName;
                    
                    foreach (Runner runner in marketBook.Runners)
                    {
                        if (runner == null || runner.ExchangePrices == null)
                            continue;

                        RunnerInfo runnerInfo = new RunnerInfo();
                        runnerInfo.id = runner.SelectionId;
                        string runnerName = getRunnerFromSelectionId(runner.SelectionId);
                        if (string.IsNullOrEmpty(runnerName))
                            continue;

                        runnerInfo.name = runnerName;
                        
                        if(runner.ExchangePrices.AvailableToBack != null && runner.ExchangePrices.AvailableToBack.Count > 0)
                        {
                            foreach(PriceSize priceSize in runner.ExchangePrices.AvailableToBack)
                            {
                                PriceInfo priceInfo = new PriceInfo();
                                priceInfo.odds = priceSize.Price;
                                priceInfo.side = SIDE.Back;
                                priceInfo.stake = priceSize.Size;

                                runnerInfo.prices.Add(priceInfo);
                            }
                        }

                        if (runner.ExchangePrices.AvailableToLay != null && runner.ExchangePrices.AvailableToLay.Count > 0)
                        {
                            foreach (PriceSize priceSize in runner.ExchangePrices.AvailableToLay)
                            {
                                PriceInfo priceInfo = new PriceInfo();
                                priceInfo.odds = priceSize.Price;
                                priceInfo.side = SIDE.Lay;
                                priceInfo.stake = priceSize.Size;

                                runnerInfo.prices.Add(priceInfo);
                            }
                        }

                        if (runnerInfo.prices.Count > 0)
                            matchInfo.runners.Add(runnerInfo);
                    }

                    if (matchInfo.runners.Count > 0)
                        eventList.Add(matchInfo);
                }
            }
            catch (Exception e)
            {

            }

            if(eventList.Count > 0)
                onSendMatchBetfair(eventList);
        }

        private string getLogTitle()
        {
            return "[Betfair] ";
        }

        // market event management
        private void addMarketEvent(string marketId, Event eve)
        {
            if (!marketEvents.ContainsKey(marketId))
                marketEvents.Add(marketId, eve);
        }

        private string getEventFromMarketId(string marketId)
        {
            if (string.IsNullOrEmpty(marketId))
                return string.Empty;

            if (!marketEvents.ContainsKey(marketId))
                return string.Empty;

            Event eve = marketEvents[marketId];
            if (eve == null)
                return string.Empty;

            DateTime openDT = eve.OpenDate.Value;
            openDT.AddHours(1);

            return string.Format("{0} {1}", openDT.ToString("HH:mm"), eve.Name.Substring(0, eve.Name.LastIndexOf(')') + 1));
        }

        private void addSelectionRunner(long selectionId, string runner)
        {
            if (!selectionRunners.ContainsKey(selectionId))
                selectionRunners.Add(selectionId, runner);
        }

        private string getRunnerFromSelectionId(long selectionId)
        {
            if (!selectionRunners.ContainsKey(selectionId))
                return string.Empty;

            return selectionRunners[selectionId];
        }
    }
}
