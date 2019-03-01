using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Betburger.Json.Matchbook
{
    public class JsonEvent
    {
        public long id { get; set; }
        public string name { get; set; }

        [JsonProperty(PropertyName = "in-running-flag")]
        public bool isLive { get; set; }
        public List<JsonMarket> markets { get; set; }
    }
}
