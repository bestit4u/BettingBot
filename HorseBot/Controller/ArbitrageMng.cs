using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Betburger.Model;
using Betburger.Constant;

namespace Betburger.Controller
{
    public class ArbitrageMng
    {
        private static ArbitrageMng _instance = null;
        private onSendArbPairListEvent onSendArbPairList;

        public static ArbitrageMng Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ArbitrageMng();
                }

                return _instance;
            }
        }

        public List<MatchInfo> infoBet365 { get; set; }
        public List<MatchInfo> infoLadbrokes { get; set; }
        public List<MatchInfo> infoBetdaq { get; set; }
        public List<MatchInfo> infoBetfair { get; set; }
        public List<MatchInfo> infoMatchbook { get; set; }

        public ArbitrageMng()
        {
            infoBet365 = new List<MatchInfo>();
            infoLadbrokes = new List<MatchInfo>();
            infoBetdaq = new List<MatchInfo>();
            infoBetfair = new List<MatchInfo>();
            infoMatchbook = new List<MatchInfo>();
        }

        public void setEvent(onSendArbPairListEvent _onSendArbPairList)
        {
            onSendArbPairList = _onSendArbPairList;
        }
        
        public void setBet365List(List<MatchInfo> infos)
        {
            infoBet365.Clear();
            foreach (MatchInfo info in infos)
                infoBet365.Add(info);
        }

        public void setLadbrokesList(List<MatchInfo> infos)
        {
            infoLadbrokes.Clear();
            foreach (MatchInfo info in infos)
                infoLadbrokes.Add(info);
        }

        public void setBetdaqList(List<MatchInfo> infos)
        {
            infoBetdaq.Clear();
            foreach (MatchInfo info in infos)
                infoBetdaq.Add(info);
        }

        public void setBetfairList(List<MatchInfo> infos)
        {
            infoBetfair.Clear();
            foreach (MatchInfo info in infos)
                infoBetfair.Add(info);
        }

        public void setMatchbookList(List<MatchInfo> infos)
        {
            infoMatchbook.Clear();
            foreach (MatchInfo info in infos)
                infoMatchbook.Add(info);
        }

        
        public void doWork()
        {
            try
            {
                List<KeyValuePair<ArbInfo, ArbInfo>> arbPairList = new List<KeyValuePair<ArbInfo,ArbInfo>>();

                // bet365
                
                lock(infoBet365)
                {
                    foreach (MatchInfo info in infoBet365)
                    {
                        
                    }
                }

                // ladbrokes

                lock (infoLadbrokes)
                {
                    foreach (MatchInfo info in infoLadbrokes)
                    {
                    }
                }

                onSendArbPairList(arbPairList);
            }
            catch(Exception e)
            {

            }
        }
    }
}
