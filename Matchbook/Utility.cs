using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matchbook
{
    public enum Status
    {
        open,
        cancelled,
        edited,
        matched,
        flushed,
        failed
    }

    public class Utility
    {
        #region Math Function

        public static int ParseToInt(string str)
        {
            int val = 0;
            int.TryParse(str, out val);
            return val;
        }

        public static double ParseToDouble(string str)
        {
            double val = 0;
            double.TryParse(str, out val);
            return val;
        }

        public static long ParseToLong(string str)
        {
            long val = 0;
            long.TryParse(str, out val);
            return val;
        }

        public static DateTime ParseToDateTime(string str)
        {
            DateTime val = DateTime.Now;
            DateTime.TryParse(str, out val);
            return val;
        }

        #endregion
    }
}
