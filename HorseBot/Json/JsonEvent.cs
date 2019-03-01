using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonEvent
    {
        public long id { get; set; }
        public string name { get; set; }
        public string group { get; set; }
        public long groupId { get; set; }
        public string sport { get; set; }
    }
}
