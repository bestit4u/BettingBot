using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonUBSubGroup
    {
        public string sport { get; set; }
        public List<JsonUBSubSubGroup> groups { get; set; }
    }
}
