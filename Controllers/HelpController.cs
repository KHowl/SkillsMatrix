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
    public class HelpController : Controller
    {
        private readonly ILogger<HelpController> _logger;

        public HelpController(ILogger<HelpController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Help()
        {
            return View();
        }

        // TODO Set up the method to save the incoming form data.
        // public IActionResult SaveContact(string name, string email, string comments)
        // {
        //     return View();
        // }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}