using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class AssignmentModel
    {
        public int AssignmentId;
        public int PersonID;
        public int ProjectID;

        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public string AssignmentStatus { get; set; }
        public double Utilisation { get; set; }

    }
}
