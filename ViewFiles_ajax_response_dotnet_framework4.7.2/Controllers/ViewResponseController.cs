using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.IO.Compression;
using System.Collections.Generic;
using System.Configuration;

namespace ViewFiles_ajax_response_dotnet_framework4._7._2.Controllers
{
    public class ViewResponseController : Controller
    {
        private static string folderPath = ConfigurationManager.AppSettings["FilePath"];

        #region View page
        public ActionResult List()
        {
            return View();
        }

        #endregion

        #region get files

        [HttpGet]
        public JsonResult GetFiles(string date)
        {
            DateTime selectedDate; string selectedDateFolderPath = string.Empty;
            if (DateTime.TryParse(date, out selectedDate))
            {
                selectedDateFolderPath = Path.Combine(folderPath, selectedDate.ToString("yyyyMMdd"));

                try
                {
                    Directory.Exists(selectedDateFolderPath);

                    string[] files = Directory.GetFiles(selectedDateFolderPath);

                    List<string> fileNames = files.Select(Path.GetFileName).ToList();

                    return Json(fileNames, JsonRequestBehavior.AllowGet);

                }
                catch (Exception errorMessage)
                {
                    var userMsg = new
                    {
                        error = errorMessage
                    };
                    return Json("Error:" + userMsg.error.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var userMsg = new
                {
                    error = "Invalid date format. Please select a valid date in the correct format." 
                };
                return Json("Error:" + userMsg.error, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region download files

        [HttpGet]
        public ActionResult Download(string filenames)
        {
            if (!string.IsNullOrEmpty(filenames))
            {
                string[] fileNamesArray = filenames.Split(',');
                List<FileContentResult> fileResponses = new List<FileContentResult>();

                foreach (string fileName in fileNamesArray)
                {
                    string trimmedFileName = fileName.Trim();
                    string filePath = Path.Combine(folderPath, trimmedFileName);

                    if (System.IO.File.Exists(filePath))
                    {
                        var fileBytes = System.IO.File.ReadAllBytes(filePath);
                        var fileResponse = File(fileBytes, "application/octet-stream", trimmedFileName);
                        fileResponses.Add(fileResponse);
                    }
                }

                if (fileResponses.Count == 1)
                {
                    // If there's only one file, return it directly
                    return fileResponses[0];
                }
                else if (fileResponses.Count > 1)
                {
                    // If there are multiple files, create a ZIP archive
                    var dateTime = DateTime.Now;
                    var zipFileName = dateTime.ToString("yyyyMMddHHmmss") + ".zip";

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                        {
                            foreach (var fileResponse in fileResponses)
                            {
                                var entry = archive.CreateEntry(fileResponse.FileDownloadName);
                                using (var entryStream = entry.Open())
                                {
                                    entryStream.Write(fileResponse.FileContents, 0, fileResponse.FileContents.Length);
                                }
                            }
                        }

                        memoryStream.Seek(0, SeekOrigin.Begin);
                        var result = new FileContentResult(memoryStream.ToArray(), "application/zip")
                        {
                            FileDownloadName = zipFileName
                        };

                        return result;
                    }
                }
            }

            return null;
        }

        #endregion

    }
}