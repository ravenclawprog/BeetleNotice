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

namespace BeetleClasses
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BugTrackingDB dd;

        public MainWindow()
        {
            InitializeComponent();
            BugTrackingLogger.InitLogger();
            LblDBFName.Content = "Имя файла БД:";
            dd = new BugTrackingDB("barkToTheMoon.db");
            LblDBFName.Content = "Имя файла БД: "+dd.DBName();
        }

            /*List<User> users;

            dd.FillTable();
            users = dd.SelectUsers();
            TextOut.Text = "";
            foreach(User usr in users)
            {
                TextOut.Text += (string)usr;
            }*/
           /* List<Project> prj;
            Project pr = new Project();
            pr.ProjectName = "Hello1";
            dd.InsertProject(ref pr);
            prj = dd.SelectProjects();
            TextOut.Text = "";
            foreach (Project pp in prj)
            {
                TextOut.Text += (string)pp;
            }*/


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
            }
        }

        private void BtnListPrj_Click(object sender, RoutedEventArgs e)
        {
            DGridProject.ItemsSource = dd.SelectProjects();
        }
    }
}
