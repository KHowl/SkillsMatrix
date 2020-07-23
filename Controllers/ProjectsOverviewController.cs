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
    public class ProjectsOverviewController : Controller
    {
        private readonly ILogger<ProjectsOverviewController> _logger;
        public ProjectsOverviewController(ILogger<ProjectsOverviewController> logger)
        {
            _logger = logger;
        }
        public IActionResult ProjectsOverview()
        {
            SkillsMatrixDB database = HttpContext.RequestServices.GetService(typeof(SkillsMatrix.Models.SkillsMatrixDB)) as SkillsMatrixDB;
            ViewModel viewModel = new ViewModel();
            viewModel.Locations = database.GetAllLocations();
            viewModel.SkillGroups = database.GetAllSkillGroups();
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
