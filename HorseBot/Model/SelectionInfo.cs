using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Betburger.Model
{
    public class SelectionInfo
    {
        public string marketId { get; set; }
        public long selectionId { get; set; }
        public double price { get; set; }
        public double size { get; set; }
        public double? handicap { get; set; }
    }
}
