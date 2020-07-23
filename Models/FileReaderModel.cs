using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.Cryptography;

using Excel = Microsoft.Office.Interop.Excel;
using System.Text;

namespace SkillsMatrix.Models
{
    public class FileReaderModel
    {
        private SkillsMatrixDB database;

        private List<string> ManagerColumns = new List<string>()
        {
            "Manager Full Name",
            "Person Full Name",
            "Person Person No",
            "Person E-mail",
            "Skill Name",
            "Skill Group Name",
            "Skill Group Parent Name",
            "Skill Required Level Name",
            "Skill Held Level Name"
        };
        private List<string> PpmColumns = new List<string>()
        {
            "Employee Number",
            "Full Name",
            "Email",
            "Assignment Status",
            "Fin. Client",
            "Assignment Start",
            "Assignment End",
            "Mgr"
        };
        private List<string> DistColumns = new List<string>()
        {
            "Manager Full Name",
            "Person Full Name",
            "Person Location Name"
        };

        public enum FileReaderResults { FileReaderSuccess, FileReaderUnknownUser, FileReaderWrongFormat, FileReaderErrorProcessingFile, FileReaderUnknownError};

        private enum ManagerItem { ManagerName, FullName, PersonnelNo, Email, SkillName, SkillGroupName, SkillGroupParentName, RequiredSkill, HeldSkill};
        private enum PpmItem { PpmPersonnelNo, PpmFullName, PpmEmail, PpmStatus, PpmClient, PpmStart, PpmEnd, PpmMgr };
        private enum DistItem { MgrFullName, DistFullName, DistLocation};
        private Dictionary<string, string> LocationMap = new Dictionary<string, string>()
        {
            { "1be35906dcedb6f2cf186d4af52fa6dd99aca3e8093bce3bb0dd094d4606b6f1", "Percival" },
            { "151ad1cc151655ac9d80cf597e23882ae0082625c00c8759e60f535116a51212", "Lancelot" },
            { "1b30470dc34b4305d1e312ae8fd42b36d279039e876a9b3f9434e87520a7126b", "Gawain" },
            { "e1203631f0705ee3dbe71e80fe4288b981447ece5dffee7a8bdf0fdb4b4a0be7", "Bedivere" },
            { "faca43a96c2fb4f5dff7ac8b85d0d57b216a93bec23679f93ae448c50068ddbb", "Galahad" },
            { "4b25c6dcfed416f61a72cd81caac11db99fb6754f78401f57b31f54113b10f8b", "Lamorak" },
            { "d9499479f60b614ac218b700c5d85f9c6ad21db3270f05429dd9891b1d750d8a", "Tristan" }
        };

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void CleanupExcel()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public FileReaderResults AddManagerSkillsReport(SkillsMatrixDB database, string filePath)
        {
            FileReaderResults ret = FileReaderResults.FileReaderUnknownError;

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Excel._Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets[1];

            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            int currentRow = 1;
            
            int matches = 0;

            //iterate over the rows and columns 
            //excel is not zero based!!
            for (int col = 1; col <= colCount; col++)
            {
                Excel.Range cell = (Excel.Range)xlWorksheet.Cells[currentRow, col];
                if (cell.Value2 != null)
                {
                    if (ManagerColumns.Contains(cell.Value2))
                    {
                        matches++;
                    }
                }
                Marshal.FinalReleaseComObject(cell);
            }
            
            if (matches == ManagerColumns.Count)
            {
                ret = FileReaderResults.FileReaderSuccess;
            }
            else
            {
                ret = FileReaderResults.FileReaderWrongFormat;
            }

            try
            {
                if (ret == FileReaderResults.FileReaderSuccess)
                {
                    for (currentRow++; currentRow <= rowCount; currentRow++)
                    {
                        SkillsModel skill = new SkillsModel();
                        PersonModel person = new PersonModel();

                        for (int col = 1; col <= colCount; col++)
                        {
                            Excel.Range cell = (Excel.Range)xlWorksheet.Cells[currentRow, col];

                            ManagerItem colIdx = (ManagerItem)(col - 1);

                            switch (colIdx)
                            {
                                case ManagerItem.ManagerName:
                                    person.ManagerName = cell.Value2.ToString();
                                    break;
                                case ManagerItem.FullName:
                                    person.Name = cell.Value2.ToString();
                                    break;
                                case ManagerItem.PersonnelNo:
                                    person.PersonnelNo = Int32.Parse(cell.Value2.ToString());
                                    break;
                                case ManagerItem.Email:
                                    person.Email = cell.Value2.ToString();
                                    break;
                                case ManagerItem.SkillName:
                                    skill.SkillName = cell.Value2.ToString();
                                    break;
                                case ManagerItem.SkillGroupName:
                                    skill.GroupName = cell.Value2.ToString();
                                    break;
                                case ManagerItem.SkillGroupParentName:
                                    skill.GroupParentName = cell.Value2.ToString();
                                    break;
                                case ManagerItem.RequiredSkill:
                                    if (cell.Value2 != null)
                                    {
                                        skill.RequiredSkillLevel = cell.Value2.ToString();
                                    }
                                    else
                                    {
                                        skill.RequiredSkillLevel = "Not Set";
                                    }
                                    break;
                                case ManagerItem.HeldSkill:
                                    if (cell.Value2 != null)
                                    {
                                        skill.HeldSkillLevel = cell.Value2.ToString();
                                    }
                                    else
                                    {
                                        skill.HeldSkillLevel = "Not Set";
                                    }
                                    break;
                            }
                            Marshal.FinalReleaseComObject(cell);
                        }

                        if (database.SaveManagerSkill(person, skill) != 1)
                        {
                            ret = FileReaderResults.FileReaderErrorProcessingFile;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = FileReaderResults.FileReaderUnknownError;
            }
            finally
            {


                //close and release
                Marshal.FinalReleaseComObject(xlRange);
                Marshal.FinalReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.FinalReleaseComObject(xlWorkbook);


                //quit and release
                xlApp.Quit();
                Marshal.FinalReleaseComObject(xlApp);

                // take the trash out
                CleanupExcel();
            }

            return ret;
                
        }

        public FileReaderResults AddPpmReport(SkillsMatrixDB database, string filePath, out List<string> unknownUsers)
        {
            FileReaderResults ret = FileReaderResults.FileReaderUnknownError;
            unknownUsers = new List<string>();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Excel.Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets[1];

            Excel.Range xlRange = xlWorksheet.UsedRange;
           
            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            int currentRow = 1;

            int matches = 0;

            //iterate over the rows and columns 
            //excel is not zero based!!
            for (int col = 1; col <= colCount; col++)
            {
                Excel.Range cell = (Excel.Range)xlWorksheet.Cells[currentRow, col];
                if (cell.Value2 != null)
                {
                    if (PpmColumns.Contains(cell.Value2))
                    {
                        matches++;
                    }
                }
                Marshal.FinalReleaseComObject(cell);
            }

            if (matches == PpmColumns.Count)
            {
                ret = FileReaderResults.FileReaderSuccess;
            }
            else 
            {
                ret = FileReaderResults.FileReaderWrongFormat;

            }

            try
            {
                if (ret == FileReaderResults.FileReaderSuccess)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ppm_errors.txt");
                    string currUser = "";
                    string currProj = "";
                    double currUtilise = 0;

                    
                
                    for (currentRow++; currentRow <= rowCount; currentRow++)
                    {
                        ProjectModel project = new ProjectModel();
                        PersonModel person = new PersonModel();
                        AssignmentModel projAssign = new AssignmentModel();
                        int numUtilisations = 0;
                        double cumulativeUtilisation = 0;


                        for (int col = 1; col <= colCount; col++)
                        {
                            Excel.Range cell = (Excel.Range)xlWorksheet.Cells[currentRow, col];



                            if ((col - 1) <= (int)PpmItem.PpmMgr)
                            {

                                PpmItem colIdx = (PpmItem)(col - 1);
                                try
                                {
                                    switch (colIdx)
                                    {
                                        case PpmItem.PpmMgr:
                                            // manager doesn't seem to be populated, so ignore it (we don't pass it to the DB function)
                                            //                               person.ManagerName = cell.Value2.ToString();
                                            break;
                                        case PpmItem.PpmFullName:
                                            person.Name = cell.Value2.ToString();
                                            break;
                                        case PpmItem.PpmPersonnelNo:
                                            person.PersonnelNo = Int32.Parse(cell.Value2.ToString());
                                            break;
                                        case PpmItem.PpmEmail:
                                            person.Email = cell.Value2.ToString();
                                            break;
                                        case PpmItem.PpmClient:
                                            project.ProjectName = cell.Value2.ToString();
                                            break;
                                        case PpmItem.PpmStart:
                                            projAssign.ProjectStartDate = DateTime.Parse(cell.Value.ToString());
                                            break;
                                        case PpmItem.PpmEnd:
                                            projAssign.ProjectEndDate = DateTime.Parse(cell.Value.ToString());
                                            break;
                                        case PpmItem.PpmStatus:
                                            projAssign.AssignmentStatus = cell.Value2.ToString();
                                            break;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    ret = FileReaderResults.FileReaderErrorProcessingFile;
                                }

                            }
                            else
                            {
                                double utilVal = (double)cell.Value2;

                                if (utilVal > 0)
                                {
                                    numUtilisations++;
                                    cumulativeUtilisation += utilVal;
                                }
                            }
                            Marshal.FinalReleaseComObject(cell);
                        }
                        if (numUtilisations > 0)
                        {
                            projAssign.Utilisation = (cumulativeUtilisation / numUtilisations);
                        }
                        else
                        {
                            projAssign.Utilisation = 0;
                        }
                        if (String.Compare(currUser, person.Name) == 0)
                        {
                            if (String.Compare(currProj, project.ProjectName) == 0)
                            {
                                currUtilise += projAssign.Utilisation;
                                projAssign.Utilisation = currUtilise;
                            }
                            else
                            {
                                currUtilise = 0;
                                currProj = project.ProjectName;
                            }
                        }
                        else
                        {
                            currUser = person.Name;
                            currProj = project.ProjectName;
                            currUtilise = projAssign.Utilisation;
                        }
                        if (database.SavePpmData(person, project, projAssign) != 1)
                        {
                            ret = FileReaderResults.FileReaderUnknownUser;
                            unknownUsers.Add(person.Name);
                        }
                    }
                
                }
            }
            catch (Exception ex)
            {
                ret = FileReaderResults.FileReaderUnknownError;
            }
            finally
            {
                Marshal.FinalReleaseComObject(xlRange);
                Marshal.FinalReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.FinalReleaseComObject(xlWorkbook);


                //quit and release
                xlApp.Quit();
                Marshal.FinalReleaseComObject(xlApp);

                // take the trash out
                CleanupExcel();
            }

            return ret;
        }

        public FileReaderResults AddDistReport(SkillsMatrixDB database, string filePath, out List<string> unknownUsers)
        {
            FileReaderResults ret = FileReaderResults.FileReaderUnknownError;
            unknownUsers = new List<string>();

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(filePath);
            Excel._Worksheet xlWorksheet = (Excel.Worksheet)xlWorkbook.Sheets[1];

            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;
            int currentRow = 1;

            int matches = 0;
            try
            {


                //iterate over the rows and columns 
                //excel is not zero based!!
                for (int col = 1; col <= colCount; col++)
                {
                    Excel.Range cell = (Excel.Range)xlWorksheet.Cells[currentRow, col];
                    if (cell.Value2 != null)
                    {
                        if (DistColumns.Contains(cell.Value2))
                        {
                            matches++;
                        }
                    }
                    Marshal.FinalReleaseComObject(cell);
                }

                if (matches == DistColumns.Count)
                {
                    ret = FileReaderResults.FileReaderSuccess;
                }
                else
                {
                    ret = FileReaderResults.FileReaderWrongFormat;
                }

                if (ret == FileReaderResults.FileReaderSuccess)
                {
                    
                
                    for (currentRow++; currentRow <= rowCount; currentRow++)
                    {
                        PersonModel person = new PersonModel();
                        LocationModel locn = new LocationModel();


                        for (int col = 1; col <= colCount; col++)
                        {
                            Excel.Range cell = (Excel.Range)xlWorksheet.Cells[currentRow, col];

                            DistItem colIdx = (DistItem)(col - 1);

                            switch (colIdx)
                            {
                                case DistItem.MgrFullName:
                                    // we only process people who are in the database already, so no need to get the manager
                                    //                               person.ManagerName = cell.Value2.ToString();
                                    break;
                                case DistItem.DistFullName:
                                    person.Name = cell.Value2.ToString();
                                    break;
                                case DistItem.DistLocation:
                                    string locStr = cell.Value2.ToString();
                                    string hashCode = ComputeSha256Hash(locStr);
                                    if (LocationMap.ContainsKey(hashCode))
                                    {
                                        locn.LocationName = LocationMap[hashCode];
                                    }
                                    else
                                    {
                                        locn.LocationName = "Unknown";
                                    }
                                    break;


                            }
                            Marshal.FinalReleaseComObject(cell);
                        }

                        if (database.SaveDistData(person, locn) != 1)
                        {
                            ret = FileReaderResults.FileReaderUnknownUser;
                            unknownUsers.Add(person.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ret = FileReaderResults.FileReaderUnknownError;
            }
            finally
            {
                //close and release
                Marshal.FinalReleaseComObject(xlRange);
                Marshal.FinalReleaseComObject(xlWorksheet);

                //close and release
                xlWorkbook.Close();
                Marshal.FinalReleaseComObject(xlWorkbook);


                //quit and release
                xlApp.Quit();
                Marshal.FinalReleaseComObject(xlApp);

                // take the trash out
                CleanupExcel();
            }


            return ret;
            }
    }
}
