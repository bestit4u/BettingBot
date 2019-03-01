using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonExchangeResponseFilter
    {
        public string dateFilter { get; set; }
        public string status { get; set; }
        public string state { get; set; }
        public object oddsType { get; set; }
        public string selectedBets { get; set; }
    }
}
