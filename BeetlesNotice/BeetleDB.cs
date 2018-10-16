using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using BeetleLog;
using BeetleClasses;

namespace BeetleDB
{
    public class BugTrackingDB
    {
        private SQLiteConnection BTConnection;//соединение 
        private string BTName;//имя файла
        private string BTQueryString = "";//строка запроса
        private SQLiteCommand BTCommand;
        private const string BTConstName = "BugTracking.db";
        public string DBName()
        {
            return BTName;
        }
        public BugTrackingDB()
        {
            if (!System.IO.File.Exists(BTConstName))
                CreateNewDataBase(BTConstName);
            ConnectToCreatedDataBase(BTName);
            if(!this.IsCorrect())
                CreateDB();
            CreateTriggers();
        }
        public BugTrackingDB(string fname)
        {            
            ConnectToCreatedDataBase(fname);
            if (!this.IsCorrect())
                CreateDB();
            CreateTriggers();
        }
        /*~BugTrackingDB()
        {
            BTConnection.Close(); //доступ к ликвидированному объекту невозможен
        }*/
        public int CreateNewDataBase(string fname)
        {
            BugTrackingLogger.Logger.Information("Create file {0} for database.",fname);
            try
            {
                SQLiteConnection.CreateFile(fname);
                BTName = fname;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                BTName = "";
                return 1;
            }
            return 0;
        }

        public int ConnectToCreatedDataBase(string fname)
        {
            BugTrackingLogger.Logger.Information("Connected to file {0} database.", fname);
            try
            {
                if (!System.IO.File.Exists(fname))
                {
                        SQLiteConnection.CreateFile(fname);
                }
                BTConnection = new SQLiteConnection("Data Source=" + fname + ";Version=3;");
                BTConnection.Open();
                BTName = fname;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                BTName = "";
                return 1;
            }
            return 0;
        }
        public int CreateDB()
        {
            BugTrackingLogger.Logger.Information("Create database.");
            try
            {
                BugTrackingLogger.Logger.Debug("Create table Project.");
                BTQueryString = "CREATE TABLE IF NOT EXISTS Project(projectID INTEGER PRIMARY KEY AUTOINCREMENT," + //autoincrement очень тормозит, необходима замена
                    "name VARCHAR(50));";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BugTrackingLogger.Logger.Debug("Create table Task.");
                BTQueryString = "CREATE TABLE IF NOT EXISTS Task(taskID INTEGER PRIMARY KEY AUTOINCREMENT," + //autoincrement очень тормозит, необходима замена
                    "theme VARCHAR(50), typeOfTask VARCHAR(20), priority INTEGER, description TEXT, " +
                    "projectID INTEGER, FOREIGN KEY(projectID) REFERENCES Project(projectID));";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BugTrackingLogger.Logger.Debug("Create table User.");
                BTQueryString = "CREATE TABLE IF NOT EXISTS User(userID INTEGER PRIMARY KEY AUTOINCREMENT," + //autoincrement очень тормозит, необходима замена
                    "FIO VARCHAR(50), post VARCHAR(20), department varchar(40));";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BugTrackingLogger.Logger.Debug("Create table UserTaskCon.");
                BTQueryString = "CREATE TABLE IF NOT EXISTS UserTaskCon(uID INTEGER, tID INTEGER, " + //autoincrement очень тормозит, необходима замена
                    "FOREIGN KEY(uID) REFERENCES User(userID), FOREIGN KEY(tID) REFERENCES Task(taskID), PRIMARY KEY (uID, tID));";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return 1;
            }
            return 0;
        }
        public int CreateTriggers()
        {
            BugTrackingLogger.Logger.Information("Create trigger.");
            try
            {
                BugTrackingLogger.Logger.Debug("Create cascade delete from table Task trigger.");
                BTQueryString = "CREATE TRIGGER IF NOT EXISTS cascade_delete_UserTaskCon_from_trigger" +
                    " BEFORE DELETE ON Task " +
                    " FOR EACH ROW BEGIN " +
                    "   DELETE FROM UserTaskCon WHERE UserTaskCon.tID = OLD.taskID; " +
                    "END;";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BugTrackingLogger.Logger.Debug("Create cascade delete from table User trigger.");
                BTQueryString = "CREATE TRIGGER IF NOT EXISTS cascade_delete_user_from_trigger" +
                    " BEFORE DELETE ON User" +
                    " FOR EACH ROW BEGIN " +
                    "   DELETE FROM UserTaskCon WHERE UserTaskCon.uID = OLD.userID ; " +
                    " END;";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BugTrackingLogger.Logger.Debug("Create cascade delete from table UserTaskCon trigger.");
                BTQueryString = "CREATE TRIGGER IF NOT EXISTS cascade_delete_usertaskcon_from_trigger" +
                    " BEFORE DELETE ON UserTaskCon " +
                    " FOR EACH ROW BEGIN " +
                    "   DELETE FROM Task WHERE OLD.tID IN " +
                    " (SELECT Task.taskID FROM Task LEFT JOIN UserTaskCon ON Task.TaskID = UserTaskCon.TaskID" +
                    " WHERE UserTaskCon.tID IS NULL) ; " +
                    " END;";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BugTrackingLogger.Logger.Debug("Create cascade delete from table Project trigger.");
                BTQueryString = "CREATE TRIGGER IF NOT EXISTS cascade_delete_project_from_trigger" +
                    " BEFORE DELETE ON Project " +
                    " FOR EACH ROW BEGIN " +
                    "   DELETE FROM Task WHERE Task.projectID = OLD.projectID; " +
                    " END;";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return 1;
            }
            return 0;
        }

        public bool IsCorrect()
        {
            BugTrackingLogger.Logger.Information("Correct BD testing.");
            bool usrtskCor;
            bool tskCor;
            bool usrCor;
            bool projCor;
            try
            {
                BTQueryString = @"SELECT name FROM sqlite_master WHERE type='table' AND name='Project' ";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                projCor = BTCommand.ExecuteScalar()!=null;
                BTQueryString = @"SELECT name FROM sqlite_master WHERE type='table' AND name='User' ";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                usrCor = BTCommand.ExecuteScalar() != null;
                BTQueryString = @"SELECT name FROM sqlite_master WHERE type='table' AND name='Task' ";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                tskCor = BTCommand.ExecuteScalar() != null; ;
                BTQueryString = @"SELECT name FROM sqlite_master WHERE type='table' AND name='UserTaskCon' ";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                usrtskCor = BTCommand.ExecuteScalar() != null;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return false;
            }
            return usrtskCor && tskCor && usrCor && projCor;
        }
        public void InsertProject(ref Project pr)
        {
            BugTrackingLogger.Logger.Information("Insert project.");
            try
            {
                BTQueryString = String.Format("INSERT INTO Project(name) values (\"{0}\");", pr.ProjectName);
                BugTrackingLogger.Logger.Debug("projectName = {0}", pr.ProjectName);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
            }
        }

        public void InsertUser(ref User us)
        {
            BugTrackingLogger.Logger.Information("Insert user.");
            try
            {
                BTQueryString = String.Format("INSERT INTO User(FIO, post, department) values (\"{0}\",\"{1}\",\"{2}\");", 
                    us.FIO, us.Post, us.Department);
                BugTrackingLogger.Logger.Debug("FIO = {0} \n post = {1} \n department = {2}", us.FIO, us.Post, us.Department);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
            }
        }
        public void InsertTask(ref BeetleClasses.Task ts, ref Project pr, int usid)
        {
            BugTrackingLogger.Logger.Information("Insert task.");
            try
            {
                BTQueryString = String.Format("INSERT INTO Task(theme, typeOfTask, priority, description, projectID) values (\"{0}\",\"{1}\",{2},\"{3}\",\"{4}\");",
                    ts.Theme, ts.TypeOfTask, ts.Priority, ts.Description, pr.ProjectID);
                BugTrackingLogger.Logger.Debug("ProjectID = {4} \n Theme = {0} \n TypeOfTask = {1} \n Priority = {2} \n Description = {3}",
                    ts.Theme, ts.TypeOfTask, ts.Priority, ts.Description, pr.ProjectID);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

                BTQueryString = @"select last_insert_rowid()";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                long lastId = (long)BTCommand.ExecuteScalar();
                BugTrackingLogger.Logger.Debug("TaskID of last inserted task = {0}", lastId);

                BTQueryString = String.Format("INSERT INTO UserTaskCon(uID,tID) values ({0},{1});",
                    usid, lastId);
                BugTrackingLogger.Logger.Debug("UserID = {0} \n TaskID = {1}",
                    usid, lastId);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
            }
        }
        public int FillTable()
        {
            BugTrackingLogger.Logger.Debug("Fill table.");
            try
            {
                Project pr = new Project
                {
                    ProjectName = "My Project"
                };
                User us = new User();
                us.FIO = "Иванов Иван Иванович";
                us.Post = "Программист";
                us.Department = "Отдел разработки ПО";
                this.InsertProject(ref pr);

                BTQueryString = @"select last_insert_rowid()";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                long prID = (long)BTCommand.ExecuteScalar();
                pr.ProjectID = (int)prID;

                this.InsertUser(ref us);

                BTQueryString = @"select last_insert_rowid()";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                long lastId = (long)BTCommand.ExecuteScalar();

                BeetleClasses.Task ts = new BeetleClasses.Task();
                ts.Theme = "Разработка системы отслеживания задач";
                ts.Priority = 1;
                ts.TypeOfTask = "Разработка ПО";
                ts.Description = "Необходимо разработать систему по отслеживанию задач/ошибок (bug tracking system)";
                this.InsertTask(ref ts, ref pr, (int)lastId);
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return 1;
            }
            return 0;
        }

        public List<User> SelectUsers()
        {
            try
            {
                List<User> users = new List<User>();
                BugTrackingLogger.Logger.Debug("Select Users.");
                BTQueryString = "SELECT userID,FIO,post,department FROM User order by FIO desc";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                SQLiteDataReader reader = BTCommand.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User()
                    {
                        UserID = reader.GetInt32(0),
                        FIO = (string)reader["FIO"],
                        Post = (string)reader["post"],
                        Department = (string)reader["department"]
                    });
                }
                reader.Close();
                return users;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return null;
            }
        }

        public List<Project> SelectProjects()
        {
            try
            {
                List<Project> projects = new List<Project>();
                BugTrackingLogger.Logger.Debug("Select Project.");
                BTQueryString = "select projectID, name from Project";
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                SQLiteDataReader reader = BTCommand.ExecuteReader();
                while (reader.Read())
                {
                    projects.Add(new Project()
                    {
                        ProjectName = (string)reader["name"],
                        ProjectID = reader.GetInt32(0)
                    }
                    );
                }
                reader.Close();
                return projects;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return null;
            }
        }

        public List<BeetleClasses.Task> SelectTasks()
        {
            try
            {
                List<BeetleClasses.Task> tasks = new List<BeetleClasses.Task>();
                BugTrackingLogger.Logger.Debug("Select Task.");
                BTQueryString = String.Format("SELECT taskID, theme, typeOfTask, priority, description FROM Task ");
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                SQLiteDataReader reader = BTCommand.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new BeetleClasses.Task()
                    {
                        TaskID = reader.GetInt32(0),
                        Theme = (string)reader["theme"],
                        TypeOfTask = (string)reader["typeOfTask"],
                        Priority = reader.GetInt32(3),
                        Description = (string)reader["description"]
                    }
                    );
                }
                reader.Close();
                return tasks;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return null;
            }
        }

        public List<BeetleClasses.Task> SelectTasks(Project pr)
        {
            try
            {
                List<BeetleClasses.Task> tasks = new List<BeetleClasses.Task>();
                BugTrackingLogger.Logger.Debug("Select Task by Project.");
                BTQueryString = String.Format("SELECT taskID, theme, typeOfTask, priority, description FROM Task WHERE projectID={0};", pr.ProjectID);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                SQLiteDataReader reader = BTCommand.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new BeetleClasses.Task()
                    {
                        TaskID = reader.GetInt32(0),
                        Theme = (string)reader["theme"],
                        TypeOfTask = (string)reader["typeOfTask"],
                        Priority = reader.GetInt32(3),
                        Description = (string)reader["description"]
                    }
                    );
                }
                reader.Close();
                return tasks;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return null;
            }
        }

        public List<BeetleClasses.Task> SelectTasks(User usr)
        {
            try
            {
                List<BeetleClasses.Task> tasks = new List<BeetleClasses.Task>();
                BugTrackingLogger.Logger.Debug("Select Task by User.");
                BTQueryString = String.Format("SELECT taskID, theme, typeOfTask, priority, description FROM Task,UserTaskCon" +
                    " WHERE Task.taskID = UserTaskCon.tID AND UserTaskCon.uID={0};", usr.UserID);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                SQLiteDataReader reader = BTCommand.ExecuteReader();
                while (reader.Read())
                {
                    tasks.Add(new BeetleClasses.Task()
                    {
                        TaskID = reader.GetInt32(0),
                        Theme = (string)reader["theme"],
                        TypeOfTask = (string)reader["typeOfTask"],
                        Priority = reader.GetInt32(3),
                        Description = (string)reader["description"]
                    }
                    );
                }
                reader.Close();
                return tasks;
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
                return null;
            }
        }

        public void DeleteProject(ref Project pr)
        {
            BugTrackingLogger.Logger.Information("Delete project.");
            try
            {
                BTQueryString = String.Format("DELETE FROM Project where projectID={0};", pr.ProjectID);
                BugTrackingLogger.Logger.Debug("projectID = {0}; name = {1}", pr.ProjectID, pr.ProjectName);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
            }
        }

        public void DeleteUser(ref User usr)
        {
            BugTrackingLogger.Logger.Information("Delete user.");
            try
            {
                BTQueryString = String.Format("DELETE FROM User WHERE UserID={0};", usr.UserID);
                BugTrackingLogger.Logger.Debug("FIO = {0} \n post = {1} \n department = {2}", usr.FIO, usr.Post, usr.Department);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
            }
        }

        public void DeleteTask(ref BeetleClasses.Task tsk)
        {
            BugTrackingLogger.Logger.Information("Delete task.");
            try
            {
                BTQueryString = String.Format("DELETE FROM Task WHERE TaskID={0};", tsk.TaskID);
                BugTrackingLogger.Logger.Debug("Theme = {0} \n TypeOfTask = {1} \n Priority = {2} \n Description = {3}",
                    tsk.Theme, tsk.TypeOfTask, tsk.Priority, tsk.Description);
                BTCommand = new SQLiteCommand(BTQueryString, BTConnection);
                BTCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", e);
            }
        }
    }
}

