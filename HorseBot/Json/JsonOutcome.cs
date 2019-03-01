using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonOutcome
    {
        public long id { get; set; }
        public string label { get; set; }
        public string oddsFractional { get; set; }
        public object line { get; set; }
    }
}
