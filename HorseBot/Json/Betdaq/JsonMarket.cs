using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json.Betdaq
{
    public class JsonMarket
    {
        public string Name { get; set; }
        public long MarketId { get; set; }
        public int MarketType { get; set; }
        public int ParentId { get; set; }
        public int MarketStatus { get; set; }
        public int NoOfWinningSelections { get; set; }
        public string StartTime { get; set; }
        public int WithdrawalSegNo { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsEnabledForMultiples { get; set; }
        public bool IsInRunningAllowed { get; set; }
        public bool ManagedWhenInRunning { get; set; }
        public bool IsCurrentlyInRunning { get; set; }
        public int InRunningDelaySeconds { get; set; }
        public List<JsonSelection> Selections { get; set; }
    }
}
