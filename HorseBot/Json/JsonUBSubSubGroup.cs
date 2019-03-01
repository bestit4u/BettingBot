using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonUBSubSubGroup
    {
        public string englishName { get; set; }
        public List<JsonUBSubSubSubGroup> groups { get; set; }
    }
}
