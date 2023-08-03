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

using System.DirectoryServices;

namespace VMMC_Core.CommonControls
{
    /// <summary>
    /// Логика взаимодействия для LoginForm.xaml
    /// </summary>
    public partial class LoginForm : Window
    {
        public SessionInfo sessionInfo;
        private SessionInfo newSessionInfo;


        //List<OrganisationRolesInfo> OrganisationRolesInfoCollection { get; set; }
        public LoginForm()
        {
            newSessionInfo = new SessionInfo();

            newSessionInfo.HostName = Environment.MachineName;
            newSessionInfo.UserName = Environment.UserName;
            newSessionInfo.UserFIO = new DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName).Properties["FullName"].Value.ToString();
            newSessionInfo.ServerName = "server-db";
            newSessionInfo.DataBaseName = "InfoModelVMMK";

            InitializeComponent();
            ServerName_TextBox.Text = newSessionInfo.ServerName;
            DataBase_ComboBox.ItemsSource = newSessionInfo.AvalibaleDataBaseList;
            DataBase_ComboBox.SelectedItem = newSessionInfo.DataBaseName;
            UserName_TextBox.Text = newSessionInfo.UserName;
        }
        public LoginForm(SessionInfo session)
        {
            newSessionInfo = session;

            newSessionInfo.HostName = Environment.MachineName;
            newSessionInfo.UserName = Environment.UserName;
            newSessionInfo.UserFIO = new DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName).Properties["FullName"].Value.ToString();

            InitializeComponent();
            ServerName_TextBox.Text = newSessionInfo.ServerName;
            DataBase_ComboBox.ItemsSource = newSessionInfo.AvalibaleDataBaseList;
            DataBase_ComboBox.SelectedItem = newSessionInfo.DataBaseName;
            UserName_TextBox.Text = newSessionInfo.UserName;
        }


        private void DataBase_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newSessionInfo.DataBaseName = DataBase_ComboBox.SelectedItem.ToString();
        }

        private void ServerName_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            newSessionInfo.ServerName = ServerName_TextBox.Text;
            DataBase_ComboBox.ItemsSource = newSessionInfo.AvalibaleDataBaseList;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            newSessionInfo.HostName = Environment.MachineName;
            newSessionInfo.UserName = Environment.UserName;
            newSessionInfo.UserFIO = new DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName).Properties["FullName"].Value.ToString();

            newSessionInfo.ServerName = ServerName_TextBox.Text;
            newSessionInfo.DataBaseName = DataBase_ComboBox.SelectedItem.ToString();

            sessionInfo = newSessionInfo;

            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
