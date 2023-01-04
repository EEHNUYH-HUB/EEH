using EEH.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Xml.Linq;

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
        public static bool ExIsNull(this object obj)
        {
            return obj == null;
        }


        public static string ExJsonSerializeString(this object obj, IContractResolver contractResolver = null)
        {
            if (contractResolver.ExIsNull())
            {
                contractResolver = new DefaultContractResolver();
            }

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }

        public static T ExDeserializeObject<T>(this string strJson, IContractResolver contractResolver = null)
        {
            if (contractResolver.ExIsNull())
            {
                contractResolver = new DefaultContractResolver();
            }

            return JsonConvert.DeserializeObject<T>(strJson, new JsonSerializerSettings { ContractResolver = new DefaultContractResolver() });
        }
        public static string ExCombine(this string root, params string[] paths)
        {
            string rtn = root;
            if (root.ExNotNull() && paths.ExNotNull() && paths.Length > 0)
            {
                foreach (string path in paths)
                {
                   rtn = System.IO.Path.Combine(rtn, path);
                }
            }
            return rtn;
        }

        public static bool ExExists(this string path)
        {
            if (!System.IO.File.Exists(path))
            {
                if (!System.IO.Directory.Exists(path))
                {
                    return false;
                }
            }

            return true;
        }

        public static string ExCreateDirectory(this string path)
        {
            if (!path.ExExists())
            {
                System.IO.Directory.CreateDirectory(path);
            }
            return path;
        }
        public static void ExDelete(this string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            else if(System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path);
            }
        }

        public static byte[] ExStringToUTF8(this string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
        public static string ExUTF8ToString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string ExFileNameWithOutExtention(this string str)
        {
            return System.IO.Path.GetFileNameWithoutExtension(str);
        }
        public static List<string> ExListForDataFolder(this string key)
        {
            List<string> rtn = new List<string>();
            
            string folder = CommonUtils.DATAFOLDER.ExCombine( key);
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(folder);
            FileInfo[] files = dir.GetFiles("*.data");
            
            if (files.ExNotNull())
            {
              rtn =  files.OrderBy(x => x.LastWriteTime).Select(x => x.Name?.ExFileNameWithOutExtention()).ToList();
            }

            return rtn;
        }

        public static void ExSaveForDataFolder(this string str,string key,string name)
        {
            string filePath = CommonUtils.DATAFOLDER.ExCombine( key).ExCreateDirectory().ExCombine(name+".data");

            filePath.ExDelete();

            using (FileStream file = new FileStream(filePath, FileMode.Create))
            {
                byte[] buffer = str.ExStringToUTF8();
                file.Write(buffer, 0, buffer.Length);
            }

        }

        
        public static string ExLoadForDataFolder(this string key,string name)
        {
            
            string filePath = CommonUtils.DATAFOLDER.ExCombine(key).ExCreateDirectory().ExCombine(name + ".data");
            using (FileStream file = new FileStream(filePath, FileMode.Open))
            {
                byte[] buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                return buffer.ExUTF8ToString();
            }


        }

    }
}
