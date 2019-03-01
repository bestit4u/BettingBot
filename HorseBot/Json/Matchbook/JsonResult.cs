using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json.Matchbook
{
    public class JsonResult
    {
        public int offset { get; set; }
        public int total { get; set; }
        public List<JsonEvent> events { get; set; }
    }
}
