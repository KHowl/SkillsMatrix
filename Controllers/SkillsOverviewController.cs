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
    public class SkillsOverviewController : Controller
    {
        private readonly ILogger<SkillsOverviewController> _logger;

        public SkillsOverviewController(ILogger<SkillsOverviewController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SkillsOverview()
        {
            SkillsMatrixDB database = HttpContext.RequestServices.GetService(typeof(SkillsMatrix.Models.SkillsMatrixDB)) as SkillsMatrixDB;
            ViewModel viewModel = new ViewModel();
            viewModel.Managers = database.GetAllManagers();
            viewModel.Skills = database.GetAllSkills();
            viewModel.People = database.GetAllPeople();
            viewModel.Locations = database.GetAllLocations();

            return View(viewModel);
        }

        private PersonModel FindPerson(List<PersonModel> people, string PersonName)
        {
            PersonModel recordingPerson = null;

            for (int i = 0; i < people.Count(); i++)
            {
                if (people[i].Name.Contains(PersonName))
                {
                    recordingPerson = people[i];
                }
            }
            return recordingPerson;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}