using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonEventGroup
    {
        public string eventGroupId { get; set; }
        public int sortOrder { get; set; }
        public List<JsonEventGroupList> list { get; set; }
        public int displayLimit { get; set; }
        public bool expanded { get; set; }
        public string locale { get; set; }
        public int versionTag { get; set; }
        public bool valid { get; set; }
        public string lastModified { get; set; }
        public string id { get; set; }
    }
}
