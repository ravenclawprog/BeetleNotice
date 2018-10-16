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
    /// Логика взаимодействия для UserAdd.xaml
    /// </summary>
    public partial class UserAddWindow : Window
    {
        public UserAddWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.MainWindow.Show();
        }

        private void BtnUsrAddWindow_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            try
            {
                user.FIO = LblUsrFIO.Text;
                user.Post = LblUsrPost.Text;
                user.Department = LblUsrDepartment.Text;
            }
            catch (Exception ee)
            {
                BugTrackingLogger.Logger.Error("{0} Exception caught.", ee);
                MessageBox.Show("Ошибка при заполнении полей данными", "Ошибка");
            }
            MainWindow.dd.InsertUser(ref user);
            this.Close();
        }
    }
}
