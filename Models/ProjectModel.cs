using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class ProjectModel
    {
        private SkillsMatrixDB database;
        public int ProjectID { get; set; }
        public string ProjectName { get; set; }
    }
}
