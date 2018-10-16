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
        public TaskAddWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            CmbBoxUser.ItemsSource = MainWindow.dd.SelectUsers();
            CmbBoxProject.ItemsSource = MainWindow.dd.SelectProjects();
        }

        private void BtnTskAddWindow_Click(object sender, RoutedEventArgs e)
        {
            Project pr;
            if (CmbBoxProject.SelectedItem != null)
            {
                pr = CmbBoxProject.SelectedItem as Project;
            }
            else
            {
                MessageBox.Show("Вы не выбрали проект, к которому будет прикреплена задача!", "Ошибка");
                return;
            }
            User usr;
            if (CmbBoxUser.SelectedItem != null)
            {
                usr = CmbBoxProject.SelectedItem as User;
            }
            else
            {
                MessageBox.Show("Вы не выбрали пользователя, к которому будет прикреплена задача!", "Ошибка");
                return;
            }
            try
            {
                BeetleClasses.Task tsk = new BeetleClasses.Task();
                tsk.Theme = LblTaskTheme.Text;
                tsk.TypeOfTask = LblTaskType.Text;
                tsk.Priority = int.Parse(LblTaskPriority.Text);
                tsk.Description = LblTaskDescription.Text;
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
            Application.Current.MainWindow.Show();
        }
    }
}
