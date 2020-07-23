using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class ManagerModel
    {
        private SkillsMatrixDB database;
        public int PersonID { get; set;}
        public string Name { get; set;}
        public int ManagerID { get; set;}
        public int Level { get; set;}
    }
}