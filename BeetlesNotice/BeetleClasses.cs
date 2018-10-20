using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleClasses
{
    public class Project: ICloneable
    {
        private int _projectID;
        private string _projectName;
        public const int _projectNameLength = 50;
        public int ProjectNameLength()
        {
            return _projectNameLength;
        }
        public int ProjectID
        {
            get
            {
                return _projectID;
            }
            set
            {
                _projectID = value;
            }
        }
        public string ProjectName
        {
            get
            {
                return _projectName != null ? _projectName : "NA";
            }
            set
            {
                if(value.Length > _projectNameLength)
                {
                    _projectName = value.Substring(0, _projectNameLength);
                } 
                else
                {
                    _projectName = value;
                }
            }
        }
        public override string ToString()
        {
            return "ProjectName: " + ProjectName + "; ProjectID: " + ProjectID.ToString();
        }

        public static explicit operator string(Project v)
        {
            return "ProjectName: " + v.ProjectName + "; ProjectID: " + v.ProjectID.ToString() ;
            throw new NotImplementedException();
        }
        public object Clone()
        {
            return new Project
            {
                ProjectID = this.ProjectID,
                ProjectName = this.ProjectName
            };
        }
    }

    public class Task: ICloneable
    {
        /*private variables*/
        private int _taskID;
        private string _theme;
        private string _typeOfTask;
        private int _priority;
        private string _description;
        /*constants*/
        private const int _themeLength = 50;
        private const int _typeOfTaskLength = 20;
        private const int _descriptionLength = 255;
        /*priority check*/
        private int PriorityCheck(int mynumb)
        {
            if (mynumb < 0 || mynumb > 32)
            {
                return -1;
            }
            else
            {
                return mynumb;
            }
        }
        public int ThemeLength()
        {
            return _themeLength; 
        }
        public int TypeOfTaskLength()
        {
            return _typeOfTaskLength; 
        }
        public int DescriptionLength()
        {
             return _descriptionLength; 
        }
        public int TaskID
        {
            get
            {
                return _taskID;
            }
            set
            {
                _taskID = value;
            }
        }
        public string Theme
        {
            get
            {
                return _theme != null ? _theme : "NA";
            }
            set
            {
                if (value.Length > _themeLength)
                {
                    _theme = value.Substring(0, _themeLength);
                }
                else
                {
                    _theme = value;
                }
            }
        }
        public string TypeOfTask
        {
            get
            {
                return _typeOfTask != null ? _typeOfTask : "NA";
            }
            set
            {
                if (value.Length > _typeOfTaskLength)
                {
                    _typeOfTask = value.Substring(0, _typeOfTaskLength);
                }
                else
                {
                    _typeOfTask = value;
                }
            }
        }
        public int Priority
        {
            get { return _priority;  }
            set { _priority = PriorityCheck(value); }
        }
        public string Description
        {
            get
            {
                return _description != null ? _description : "NA";
            }
            set
            {
                if (value.Length > _descriptionLength)
                {
                    _description = value.Substring(0, _descriptionLength);
                }
                else
                {
                    _description = value;
                }
            }
        }
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
        public object Clone()
        {
            return new Task
            {
                TaskID = this.TaskID,
                Theme = this.Theme,
                TypeOfTask = this.TypeOfTask,
                Priority = this.Priority,
                Description = this.Description
            };
        }
    }

    public class User: ICloneable
    {
        private int _userID;
        private string _fio;
        private string _post;
        private string _department;
        private const int _fioLength = 50;
        private const int _postLength = 20;
        private const int _departmentLength = 40;
        public int FIOLength()
        {
            return _fioLength;
        }
        public int PostLength()
        {
            return _postLength;
        }
        public int DepartmentLength()
        {
            return _departmentLength;
        }
        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }
        public string FIO
        {
            get
            {
                return _fio != null ? _fio : "NA";
            }
            set
            {
                if (value.Length > _fioLength)
                {
                    _fio = value.Substring(0, _fioLength);
                }
                else
                {
                    _fio = value;
                }
            }
        }
        public string Post
        {
            get
            {
                return _post != null ? _post : "NA";
            }
            set
            {
                if (value.Length > _postLength)
                {
                    _post = value.Substring(0, _postLength);
                }
                else
                {
                    _post = value;
                }
            }
        }
        public string Department
        {
            get
            {
                return _department != null ? _department : "NA";
            }
            set
            {
                if (value.Length > _departmentLength)
                {
                    _department = value.Substring(0, _departmentLength);
                }
                else
                {
                    _department = value;
                }
            }
        }
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
        public object Clone()
        {
            return new User
            {
                UserID = this.UserID,
                FIO = this.FIO,
                Post = this.Post,
                Department = this.Department
            };
        }
    }
}
