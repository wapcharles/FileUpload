using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace FileUpload.Controllers
{
    public class FileUploadController : ApiController
    {
        public FileUploadController()
        {

        }

        [Action1DebugActionWebApiFilter]
        [TransactionLog("FileUploadController")]
        [Route("api/File/{type}")]
        public HttpResponseMessage Post(string type)
        {
            var httpRequest = HttpContext.Current.Request;

            var uploadPath = ConfigurationManager.AppSettings["UploadPath"];

            Directory.CreateDirectory(uploadPath);

            foreach (string fileKeyName in httpRequest.Files)
            {
                HttpPostedFile file = httpRequest.Files[fileKeyName];

                string filename = file.FileName;
                string extension = file.ContentType;

                var curFolderPath = uploadPath;
                while (filename.Contains("/"))
                {
                    var indexOf = filename.IndexOf('/');
                    curFolderPath += "\\" + filename.Substring(0, indexOf);
                    Directory.CreateDirectory(curFolderPath);
                    filename = filename.Substring(indexOf + 1);
                }

                if (!string.IsNullOrEmpty(filename))
                {
                    extension = Path.GetExtension(filename);

                    //var renamedFile = type;

                    file.SaveAs(curFolderPath + $@"\{filename}");
                }
            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/File")]
        public HttpResponseMessage Get(string filename)
        {
            try
            {
                var response = new HttpResponseMessage();

                var uploadPath = ConfigurationManager.AppSettings["UploadPath"];

                byte[] filedata = File.ReadAllBytes(uploadPath + "\\" + filename);

                response.Content = new ByteArrayContent(filedata);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = filename
                };

                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                return response;
            }
            catch (Exception e)
            {

            }

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
