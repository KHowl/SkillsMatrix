using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkillsMatrix.Models
{
    public class SkillsMatrixDB
    {
        public string ConnectionString { get; set; }

        public SkillsMatrixDB(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<SkillsModel> GetAllSkills()
        {
            List<SkillsModel> list = new List<SkillsModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from skill", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SkillsModel()
                        {
                            SkillID = Convert.ToInt32(reader["SkillID"]),
                            SkillGroupID = Convert.ToInt32(reader["SkillGroupID"]),
                            SkillName = reader["SkillName"].ToString(),
                        });
                    }
                }
            }
            list = list.OrderBy(SkillsModel => SkillsModel.SkillName).ToList();
            return list;
        }

        public List<SkillGroupModel> GetAllSkillGroups()
        {
            List<SkillGroupModel> list = new List<SkillGroupModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from skillgroup", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SkillGroupModel()
                        {
                            SkillGroupID = Convert.ToInt32(reader["SkillGroupID"]),
                            GroupName = reader["GroupName"].ToString(),
                            SkillGroupParentID = Convert.ToInt32(reader["SkillGroupParentID"])
                        });
                    }
                }
            }
            list = list.OrderBy(SkillGroupModel => SkillGroupModel.GroupName).ToList();
            return list;
        }

        public List<SkillGroupParentModel> GetAllSkillGroupParents()
        {
            List<SkillGroupParentModel> list = new List<SkillGroupParentModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from skillgroup_parent", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SkillGroupParentModel()
                        {
                            SkillGroupParentID = Convert.ToInt32(reader["SkillGroupParentID"]),
                            GroupName = reader["ParentGroupName"].ToString()
                        });
                    }
                }
            }
            list = list.OrderBy(SkillGroupParentModel => SkillGroupParentModel.GroupName).ToList();
            return list;
        }

        public List<SkillLevelsModel> GetAllSkillLevels()
        {
            List<SkillLevelsModel> list = new List<SkillLevelsModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from skill_level", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SkillLevelsModel()
                        {
                            SkillLevelID = Convert.ToInt32(reader["SkillLevelID"]),
                            LevelName = reader["LevelName"].ToString(),
                            SkillLevel = Convert.ToInt32(reader["SkillLevelValue"]),
                            SkillLevelGroupID = Convert.ToInt32(reader["SkillLevelGroupID"])
                        });
                    }
                }
            }
            return list;
        }

        public List<SkillLevelGroupModel> GetAllSkillLevelGroups()
        {
            List<SkillLevelGroupModel> list = new List<SkillLevelGroupModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from skill_level_group", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new SkillLevelGroupModel()
                        {
                            SkillLevelGroupID = Convert.ToInt32(reader["SkillLevelGroupID"]),
                            GroupName = reader["GroupName"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<PersonModel> GetAllPeople()
        {
            List<PersonModel> list = new List<PersonModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select p.PersonID, p.FullName, l.LocationName,  " +
                                                    " p.Email " +
                                                    "from people as p " +
                                                    
                                                    "inner join location as l on p.LocationID = l.LocationID", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new PersonModel()
                        {
                            PersonID = Convert.ToInt32(reader["PersonID"]),
                            Name = reader["FullName"].ToString(),
                            RoleName = "no role",
                            LocationName = reader["LocationName"].ToString(),
                            Email = reader["Email"].ToString()
                        });
                    }
                }
            }
            list = list.OrderBy(PersonModel => PersonModel.Name).ToList();
            return list;
        }

        public List<LocationModel> GetAllLocations()
        {
            List<LocationModel> list = new List<LocationModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from location", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new LocationModel()
                        {
                            LocationID = Convert.ToInt32(reader["LocationID"]),
                            LocationName = reader["LocationName"].ToString()
                        });
                    }
                }
            }
            list = list.OrderBy(LocationModel => LocationModel.LocationName).ToList();
            return list;
        }

        public List<RoleModel> GetAllRoles()
        {
            List<RoleModel> list = new List<RoleModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from role", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RoleModel()
                        {
                            RoleID = Convert.ToInt32(reader["RoleID"]),
                            RoleName = reader["RoleName"].ToString()
                        });
                    }
                }
            }
            list = list.OrderBy(RoleModel => RoleModel.RoleName).ToList();
            return list;
        }

        public List<ProjectModel> GetAllProjects()
        {
            List<ProjectModel> list = new List<ProjectModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from project", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ProjectModel()
                        {
                            ProjectID = Convert.ToInt32(reader["ProjectID"]),
                            ProjectName = reader["ProjectName"].ToString()
                        });
                    }
                }
            }
            list = list.OrderBy(ProjectModel => ProjectModel.ProjectName).ToList();
            return list;
        }

        public List<ManagerModel> GetAllManagers()
        {
            List<ManagerModel> list = new List<ManagerModel>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select p.PersonID, p.FullName, m.ManagerID, m.Level" +
                                                    "from manager as m" +
                                                    "inner join people as p ON m.PersonID = p.PersonID", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        list.Add(new ManagerModel()
                        {
                            PersonID = Convert.ToInt32(reader["PersonID"]),
                            Name = reader["FullName"].ToString(),
                            ManagerID = Convert.ToInt32(reader["ManagerID"]),
                            Level = Convert.ToInt32(reader["Level"])
                        });
                    }
                }
            }
            list = list.OrderBy(ManagerModel => ManagerModel.Name).ToList();
            return list;
        } 
 
        public List<SkillsModel> GetSkillsForEmployee(string full_name)
        {
            List<SkillsModel> list = new List<SkillsModel>();

            using (MySqlConnection conn = GetConnection())
            {
                string usernameSearch = full_name;
                if (String.IsNullOrEmpty(full_name))
                {
                    usernameSearch = "%";
                }

                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT p.PersonID, s.SkillID, s.SkillName, " +
                                                    "g.GroupName, gp.ParentGroupName, slr.LevelName as 'SkillLevelRequired', slr.skilllevelvalue as 'SkillRequiredValue', " +
                                                    "slh.LevelName as 'SkillLevelHeld', slh.skilllevelvalue as 'SkillHeldValue'" +
                                                    "from people p, employee_skill e, skill s, skillgroup g, skillgroup_parent gp, skill_level slr, skill_level slh " +
                                                    "where p.fullname like '" + usernameSearch + "' and " +
                                                    "p.personid = e.personid and " +
                                                    "s.skillid = e.skillid and " +
                                                    "g.skillgroupid = s.SkillGroupID and " +
                                                    "gp.SkillGroupParentID = g.SkillGroupParentID and " +
                                                    "slh.SkillLevelID = e.SkillLevelID and " +
                                                    "slr.SkillLevelID = e.SkillLevelRequiredID", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {


                        list.Add(new SkillsModel()
                        {
                            SkillID = Convert.ToInt32(reader["SkillID"].ToString()),
                            PersonID = Convert.ToInt32(reader["PersonID"].ToString()),
                            SkillName = reader["SkillName"].ToString(),
                            GroupName = reader["GroupName"].ToString(),
                            GroupParentName = reader["ParentGroupName"].ToString(),
                            RequiredSkillLevel = reader["SkillLevelRequired"].ToString(),
                            RequireSkillLevelValue = Convert.ToInt32(reader["SkillRequiredValue"].ToString()),
                            HeldSkillLevel = reader["SkillLevelHeld"].ToString(),
                            HeldSkillLevelValue = Convert.ToInt32(reader["SkillHeldValue"].ToString())
                        }) ;
                    }
                }
            }
            return list;
        }

        


        public int SaveManagerSkill(PersonModel person, SkillsModel skill)
        {
            int ret = 0;

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT process_manager_skill(@ManagerName,@FullName,@Email,@PersonnelNo,@SkillName,@GroupName,@ParentName,@ReqLevel,@HeldLevel)";
                cmd.Parameters.Add("@ManagerName", MySqlDbType.VarChar, 45).Value = person.ManagerName;
                cmd.Parameters.Add("@FullName", MySqlDbType.VarChar, 45).Value = person.Name;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar, 320).Value = person.Email;
                cmd.Parameters.Add("@PersonnelNo", MySqlDbType.Int32).Value = person.PersonnelNo;
                cmd.Parameters.Add("@SkillName", MySqlDbType.VarChar, 120).Value = skill.SkillName;
                cmd.Parameters.Add("@GroupName", MySqlDbType.VarChar, 120).Value = skill.GroupName;
                cmd.Parameters.Add("@ParentName", MySqlDbType.VarChar, 120).Value = skill.GroupParentName;
                cmd.Parameters.Add("@ReqLevel", MySqlDbType.VarChar, 45).Value = skill.RequiredSkillLevel;
                cmd.Parameters.Add("@HeldLevel", MySqlDbType.VarChar, 45).Value = skill.HeldSkillLevel;


                ret = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return ret;
        }
        
        public int SavePpmData(PersonModel person, ProjectModel project, AssignmentModel projAssign)
        {
            int ret = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT process_ppm_data(@FullName,@Email,@PersonnelNo,@Status,@Project,@ProjStart,@ProjEnd,@ProjUtil)";
                
                cmd.Parameters.Add("@FullName", MySqlDbType.VarChar, 45).Value = person.Name;
                cmd.Parameters.Add("@Email", MySqlDbType.VarChar, 320).Value = person.Email;
                cmd.Parameters.Add("@PersonnelNo", MySqlDbType.Int32).Value = person.PersonnelNo;
                cmd.Parameters.Add("@Status", MySqlDbType.VarChar, 45).Value = projAssign.AssignmentStatus;
                cmd.Parameters.Add("@Project", MySqlDbType.VarChar, 45).Value = project.ProjectName;
                cmd.Parameters.Add("@ProjStart", MySqlDbType.Date).Value = projAssign.ProjectStartDate;
                cmd.Parameters.Add("@ProjEnd", MySqlDbType.Date).Value = projAssign.ProjectEndDate;
                cmd.Parameters.Add("@ProjUtil", MySqlDbType.Double).Value = projAssign.Utilisation;

                ret = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return ret;
        }

        public int SaveDistData(PersonModel person, LocationModel locn)
        {
            int ret = 0;
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();

                cmd.CommandText = "SELECT process_dist_data(@FullName,@Location)";

                cmd.Parameters.Add("@FullName", MySqlDbType.VarChar, 45).Value = person.Name;
                cmd.Parameters.Add("@Location", MySqlDbType.VarChar, 45).Value = locn.LocationName;


                ret = (int)cmd.ExecuteScalar();
                conn.Close();
            }
            return ret;
        }
    
    
    }
}
