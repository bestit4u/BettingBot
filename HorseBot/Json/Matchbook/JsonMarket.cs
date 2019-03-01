using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json.Matchbook
{
    public class JsonMarket
    {
        public long id { get; set; }
        public string name { get; set; }
        public List<JsonRunner> runners { get; set; }
    }
}
