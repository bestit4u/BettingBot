using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonLBMarket
    {
        public string id { get; set; }
        public JsonName nameTranslations { get; set; }
        public List<JsonLBSelection> selections { get; set; }
    }
}
