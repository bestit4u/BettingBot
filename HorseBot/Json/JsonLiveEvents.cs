using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonLiveEvents
    {
        public JsonUBGroup group { get; set; }
        public List<JsonLiveEvent> liveEvents { get; set; }
    }
}
