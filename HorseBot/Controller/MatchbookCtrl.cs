using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Matchbook;
using Betburger.Model;
using Betburger.Constant;

namespace Betburger.Controller
{
    public class MatchbookCtrl
    {
        private onWriteStatusEvent onWriteStatus;
        private onSendMatchEvent onSendMatch;

        private MatchbookClient client = null;

        public MatchbookCtrl(onWriteStatusEvent _onWriteStatus, onSendMatchEvent _onSendMatch)
        {
            onWriteStatus = _onWriteStatus;
            onSendMatch = _onSendMatch;

            client = new MatchbookClient(Setting.Instance.usernameMatchbook, Setting.Instance.passwordMatchbook);
        }

        public void doWork()
        {
            if (!client.Login())
                return;

            List<MatchInfo> infos = new List<MatchInfo>();

            infos.AddRange(getMatchInfoList(SPORT.Horse));
            infos.AddRange(getMatchInfoList(SPORT.Dog));

            onSendMatch(infos);
        }

        private List<MatchInfo> getMatchInfoList(SPORT sport)
        {
            List<MatchInfo> infos = new List<MatchInfo>();

            try
            {
                string jsonHorseString = client.GetEventsById(sport == SPORT.Horse ? 241798357140019 : 24735152712200);

                Betburger.Json.Matchbook.JsonResult jsonResult = JsonConvert.DeserializeObject<Betburger.Json.Matchbook.JsonResult>(jsonHorseString);
                if (jsonResult == null || jsonResult.events == null)
                    return infos;

                foreach(Betburger.Json.Matchbook.JsonEvent jsonEvent in jsonResult.events)
                {
                    if(jsonEvent == null || jsonEvent.markets == null || jsonEvent.markets.Count < 1)
                        continue;

                    MatchInfo info = new MatchInfo();
                    info.id = jsonEvent.id.ToString();
                    info.name = jsonEvent.name;
                    info.sport = sport;
                    info.bookie = BOOKIE.Matchbook;
                    
                    foreach(Betburger.Json.Matchbook.JsonRunner jsonRunner in jsonEvent.markets[0].runners)
                    {
                        RunnerInfo runner = new RunnerInfo();
                        runner.id = jsonRunner.id;
                        runner.name = jsonRunner.name;

                        foreach(Betburger.Json.Matchbook.JsonPrice jsonPrice in jsonRunner.prices)
                        {
                            PriceInfo price = new PriceInfo();
                            price.odds = jsonPrice.odds;
                            price.stake = jsonPrice.maxStake;
                            price.side = jsonPrice.side == "back" ? SIDE.Back : SIDE.Lay;

                            runner.prices.Add(price);
                        }

                        info.runners.Add(runner);
                    }

                    infos.Add(info);
                }
            }
            catch(Exception e)
            {

            }

            return infos;
        }

        public Status doPlace(long runnerId, string side, double odds, double stake)
        {
            return client.SubmitOffers(runnerId, side, odds, stake);
        }
    }
}
