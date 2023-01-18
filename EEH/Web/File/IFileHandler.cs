using EEH.WEB.File.Entity;
using EEH.WEB.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace EEH.WEB.File
{
    public interface IFileHandler
    {
        long BufferSize { get; }
        DownloadResult Read(string id, string num);
        //DownloadResult Get(string param);

        UploadResult Write(FileParameter param);
    }
}
