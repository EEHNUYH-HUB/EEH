using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EEH.Utils
{
    public class CommonUtils
    {
        public static string APPLICATIONDATAFOLDER { get; }
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

        public static bool IsExists(string path)
        {
            if (System.IO.File.Exists(path))
            {
                return true;
            }
            else
            {
                return System.IO.Directory.Exists(path);
            }
        }

        public static bool IsFile(string path)
        {

            FileInfo info = new FileInfo(path);

            if (info.Exists)
            {
                return true;
            }
            DirectoryInfo dinfo = new DirectoryInfo(path);
            if (dinfo.Exists)
                return false;

            string ex = path.ExFileExtension();
            return ex.ExNotNullOrEmpty();
        }

        public static bool IsDirectory(string path)
        {

            if (System.IO.Directory.Exists(path))
            {
                return true;
            }


            return false;
        }



        public static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                return true;    //  잠겨있다. (쓰고있음)
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            return false;   // 잠겨있지않다. (아무도 안쓰고있음)
        }
        public static bool RunWipe(string filePath)
        {
            bool returnValue = false;

            bool isMove = false;
            string outputFileName = string.Empty;
            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    // 확장자도 감춰줍니다. (원본이 어떤 파일이었는지 추론할 수 없도록..)
                    outputFileName = Path.Combine(filePath.ExDirectoryName(), System.Guid.NewGuid().ToString());// +Path.GetExtension(filePath);

                    System.IO.File.Move(filePath, outputFileName);
                    isMove = true;

                    // 제로필입니다.
                    using (FileStream stream = new FileStream(outputFileName, FileMode.Truncate, FileAccess.Write, FileShare.Read))
                    {
                        for (int i = 0; i < stream.Length; i++)
                            stream.WriteByte(0x00);

                        stream.Close();
                    }


                    System.IO.File.Delete(outputFileName);
                    returnValue = true;


                }
            }

            catch
            {

                returnValue = FileDelete(isMove, outputFileName, filePath);

            }
            return returnValue;
        }
        private static bool FileDelete(bool isMove, string outputFileName, string filePath)
        {
            try
            {
                if (isMove)
                {

                    System.IO.File.Delete(outputFileName);

                }
                else
                {
                    System.IO.File.Delete(filePath);

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string CreateDirectory(string directoryFullPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(directoryFullPath);


                if (!dir.Exists)
                {
                    if (dir.Parent.ExNotNull())
                    {
                        CreateDirectory(dir.Parent.FullName);
                        Directory.CreateDirectory(directoryFullPath);
                    }

                }

                return directoryFullPath;
            }
            catch
            {
                return directoryFullPath;
            }
        }

        public delegate bool UploadFileSpliteHandler(long totalSize, long readSize, byte[] readByte);
        public static void FileSpliter(string path, int bufferSize, UploadFileSpliteHandler progressHandler)
        {


            int totalReadSize = 0;
            int readSize;

            using (Stream stream = new System.IO.FileStream(path, FileMode.Open, FileAccess.Read))
            {
                long totalSize = stream.Length;

                if (totalSize < bufferSize)
                {
                    bufferSize = (int)totalSize;
                }
                byte[] buffer = new byte[bufferSize];

                if (bufferSize > 0)
                {
                    while ((readSize = stream.Read(buffer, 0, bufferSize)) > 0)
                    {
                        totalReadSize += readSize;

                        if (progressHandler(totalSize, totalReadSize, buffer))
                        {
                            if (totalReadSize >= totalSize)
                                bufferSize = 0;
                            else if (totalSize - totalReadSize < bufferSize)
                            {
                                bufferSize = (int)(totalSize - totalReadSize);

                            }
                            buffer = new byte[bufferSize];
                        }
                        else
                        {
                            break;
                        }

                    }
                }
            }
        }

    }
}
