using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileUpload.Controllers
{
    /// <summary>
    /// https://code.msdn.microsoft.com/Uploading-and-Downloading-7b5277ef
    /// </summary>
    public class MvcFileUploadController : Controller
    {
        /// <summary>
        /// This one uploads he files
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            foreach (string upload in Request.Files)
            {
                if (Request.Files[upload].FileName != "")
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + "/App_Data/uploads/";
                    string filename = Path.GetFileName(Request.Files[upload].FileName);
                    Request.Files[upload].SaveAs(Path.Combine(path, filename));
                }
            }
            return View("Upload");
        }
        public ActionResult Upload()
        {
            return View();
        }
        public ActionResult Downloads()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/App_Data/uploads/"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*"); List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }
            return View(items);
        }

        public FileResult Download(string ImageName)
        {
            var FileVirtualPath = "~/App_Data/uploads/" + ImageName;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }

    }
}