using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class RecordsModel
    {
        private SkillsMatrixDB database;
        public int RecordID { get; set; }
        public int PersonID { get; set; }
        public int SkillGroupID { get; set; }
        public int SkillID { get; set; }
        public int SkillLevelID { get; set; }
        public string Comments { get; set; }
        public DateTime DateSaved { get; set; }
        public int YearlyQuarter { get; set; }
    }
}
