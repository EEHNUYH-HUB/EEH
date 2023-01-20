

using EEH.WEB.Auth;
using EEH.WEB.File;
using EEH;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using EEH.WEB.File.Entity;
using EEH.WEB.Parameters;

namespace EEH.WEB.Controllers
{
    public class FileController : BaseController
    {
        IFileHandler fileHandler;
        public FileController(IAuthenticator ctx, IFileHandler fileHandler) : base(ctx)
        {
            this.fileHandler = fileHandler;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task Download(string staticid)
        {
            if (fileHandler.ExNotNull())
            {
                await Download(this.fileHandler.Read(staticid));
            }
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task Image(string staticid)
        {
            try
            {

                using (var downloadResult = this.fileHandler.Read(staticid))
                {
                    if (downloadResult.ExNotNull())
                    {
                        HttpContext.Response.ContentType = "image/png";
                        byte[] buffer = new byte[downloadResult.DownloadStream.Length];

                        downloadResult.DownloadStream.Read(buffer, 0, buffer.Length);


                        await HttpContext.Response.Body.WriteAsync(buffer);
                    }
                }
                    

            }
            catch (Exception ex)
            {
                await HttpContext.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
            }

        }

        private async Task Download(DownloadResult result)
        {
            if (result.ExNotNull())
            {
                using (result)
                {
                    HttpContext.Response.ContentType = "application/octet-stream";
                    HttpContext.Response.ContentLength = result.DownloadStream.Length;
                    
                    HttpContext.Response.Headers.Add("Content-Disposition", "attachment; Filename=" + System.Net.WebUtility.UrlEncode(result.FileName));


                    byte[] buffer = new byte[fileHandler.BufferSize];
                    int bytesRead = result.DownloadStream.Read(buffer, 0, buffer.Length);


                    while (bytesRead > 0)
                    {
                        byte[] buffer2 = new byte[bytesRead];
                        System.Buffer.BlockCopy(buffer, 0, buffer2, 0, bytesRead);

                        await HttpContext.Response.Body.WriteAsync(buffer2);
                        await HttpContext.Response.Body.FlushAsync();

                        bytesRead = result.DownloadStream.Read(buffer, 0, 8192);
                    }

                    HttpContext.Response.Body.Close();
                    await HttpContext.Response.Body.FlushAsync();// context.Response.End();
                }
            }
        }

        [HttpPost]
        public async Task <UploadResult> Upload(FileParameter param)
        {            
            return this.fileHandler.Write(param);
        }

    }
}
