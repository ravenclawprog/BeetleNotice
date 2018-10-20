using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BeetleLog;
using BeetleDB;
using BeetleClasses;


namespace BeetlesNotice
{
    
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        public static BugTrackingDB dd;
        public void UpdateProjects()
        {
            DGridProject.ItemsSource = dd.SelectProjects();
        }
        public void UpdateTasks()
        {
            DGridTask.ItemsSource = dd.SelectTasks();
        }
        public void UpdateUsers()
        {
            DGridUser.ItemsSource = dd.SelectUsers();
        }
        public MainWindow()
        {
            InitializeComponent();
            BugTrackingLogger.InitLogger();
            LblDBFName.Content = "Имя файла БД:";
            dd = new BugTrackingDB("barkToTheMoon.db");
            LblDBFName.Content = "Имя файла БД: "+dd.DBName();
            CmbBoxUsr.ItemsSource = dd.SelectUsers();
        }
        
        private void GeneralWindow_Closed(object sender, EventArgs e)
        {
            BugTrackingLogger.ClearTheLogger();
        }

        private void BtnDBOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                dd.ConnectToCreatedDataBase(openFileDialog.FileName);
                LblDBFName.Content = "Имя файла БД: " + dd.DBName();
                UpdateUsers();
                UpdateProjects();
                UpdateTasks();
            }
        }

        private void BtnDBCreate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                dd.ConnectToCreatedDataBase(saveFileDialog.FileName);
                dd.CreateDB();
                dd.CreateTriggers();
                LblDBFName.Content = "Имя файла БД: " + dd.DBName();
                UpdateUsers();
                UpdateProjects();
                UpdateTasks();
            }
        }

        private void BtnListPrj_Click(object sender, RoutedEventArgs e)
        {
            UpdateProjects();
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectAddWindow w = new ProjectAddWindow();
            w.OnAddProject += UpdateProjects;
            Application.Current.MainWindow.Hide();
            w.Show();
        }

        private void BtnDelPrj_Click(object sender, RoutedEventArgs e)
        {
            Project pr = DGridProject.SelectedItem as Project;
            dd.DeleteProject(ref pr);
            UpdateProjects();
        }

        private void BtnUsrSelect_Click(object sender, RoutedEventArgs e)
        {
            UpdateUsers();
        }

        private void BtnUsrAdd_Click(object sender, RoutedEventArgs e)
        {
            UserAddWindow w = new UserAddWindow();
            w.OnAddUser += UpdateUsers;
            Application.Current.MainWindow.Hide();
            w.Show();
        }

        private void BtnDelUsr_Click(object sender, RoutedEventArgs e)
        {
            User usr = DGridUser.SelectedItem as User;
            dd.DeleteUser(ref usr);
            dd.DeleteUnlinkedtasks();//после удаления пользователя необходимо также удалить задачи, которых нету в UserTaskCon
            UpdateUsers();
        }

        private void BtnTaskSelect_Click(object sender, RoutedEventArgs e)
        {
            UpdateTasks();
        }

        private void BtnTaskUsersSelect_Click(object sender, RoutedEventArgs e)
        {
            if (CmbBoxUsr.SelectedItem != null)
            {
                User user = CmbBoxUsr.SelectedItem as User;
                DGridTask.ItemsSource = dd.SelectTasks(user);
            }
            else
            {
                MessageBox.Show("Вы не выбрали пользователя для выбора задачи","Beetle Notice");
            }
            
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource == this.MainTabControl)
            {
                if (this.MainTabControl.SelectedIndex == 1)
                {
                    CmbBoxUsr.ItemsSource = dd.SelectUsers();
                    UpdateTasks();
                }
                if(this.MainTabControl.SelectedIndex == 0)
                {
                    UpdateProjects();
                }
                if (this.MainTabControl.SelectedIndex == 2)
                {
                    UpdateUsers();
                }
            }
        }

        private void BtnDelTask_Click(object sender, RoutedEventArgs e)
        {
            BeetleClasses.Task tsk = DGridTask.SelectedItem as BeetleClasses.Task;
            dd.DeleteTask(ref tsk);
            UpdateTasks();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TaskAddWindow w = new TaskAddWindow();
            w.OnAddTask += UpdateTasks;
            Application.Current.MainWindow.Hide();
            w.Show();
        }
    }
}
