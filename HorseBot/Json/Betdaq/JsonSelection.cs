using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Json.Betdaq
{
    public class JsonSelection
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public int ResetCount { get; set; }
        public double DeductionFactor { get; set; }
        public List<JsonPrice> PricesFor { get; set; }
        public List<JsonPrice> PricesAgainst { get; set; }
    }
}
