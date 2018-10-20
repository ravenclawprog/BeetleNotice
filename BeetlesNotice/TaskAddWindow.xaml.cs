using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BeetleClasses;
using BeetleLog;

namespace BeetlesNotice
{
    /// <summary>
    /// Логика взаимодействия для TaskAddWindow.xaml
    /// </summary>
    public partial class TaskAddWindow : Window
    {
        private Project _addproject;//
        private User _adduser;//changing name solve the problem....what?
        private BeetleClasses.Task _addtask;

        public delegate void UpdateTaskContainer();
        public event UpdateTaskContainer OnAddTask;// правильней было бы назвать OnCloseWindowAddTask

        public TaskAddWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {

            CmbBoxUser.ItemsSource = MainWindow.dd.SelectUsers(); ; 
            CmbBoxProject.ItemsSource = MainWindow.dd.SelectProjects();

        }

        private void BtnTskAddWindow_Click(object sender, RoutedEventArgs e)
        {
            _addtask = new BeetleClasses.Task();
            _adduser = new User();//maybe it is solve the problem
            _addproject = new Project();
            if (CmbBoxProject.SelectedIndex!=-1)
            {
                _addproject = (Project)CmbBoxProject.Items[CmbBoxProject.SelectedIndex];
            }
            else
            {
                MessageBox.Show("Вы не выбрали проект, к которому будет прикреплена задача!", "Ошибка");
                return;
            }
            
            if (CmbBoxUser.SelectedIndex != -1)
            {
                _adduser = (User)CmbBoxUser.Items[CmbBoxUser.SelectedIndex];               
            }
            else
            {
                MessageBox.Show("Вы не выбрали пользователя, к которому будет прикреплена задача!", "Ошибка");
                return;
            }
            try
            {
                
                _addtask.Theme = LblTaskTheme.Text;
                _addtask.TypeOfTask = LblTaskType.Text;
                _addtask.Priority = int.Parse(LblTaskPriority.Text);//замена по просьбе 

                _addtask.Description = LblTaskDescription.Text;
                MainWindow.dd.InsertTask(_addtask, _addproject, _adduser);
            }
             catch (Exception ee)
            {
                 BugTrackingLogger.Logger.Error("{0} Exception caught.", ee);
                 MessageBox.Show("Ошибка при заполнении полей данными", "Ошибка");
            }
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            OnAddTask();
            Application.Current.MainWindow.Show();
        }
    }
}
