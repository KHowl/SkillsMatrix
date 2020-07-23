using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class PersonModel
    {
        private SkillsMatrixDB database;
        public int PersonID { get; set; }
        public string Name { get; set; }
        public string ManagerName { get; set; }
        public int PersonnelNo { get; set; }
        public string RoleName { get; set; }
        public string LocationName { get; set; }

        public string Email { get; set; }
    }
}
