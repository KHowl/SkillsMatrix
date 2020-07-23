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
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;

        public ManagementController(ILogger<ManagementController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Management()
        {
            return View();
        }

        // TODO Assign the delegate and save in the database.
        // public IActionResult AssignDelegate()
        // {

        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}