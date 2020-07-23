using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class RoleModel
    {
        private SkillsMatrixDB database;
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
