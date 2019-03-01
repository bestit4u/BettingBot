using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Betburger.Constant;

namespace Betburger.Model
{
    public class PriceInfo
    {
        public long selectionId { get; set; }
        public double odds { get; set; }
        public double stake { get; set; }
        public SIDE side { get; set; }
    }
}
