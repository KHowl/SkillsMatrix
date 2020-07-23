using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class SkillGroupParentModel
    {
        private SkillsMatrixDB database;
        public int SkillGroupParentID { get; set; }
        public string GroupName { get; set; }
    }
}