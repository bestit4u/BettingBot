using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Betburger.Model;
using Betburger.Json;
using Betburger.Constant;

using Betdaq_Api;

namespace Betburger.Controller
{
    public class BetdaqCtrl
    {
        private onWriteStatusEvent onWriteStatus = null;
        private onSendMatchEvent onSendMatchBetdaq = null;

        private Betdaq client { get; set; }

        public BetdaqCtrl(onWriteStatusEvent _onWriteStatus, onSendMatchEvent _onSendMatch)
        {
            onWriteStatus = _onWriteStatus;
            onSendMatchBetdaq = _onSendMatch;

            client = new Betdaq();
        }

        public void doWork()
        {
            List<MatchInfo> matchInfoList = getMatchInfos();
            if (matchInfoList.Count < 1)
                return;

            onSendMatchBetdaq(matchInfoList);
        }

        public List<MatchInfo> getMatchInfos()
        {
            List<MatchInfo> infos = new List<MatchInfo>();

            try
            {
                string jsonEventListString = client.GetEventList().ToString();
                string jsonHorseString = client.GetEvents((long)100004).ToString();

                Betburger.Json.Betdaq.JsonEvent jsonHorse = JsonConvert.DeserializeObject<Betburger.Json.Betdaq.JsonEvent>(jsonHorseString);
                if (jsonHorse == null || jsonHorse.Events == null)
                    return infos;

                foreach(Betburger.Json.Betdaq.JsonEvent item in jsonHorse.Events)
                {
                    if(item.Events == null)
                        continue;

                    foreach(Betburger.Json.Betdaq.JsonEvent subItem in item.Events)
                    {
                        if (subItem.Events == null)
                            continue;

                        foreach(Betburger.Json.Betdaq.JsonEvent subsubItem in subItem.Events)
                        {
                            if (subsubItem.Markets == null || subsubItem.Markets.Count < 1)
                                continue;

                            if (!subsubItem.Markets[0].IsCurrentlyInRunning)
                                continue;

                            // Current Running Market
                            MatchInfo matchInfo = new MatchInfo();
                            matchInfo.id = subsubItem.EventId.ToString();
                            matchInfo.name = subsubItem.Name;
                            matchInfo.runners = getRunnersFromMarketId(subsubItem.Markets[0].MarketId);

                            infos.Add(matchInfo);
                        }
                    }
                }
            }
            catch(Exception e)
            {

            }

            return infos;
        }

        private List<RunnerInfo> getRunnersFromMarketId(long marketId)
        {
            List<RunnerInfo> runners = new List<RunnerInfo>();

            try
            {
                string jsonSelectionString = client.GetInfo("price", (int)marketId).ToString();
                if (string.IsNullOrEmpty(jsonSelectionString))
                    return runners;

                Betburger.Json.Betdaq.JsonMarket jsonMarket = JsonConvert.DeserializeObject<Betburger.Json.Betdaq.JsonMarket>(jsonSelectionString);
                if (jsonMarket == null || jsonMarket.Selections == null)
                    return runners;

                foreach(Betburger.Json.Betdaq.JsonSelection jsonSelection in jsonMarket.Selections)
                {
                    RunnerInfo runner = new RunnerInfo();
                    runner.id = jsonSelection.Id;
                    runner.name = jsonSelection.Name;

                    foreach(Betburger.Json.Betdaq.JsonPrice jsonPrice in jsonSelection.PricesFor)
                    {
                        PriceInfo price = new PriceInfo();
                        price.selectionId = marketId;
                        price.odds = jsonPrice.Odds;
                        price.stake = jsonPrice.Stake;
                        price.side = SIDE.Back;

                        runner.prices.Add(price);
                    }

                    foreach (Betburger.Json.Betdaq.JsonPrice jsonPrice in jsonSelection.PricesFor)
                    {
                        PriceInfo price = new PriceInfo();
                        price.selectionId = marketId;
                        price.odds = jsonPrice.Odds;
                        price.stake = jsonPrice.Stake;
                        price.side = SIDE.Lay;

                        runner.prices.Add(price);
                    }

                    runners.Add(runner);
                }
            }
            catch(Exception e)
            {

            }

            return runners;
        }

        public bool doPlace(string marketId, string selectionId, string polarity, string amount, string odds)
        {
            try
            {
                Dictionary<string, object> dictionary1 = new Dictionary<string, object>();
                dictionary1.Add("type", (object)"placebets");
                Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
                dictionary2.Add("UserId", (object)Setting.Instance.usernameBetdaq);
                dictionary2.Add("Password", (object)Setting.Instance.passwordBetdaq);
                dictionary2.Add("WantAllOrNothing", (object)true);
                dictionary2.Add("MarketId", (object)marketId);
                Dictionary<string, string>[] dictionaryArray = new Dictionary<string, string>[1]
                {
                    new Dictionary<string, string>()
                    {
                        { "SelectionId", selectionId },
                        { "Polarity", polarity },
                        { "Amount", amount },
                        { "Odds", odds },
                        { "ResetCount", "1" }
                    }
                };
                dictionary2.Add("table", (object)dictionaryArray);
                dictionary1.Add("values", (object)dictionary2);
                string strPost = JsonConvert.SerializeObject((object)dictionary1);
                string jsonPlaceString = client.Post((object)strPost).ToString();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
