using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonLBMarketResponse
    {
        public string id { get; set; }
        public List<JsonLBMarket> markets { get; set; }
    } 
}
