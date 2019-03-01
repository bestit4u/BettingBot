using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonLBSelection
    {
        public string id { get; set; }
        public JsonName nameTranslations { get; set; }
        public List<JsonLBPrice> prices { get; set; }
        public object handicap { get; set; }
    }
}
