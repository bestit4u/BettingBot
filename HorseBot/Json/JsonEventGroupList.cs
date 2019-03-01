using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Betburger.Json
{
    public class JsonEventGroupList
    {
        public string id { get; set; }

        [JsonProperty(PropertyName = "event")]
        public JsonGroupEvent eve { get; set; }
        public string locale { get; set; }
        public int index { get; set; }
    }
}
