using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Model
{
    public class RunnerInfo
    {
        public long id { get; set; }
        public string name { get; set; }
        public List<PriceInfo> prices { get; set; }

        public RunnerInfo()
        {
            prices = new List<PriceInfo>();
        }
    }
}
