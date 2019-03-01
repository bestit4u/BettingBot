using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json.Matchbook
{
    public class JsonRunner
    {
        public long id { get; set; }
        public string name { get; set; }
        public List<JsonPrice> prices { get; set; }
    }
}
