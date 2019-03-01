using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonExchangeResult
    {
        public bool moreAvailable { get; set; }
        public int nextPageIndex { get; set; }
        public int tableSize { get; set; }
        public object errorMessage { get; set; }
        public int rangeStart { get; set; }
        public int pageSize { get; set; }
        public JsonExchangeResponseFilter responseFilters { get; set; }
        public List<JsonExchangeBet> bets { get; set; }
    }
}
