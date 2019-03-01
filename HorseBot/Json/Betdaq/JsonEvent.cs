using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json.Betdaq
{
    public class JsonEvent
    {
        public long EventId { get; set; }
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsEnabledForMultiples { get; set; }
        public List<JsonMarket> Markets { get; set; }
        public List<JsonEvent> Events { get; set; }
    }
}
