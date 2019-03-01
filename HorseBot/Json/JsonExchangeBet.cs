using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonExchangeBet
    {
        public object index { get; set; }
        public string betId { get; set; }
        public object regulatorBetId { get; set; }
        public string placedDate { get; set; }
        public string eventDescription { get; set; }
        public string marketName { get; set; }
        public string selectionName { get; set; }
        public string side { get; set; }
        public string odds { get; set; }
        public string stake { get; set; }
        public string status { get; set; }
        public string grossProfit { get; set; }
        public List<JsonPair> liability { get; set; }
        public List<JsonPair> returnAmount { get; set; }
        public bool isBSP { get; set; }
        public bool isEachWayBet { get; set; }
        public string marketLink { get; set; }
        public object editBetLink { get; set; }
        public string matchedDate { get; set; }
    }
}
