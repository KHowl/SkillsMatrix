using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class ViewModel
    {
        public List<SkillGroupModel> SkillGroups { get; set; }
        public List<SkillGroupParentModel> SkillGroupParents { get; set; }
        public List<SkillsModel> Skills { get; set; }
        public List<SkillLevelsModel> SkillLevels { get; set; }
        public List<SkillLevelGroupModel> SkillLevelGroups { get; set; }
        public List<PersonModel> People { get; set; }
        public List<ManagerModel> Managers { get; set;}
        public List<RoleModel> Roles { get; set; }
        public List<ProjectModel> Projects { get; set; }
        public List<LocationModel> Locations { get; set; }
        public List<RecordsModel> Records { get; set; }
        public UploadModel Upload { get; set; }
    }
}
