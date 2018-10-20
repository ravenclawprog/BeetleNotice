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
using BeetleDB;
using BeetleLog;
using BeetleClasses;

namespace BeetlesNotice
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class ProjectAddWindow : Window
    {
        public delegate void UpdateProjectContainer();
        public event UpdateProjectContainer OnAddProject;

        public ProjectAddWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Project project = new Project();
            try
            {
                project.ProjectName = LblPrjName.Text;
            }
            catch (Exception ee)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", ee);
                MessageBox.Show("Ошибка при вводе имени проекта","Ошибка при вводе данных.");
            }
            MainWindow.dd.InsertProject(ref project);
            Application.Current.MainWindow.Show();
            this.Close();
        }

        private void PrAddWindow_Closed(object sender, EventArgs e)
        {
            OnAddProject();
            Application.Current.MainWindow.Show();
        }
    }
}
