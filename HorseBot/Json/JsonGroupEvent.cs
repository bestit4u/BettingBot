using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonGroupEvent
    {
        public string id { get; set; }
        public JsonName nameTranslations { get; set; }
        public JsonSportsBookType sportsBookType { get; set; }
        public JsonSportsBookClass sportsBookClass { get; set; }
    }
}
