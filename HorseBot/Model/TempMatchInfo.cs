using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Betburger.Constant;

namespace Betburger.Model
{
    public class TempMatchInfo
    {
        public string id { get; set; }
        public BOOKIE bookie { get; set; }
        public SPORT sport { get; set; }
        public string name { get; set; }
        public string time { get; set; }
        public string td { get; set; }
        public string tm { get; set; }
        public string ts { get; set; }
        public string tt { get; set; }

        public TempMatchInfo()
        {
            id = string.Empty;
            name = string.Empty;
            time = string.Empty;
            td = string.Empty;
            tm = string.Empty;
            ts = string.Empty;
            tt = "1";
        }

        public bool isComplete()
        {
            if (string.IsNullOrEmpty(name))
                return false;

            if (string.IsNullOrEmpty(id))
                return false;

            if (string.IsNullOrEmpty(td))
                return false;

            if (string.IsNullOrEmpty(tm))
                return false;

            if (string.IsNullOrEmpty(ts))
                return false;

            return true;
        }

        public void setMatchInfo(TempMatchInfo info)
        {
            id = info.id;
            sport = info.sport;
            name = info.name;
            time = info.time;
            td = info.td;
            tm = info.tm;
            ts = info.ts;
            tt = info.tt;
        }
    }
}
