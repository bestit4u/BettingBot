using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Betburger.Json.Matchbook
{
    public class JsonPrice
    {
        [JsonProperty(PropertyName = "available-amount")]
        public double maxStake { get; set; }
        public double odds { get; set; }
        public string side { get; set; }
    }
}
