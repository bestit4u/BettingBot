using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Betburger.Json
{
    public class JsonLiveEvent
    {
        [JsonProperty(PropertyName = "event")]
        public JsonEvent eve { get; set; } 
    }
}
