using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class SkillLevelGroupModel
    {
        private SkillsMatrixDB database;
        public int SkillLevelGroupID { get; set; }
        public string GroupName { get; set; }
    }
}