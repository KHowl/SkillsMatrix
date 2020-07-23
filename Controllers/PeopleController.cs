using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkillsMatrix.Models;

namespace SkillsMatrix.Controllers
{
    public class PeopleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<PersonModel> GeneratePeople()
        {
            List<PersonModel> list = new List<PersonModel>();

            return list;
        }
    }
}
