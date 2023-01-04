using System;
using System.Collections.Generic;
using System.Text;

namespace EEH.Utils
{
    public class CommonUtils
    {
        public  static string APPLICATIONDATAFOLDER { get; }
        public static string DATAFOLDER { get; }
        public const string ROOTFOLDER = "EEH";
        public const string DATAFOLDERNAME = "DATA";
        public static long GetTimeStamp()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }

        static CommonUtils()
        {
            APPLICATIONDATAFOLDER = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            DATAFOLDER = CommonUtils.APPLICATIONDATAFOLDER.ExCombine(CommonUtils.ROOTFOLDER, CommonUtils.DATAFOLDERNAME).ExCreateDirectory();
        }
    }
}
