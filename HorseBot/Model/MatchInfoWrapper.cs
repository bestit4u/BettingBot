using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Model
{
    public class MatchInfoWrapper
    {
        public TempMatchInfo matchInfo { get; set; }
        public bool response { get; set; }

        public MatchInfoWrapper()
        {
            matchInfo = new TempMatchInfo();
            response = true;
        }

        public void setMatchInfoWrapper(TempMatchInfo info)
        {
            matchInfo.id = info.id;
            matchInfo.sport = info.sport;
            matchInfo.name = info.name;
            matchInfo.time = info.time;
            matchInfo.td = info.td;
            matchInfo.tm = info.tm;
            matchInfo.ts = info.ts;
            matchInfo.tt = info.tt;

            response = true;
        }
    }
}
