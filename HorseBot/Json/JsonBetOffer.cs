using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json
{
    public class JsonBetOffer
    {
        public long id { get; set; }
        public JsonCriterion criterion { get; set; }
        public JsonBetOfferType betOfferType { get; set; }
        public List<JsonOutcome> outcomes { get; set; }
    }
}
