using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class LocationModel
    {
        private SkillsMatrixDB database;
        public int LocationID { get; set; }
        public string LocationName { get; set; }
    }
}