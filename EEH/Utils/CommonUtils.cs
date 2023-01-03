using System;
using System.Collections.Generic;
using System.Text;

namespace EEH.Utils
{
    public class CommonUtils
    {
        public const string ROOTFOLDER = "EEH";
        public const string DATAFOLDER = "DATA";
        public static long GetTimeStamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }
    }
}
