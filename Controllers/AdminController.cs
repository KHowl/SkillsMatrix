using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SkillsMatrix.Models;

namespace SkillsMatrix.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Admin()
        {
            SkillsMatrixDB database = HttpContext.RequestServices.GetService(typeof(SkillsMatrix.Models.SkillsMatrixDB)) as SkillsMatrixDB;
            ViewModel viewModel = new ViewModel();
            viewModel.People = database.GetAllPeople();

            return View(viewModel);
        }

        // TODO Implement working functionality to upload file to db.
        // public IActionResult UploadFile()
        // {

        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}