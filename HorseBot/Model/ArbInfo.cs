using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Betburger.Constant;

namespace Betburger.Model
{
    public class ArbInfo
    {
        public double percent { get; set; }
        public string bookie { get; set; }
        public string sport { get; set; }
        public string league { get; set; }
        public string name { get; set; }
        public string outcome { get; set; }
        public string odds { get; set; }

        public ArbInfo()
        {
            percent = 0;
            bookie = BOOKIE.Bet365.ToString();
            sport = "";
            league = "";
            name = "";
            outcome = "";
            odds = "";
        }
    }
}
