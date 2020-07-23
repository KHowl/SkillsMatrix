using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;




namespace SkillsMatrix.Models
{
    public class UploadModel
    {
        public IFormFile MgrReportFilename { get; set; }
        public IFormFile PpmFilename { get; set; }
        public IFormFile DistFilename { get; set; }
        public List<string> PpmUnknownUsers { get; set; }
        public List<string> DistUnknownUsers { get; set; }
    }

}
