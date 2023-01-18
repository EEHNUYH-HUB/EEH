
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using EEH;
using EEH.WEB.File.Entity;
using EEH.WEB.Parameters;

namespace EEH.WEB.File
{
    public class FileHandler : IFileHandler
    {

        public FileHandler(string path)
        {
            RootFolder = path.ExCreateDirectory();
        }
        public string RootFolder { get; set; }
        public const long BUFFERSIZE =  2048000 ;
        public long BufferSize { get { return BUFFERSIZE; } }

       

        public DownloadResult Read(string strID, string number)
        {
            string folderPath = RootFolder.ExCombine(strID);
            if (folderPath.ExIsExists())
            {
                List<DirectoryInfo> dirs = folderPath.ExDirectoryList();

                if (dirs.ExNotNull())
                {
                    foreach (var dir in dirs)
                    {
                        if (dir.Name.ExToLower() == number)
                        {
                            List<FileInfo> files = dir.FullName.ExFileList();
                            if (files.ExNotNull() && files.Count > 0)
                            {
                                DownloadResult result = new DownloadResult();
                                result.DownloadStream = new FileStream(files[0].FullName, FileMode.Open, FileAccess.Read);
                                result.FileName = files[0].Name;
                                return result;
                            }
                        }
                    }
                }
            }

            return null;
        }

        
        public UploadResult Write(FileParameter param)
        {
            try
            {
                if (param.ExNotNull())
                {
                    UploadResult result = new UploadResult();

                    result.ResultType = UploadResultType.PROGRESS;
                    if (param.ExNotNull())
                    {
                        string name = param.FileName.ExToLower();
                        long totalSize = param.FileSize.ExLong();
                        byte[] buffer = param.Base64String.ExBase64Byte();
                        if (buffer.ExNotNull())
                        {
                            string folder = RootFolder.ExCreateDirectory();
                            using (FileStream fs = new FileStream(folder.ExCombine(name), FileMode.Append, FileAccess.Write))
                            {
                                long currentSize = fs.Length + buffer.Length;
                                fs.Write(buffer, 0, buffer.Length);

                                if (currentSize < totalSize)
                                {
                                    result.ResultType = UploadResultType.PROGRESS;
                                }
                                else
                                {
                                    result.ResultType = UploadResultType.COMPLETED;
                                }
                            }
                        }

                    }

                    return result;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                UploadResult result = new UploadResult();
                result.ResultType = UploadResultType.ERROR;
                result.ErrorMsg = ex.Message;
                return result;
            }
        }





    }
}
