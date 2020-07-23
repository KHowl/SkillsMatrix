using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class SkillGroupModel
    {
        private SkillsMatrixDB database;
        public int SkillGroupID { get; set; }
        public string GroupName { get; set; }
        public int SkillGroupParentID { get; set; }
    }
}
