using EEH.Cryptography;
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


        public static string ExCreateDirectory(this string directoryFullPath)
        {
            return FileUtil.CreateDirectory(directoryFullPath);
        }


        public static bool ExBool(this object obj)
        {
            bool isSuccess = false;

            return obj.ExBool(out isSuccess);
        }
        public static bool ExBool(this object obj, out bool isSuccess)
        {
            bool returnValue = false;

            isSuccess = bool.TryParse(obj.ExToString(), out returnValue);

            if (!isSuccess && obj.ExToString().ExNotNullOrEmpty())
                returnValue = Convert.ToBoolean(obj.ExToString().ExInt());

            return returnValue;
        }
        public static int ExInt(this object obj)
        {
            bool isSuccess = false;
            return obj.ExInt(out isSuccess);
        }

        public static float ExFloat(this object obj)
        {
            bool isSuccess = false;
            return obj.ExFloat(out isSuccess);
        }

        public static double ExDouble(this object obj)
        {
            bool isSuccess = false;
            return obj.ExDouble(out isSuccess);
        }
        public static long ExLong(this object obj)
        {
            bool isSuccess = false;
            return obj.ExLong(out isSuccess);
        }

        public static T ExEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }

        public static T ExEnum<T>(this string str, out bool isConvert)
        {
            try
            {
                isConvert = true;
                return (T)Enum.Parse(typeof(T), str);
            }
            catch
            {
                isConvert = false;
                return default(T);
            }
        }

        public static int ExInt(this object obj, out bool isSuccess)
        {
            int returnValue = 0; // 

            if (obj == null)
            {
                isSuccess = false;
                return 0;
            }

            if (obj != null && obj is bool)
            {
                isSuccess = true;

                bool value = (bool)obj;
                if (value) return 1;
                else return 0;
            }

            isSuccess = int.TryParse(obj.ExToString(), out returnValue);

            return returnValue;
        }

        public static float ExFloat(this object obj, out bool isSuccess)
        {
            float returnValue = -1f;
            isSuccess = float.TryParse(obj.ExToString(), out returnValue);
            return returnValue;
        }

        public static double ExDouble(this object obj, out bool isSuccess)
        {
            double returnValue = -1f;
            isSuccess = double.TryParse(obj.ExToString(), out returnValue);
            return returnValue;
        }

        public static long ExLong(this object obj, out bool isSuccess)
        {
            long returnValue = 0;
            isSuccess = long.TryParse(obj.ExToString(), out returnValue);
            return returnValue;
        }


        public static string ExCombine(this string path1, string path2)
        {
            return System.IO.Path.Combine(path1, path2);
        }
        public static bool ExIsNull<T>(this T obj)
        {

            if (obj == null)
                return true;
            else
                return false;

        }

        public static bool ExNotNull<T>(this T obj)
        {

            if (obj == null)
                return false;
            else
                return true;


        }
        public static bool ExNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str.ExToString().Trim());
        }

        public static bool ExIsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static string ExToString(this string obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString();
        }
        public static string ExToString(this object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString();
        }
        public static string ExToLower(this object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString().ToLower();
        }
        public static string ExToUpper(this object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString().ToUpper();
        }

        public static string ExToString(this DateTime dt, string format)
        {

            return dt.ToString(format);
        }

        public static string ExBase64String(this byte[] buffer)
        {
            if (buffer.ExNotNull())
                return Convert.ToBase64String(buffer);

            return string.Empty;
        }

        public static byte[] ExBase64Byte(this string str)
        {
            if (str.ExNotNullOrEmpty())
                return Convert.FromBase64String(str);

            return null;
        }

        public static byte[] ExASCIIByte(this string str)
        {
            if (str.ExNotNullOrEmpty())
                return Encoding.ASCII.GetBytes(str);

            return null;
        }
        public static Stream ExASCIIStream(this string str)
        {
            if (str.ExNotNullOrEmpty())
                return new MemoryStream(str.ExASCIIByte());

            return null;
        }
        public static string ExASCIIString(this byte[] buffer)
        {
            if (buffer != null)
                return Encoding.ASCII.GetString(buffer);
            else
            {
                return string.Empty;
            }
        }

        public static string ExASCIIString(this Stream strm)
        {
            if (strm != null)
            {

                byte[] buffer = new byte[strm.Length];
                strm.Read(buffer, 0, buffer.Length);
                return buffer.ExASCIIString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string ExDirectoryName(this string fileName)
        {
            if (fileName.ExIsFile())
            {
                try
                {
                    return System.IO.Path.GetDirectoryName(fileName);
                }
                catch (PathTooLongException)
                {
                    return fileName.Substring(0, fileName.LastIndexOf("\\"));// System.IO.Path.GetDirectoryName(fileName);
                }
            }
            else
                return fileName;
        }

        public static string ExFileExtension(this string fileName)
        {
            return System.IO.Path.GetExtension(fileName);
        }

        public static string ExFileNameWithoutExtension(this string fileName)
        {
            string str = System.IO.Path.GetFileName(fileName);
            return System.IO.Path.GetFileNameWithoutExtension(str);
        }

        public static string ExFileName(this string fileName)
        {
            return System.IO.Path.GetFileName(fileName);
        }

        public static bool ExIsExists(this string path)
        {
            return FileUtil.IsExists(path);
        }


        public static bool ExFileDelete(this string filePath)
        {
            try
            {

                if (filePath.ExNotNullOrEmpty())
                {
                    FileInfo file = new FileInfo(filePath);


                    if (file.Exists)
                    {
                        if (file.IsReadOnly)
                            file.IsReadOnly = false;

                        using (FileStream fs = new FileStream(filePath, FileMode.Open))
                        {

                        }

                        int loopCount = 0;
                        while (FileUtil.IsFileLocked(file)) // 만약 파일이 잠겨있다면 루프를 도는데
                        {
                            if (loopCount >= 13) // 3초까지만 돕니다.
                            {
                                break; // 그냥 탈출
                            }
                            System.Threading.Thread.Sleep(1000);
                            loopCount++;

                        }

                        return FileUtil.RunWipe(filePath);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }


        public static List<FileInfo> ExFileList(this string path)
        {
            if (path.ExIsExists())
            {
                if (path.ExIsDirectory())
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] fileArray = dir.GetFiles();
                    if (fileArray.ExNotNull() && fileArray.Length > 0)
                    {
                        return fileArray.ToList();
                    }
                }
            }

            return null;
        }
        public static List<DirectoryInfo> ExDirectoryList(this string path)
        {
            if (path.ExIsExists())
            {
                if (path.ExIsDirectory())
                {
                    DirectoryInfo dir = new DirectoryInfo(path);
                    DirectoryInfo[] dirArray = dir.GetDirectories();
                    if (dirArray.ExNotNull() && dirArray.Length > 0)
                    {
                        return dirArray.ToList();
                    }
                }
            }

            return null;
        }
        public static bool ExIsFile(this string path)
        {
            return FileUtil.IsFile(path);
        }
        public static bool ExIsDirectory(this string path)
        {
            return FileUtil.IsDirectory(path);
        }
        public static string ExEncryptString(this string str)
        {
            if (str.ExNotNullOrEmpty())
            {
                ICryptography cry = EEH.Cryptography.CrypEnvironment.Get(EEH.Cryptography.CrypType.AES);
                return cry.EncryptString(str);
            }
            return str;
        }

        public static string ExDecryptString(this string str)
        {
            if (str.ExNotNullOrEmpty())
            {
                ICryptography cry = EEH.Cryptography.CrypEnvironment.Get(EEH.Cryptography.CrypType.AES);
                return cry.DecryptString(str);
            }
            return str;
        }


        public static void ExEncryptFile(this string path)
        {
            if (path.ExNotNullOrEmpty() && path.ExIsFile())
            {
                ICryptography cry = EEH.Cryptography.CrypEnvironment.Get(EEH.Cryptography.CrypType.AES);
                cry.EncryptFile(path);
            }
        }

        public static void ExDecryptFile(this string path)
        {
            if (path.ExNotNullOrEmpty() && path.ExIsFile())
            {
                ICryptography cry = EEH.Cryptography.CrypEnvironment.Get(EEH.Cryptography.CrypType.AES);

                cry.DecryptFile(path);
            }

        }


        public static T ExJsonDeserialize<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public static string ExJsonSerialize(this object obj)
        {
            if (obj.ExNotNull())
                return JsonConvert.SerializeObject(obj);

            return null;
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
