using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SkillsMatrix.Models;

namespace SkillsMatrix.Controllers
{
    public class RecordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<RecordsModel> GenerateRecords()
        {
            List<RecordsModel> records = new List<RecordsModel>();

            return records;
        }
    }
}
