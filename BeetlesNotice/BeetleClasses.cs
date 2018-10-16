using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleClasses
{
    public class Project
    {
        public int ProjectID { get; set; }
        public string ProjectName { get; set;}
        public override string ToString()
        {
            return "ProjectName: " + ProjectName + "; ProjectID: " + ProjectID.ToString();
        }

        public static explicit operator string(Project v)
        {
            return "ProjectName: " + v.ProjectName + "; ProjectID: " + v.ProjectID.ToString() ;
            throw new NotImplementedException();
        }
    }

    public class Task
    {
        public int TaskID { get; set; }
        public string Theme { get; set; }
        public string TypeOfTask { get; set; }
        public int Priority { get; set; }
        public string Description { get; set; }
        public override string ToString()
        {
            return "TaskID: " + TaskID.ToString() + "; Theme: " + Theme + "; TypeOfTask: " + TypeOfTask +
                ";Priority: " + Priority.ToString() + "; Description: " + Description;
        }
        public static explicit operator string(Task v)
        {
            return "TaskID: " + v.TaskID.ToString() + "; Theme: " + v.Theme + "; TypeOfTask: " + v.TypeOfTask +
                ";Priority: " + v.Priority.ToString() + "; Description: " + v.Description;
            throw new NotImplementedException();
        }
    }

    public class User
    {
        public int UserID { get; set; }
        public string FIO { get; set; }
        public string Post { get; set; }
        public string Department { get; set; }
        public override string ToString()
        {
            return "UserID: " + UserID.ToString() + "; FIO: " + FIO + 
                "; Post: " + Post + "; Department: " + Department;
        }

        public static explicit operator string(User v)
        {
            return "UserID: " + v.UserID.ToString() + "; FIO: " + v.FIO +
                "; Post: " + v.Post + "; Department: " + v.Department;
            throw new NotImplementedException();
        }
    }
}
