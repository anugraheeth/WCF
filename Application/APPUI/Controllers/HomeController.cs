using ALTSCANUI.AtlscanRef;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;

namespace ALTSCANUI.Controllers
{
    public class HomeController : Controller
    {
        private ZipServiceClient client = new ZipServiceClient();

        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Batch()
        {
            ViewBag.Result = client.ProcessZipBatch();
            return View();
        }

        [HttpGet]
        public ActionResult Manual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Manual(string zipName)
        {
            try
            {
                ViewBag.Result = client.ProcessZipManual(zipName);
            }
            catch (FaultException ex)
            {
                ViewBag.Result = ex.Message;
            }
            return View();
        }

        [HttpGet]
        public ActionResult Search(string fileName)
        {
            var filesArray = client.SearchFile(fileName);
            var files = filesArray?.ToList() ?? new List<string>();
            return View(files);
        }

        [HttpGet]
        public ActionResult Download(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return HttpNotFound();

            try
            {
                var data = client.DownloadFile(path);
                var fileName = Path.GetFileName(path);
                return File(data, "application/octet-stream", fileName);
            }
            catch (FaultException)
            {
                return HttpNotFound("File not found");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (client != null)
                {
                    try { client.Close(); }
                    catch { client.Abort(); }
                }
            }
            base.Dispose(disposing);
        }
    }
}
