using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class SkillsModel
    {
        private SkillsMatrixDB database;
        public int SkillID { get; set; }
        public int PersonID { get; set; }

        public int SkillGroupID { get; set; }
        public string SkillName { get; set; }
        public string GroupName { get; set; }
        public string GroupParentName { get; set; }
        public string RequiredSkillLevel { get; set; }
        public int RequireSkillLevelValue { get; set; }
        public string HeldSkillLevel { get; set; }
        public int HeldSkillLevelValue { get; set; }


    }


}
