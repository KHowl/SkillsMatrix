using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class SkillLevelsModel
    {
        private SkillsMatrixDB database;
        public int SkillLevelID { get; set; }
        public string LevelName { get; set; }
        public int SkillLevel { get; set; }
        public int SkillLevelGroupID { get; set; }
    }
}
