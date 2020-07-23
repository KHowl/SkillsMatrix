using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using static System.Net.WebRequestMethods;
using SkillsMatrix.Models;


namespace SkillsMatrix.Controllers
{
    
    public class UploadController : Controller
    {
        private string getErrorString(FileReaderModel.FileReaderResults dbResult)
        {
            string retStr = "";

            
            switch (dbResult)
            {
                case FileReaderModel.FileReaderResults.FileReaderSuccess:
                    retStr = "Success";
                    break;
                case FileReaderModel.FileReaderResults.FileReaderUnknownUser:
                    retStr = "Unknown User in file";
                    break;
                case FileReaderModel.FileReaderResults.FileReaderWrongFormat:
                    retStr = "File does not have correct columns";
                    break;
                case FileReaderModel.FileReaderResults.FileReaderErrorProcessingFile:
                    retStr = "Error processing data in the file";
                    break;
                case FileReaderModel.FileReaderResults.FileReaderUnknownError:
                    retStr = "Unknown error processing the file";
                    break;
            }
            return retStr;
        }
        [HttpGet]
        public IActionResult Upload()
        {
            ViewModel viewModel = new ViewModel();
            viewModel.Upload = new UploadModel();
            viewModel.Upload.PpmUnknownUsers = null;
            viewModel.Upload.DistUnknownUsers = null;
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Upload(UploadModel files)
        {
            ViewBag.Msg = "";
            SkillsMatrixDB database = HttpContext.RequestServices.GetService(typeof(SkillsMatrix.Models.SkillsMatrixDB)) as SkillsMatrixDB;

            int filesProcessed = 0;
            List<string> distUnknownUsers = null;
            List<string> ppmUnknownUsers = null;

            // process the manager report
            var filename = files.MgrReportFilename;
            if (filename != null && filename.Length > 0)
            {
                filesProcessed++;
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.GetFileName(filename.FileName));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await filename.CopyToAsync(stream);
                }

                FileReaderModel reader = new FileReaderModel();
                FileReaderModel.FileReaderResults dbResult = reader.AddManagerSkillsReport(database, path);
                if (dbResult == FileReaderModel.FileReaderResults.FileReaderSuccess)
                {
                    ViewBag.Msg = "Uploaded " + Path.GetFileName(filename.FileName) + " Successfuly";
                }
                else
                {
                    ViewBag.Msg = "Upload of " + Path.GetFileName(filename.FileName) + " failed " + getErrorString(dbResult);
                }
                System.IO.File.Delete(path);
            
            }

            // now process the PPM report

            filename = files.PpmFilename;
            if (filename != null && filename.Length > 0)
            {
                filesProcessed++;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.GetFileName(filename.FileName));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await filename.CopyToAsync(stream);
                }

                FileReaderModel reader = new FileReaderModel();
                if (filesProcessed > 1)
                {
                    ViewBag.Msg += "<br/>";
                }

                

                FileReaderModel.FileReaderResults dbResult = reader.AddPpmReport(database, path, out ppmUnknownUsers);

                if (dbResult == FileReaderModel.FileReaderResults.FileReaderSuccess)
                {

                    ViewBag.Msg += "Uploaded " + Path.GetFileName(filename.FileName) + " Successfuly";
                }
                else
                {
                    ViewBag.Msg += "Upload of " + Path.GetFileName(filename.FileName) + " failed " + getErrorString(dbResult);
                }
                System.IO.File.Delete(path);
            }

            // now process the distribution report

            filename = files.DistFilename;
            if (filename != null && filename.Length > 0)
            {
                filesProcessed++;

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.GetFileName(filename.FileName));
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await filename.CopyToAsync(stream);
                }

                FileReaderModel reader = new FileReaderModel();
                if (filesProcessed > 1)
                {
                    ViewBag.Msg += "<br/>";
                }
                
                FileReaderModel.FileReaderResults dbResult = reader.AddDistReport(database, path, out distUnknownUsers);

                if (dbResult == FileReaderModel.FileReaderResults.FileReaderSuccess)
                {
                    ViewBag.Msg += "Uploaded " + Path.GetFileName(filename.FileName) + " Successfuly";
                }
                else
                {
                    ViewBag.Msg += "Upload of " + Path.GetFileName(filename.FileName) + " failed " + getErrorString(dbResult);
                }
                System.IO.File.Delete(path);
            }
            ViewModel viewModel = new ViewModel();
            viewModel.Upload = new UploadModel();

            if (ppmUnknownUsers != null)
            {
                viewModel.Upload.PpmUnknownUsers = ppmUnknownUsers;
            }
            else
            {
                viewModel.Upload.PpmUnknownUsers = new List<string>();
            }
            if (distUnknownUsers != null)
            {
                viewModel.Upload.DistUnknownUsers = distUnknownUsers;
            }
            else
            {
                viewModel.Upload.DistUnknownUsers = new List<string>();
            }
            return View(viewModel);
        }
    }
}
