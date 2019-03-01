using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Betburger.Json;
using Betburger.Constant;

namespace Betburger.Model
{
    public class MatchInfo
    {
        public BOOKIE bookie { get; set; }
        public string id { get; set; }
        public SPORT sport { get; set; }
        public string country { get; set; }
        public string name { get; set; }
        public List<RunnerInfo> runners { get; set; }

        public MatchInfo()
        {
            runners = new List<RunnerInfo>();
        }

        public void setArbInfo(MatchInfo _info)
        {
            bookie = _info.bookie;
            id = _info.id;
            sport = _info.sport;
            name = _info.name;
            runners = _info.runners;
        }

        public void setArbInfo(TempMatchInfo _info, List<RunnerInfo> _runners)
        {
            bookie = _info.bookie;
            id = _info.id;
            sport = _info.sport;
            name = _info.name;
            runners = _runners;
        }
    }
}
