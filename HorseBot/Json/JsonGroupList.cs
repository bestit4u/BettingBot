using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonGroupList
    {
        public string id { get; set; }
        public int versionTag { get; set; }
        public List<JsonGroupItem> itemIds { get; set; }
    }
}
