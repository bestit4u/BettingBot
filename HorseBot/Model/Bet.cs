using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Model
{
    public class Bet
    {
        public DateTime datetime { get; set; }
        public string time { get; set; }
        public string betId { get; set; }
        public string league { get; set; }
        public string match { get; set; }
        public string type { get; set; }
        public string odds { get; set; }
        public string stake { get; set; }
        public string status { get; set; }
        public double grossProfit { get; set; }

        public Bet()
        {
            datetime = DateTime.Now;
            time = string.Empty;
            betId = string.Empty;
            league = string.Empty;
            match = string.Empty;
            type = string.Empty;
            odds = string.Empty;
            stake = string.Empty;
            status = string.Empty;
            grossProfit = 0;
        }
    }
}
