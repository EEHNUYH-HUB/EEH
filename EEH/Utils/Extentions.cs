using System;
using System.Collections.Generic;
using System.Text;

namespace EEH
{
    public static class Extentions
    {
        public static long ExLong(this string str)
        {
            long rtn = 0;
            long.TryParse(str, out rtn);
            return rtn;
        }
        public static int ExInt(this string str)
        {
            int rtn = 0;
            int.TryParse(str, out rtn);
            return rtn;
        }
        public static double ExDouble(this string str)
        {
            double rtn = 0;
            double.TryParse(str, out rtn);
            return rtn;
        }

        public static bool ExNotNull(this object obj)
        {
            return obj != null;
        }

        
    }
}
