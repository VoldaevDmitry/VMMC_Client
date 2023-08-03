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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using VMMC_Client;
using VMMC_Core;
using VMMC_Editor;
using System.DirectoryServices;


namespace VMMC_Login
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SessionInfo sessionInfo;


        //List<OrganisationRolesInfo> OrganisationRolesInfoCollection { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            
            sessionInfo = new SessionInfo();
            sessionInfo.HostName = Environment.MachineName;
            sessionInfo.UserName = Environment.UserName;
            sessionInfo.UserFIO = new DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName).Properties["FullName"].Value.ToString();
            //DirectoryEntry de = new DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName);
            //de.Properties["FullName"].Value.ToString();

            sessionInfo.ServerName = "server-db";
            ServerName_TextBox.Text = sessionInfo.ServerName;
            DataBase_ComboBox.ItemsSource = sessionInfo.AvalibaleDataBaseList;
            DataBase_ComboBox.SelectedItem = "InfoModelVMMK";
            UserName_TextBox.Text = sessionInfo.UserName;
        }

        private void ShowClient_Button_Click(object sender, RoutedEventArgs e)
        {

            VMMC_Client.MainWindow clientMainWindow = new VMMC_Client.MainWindow(sessionInfo);
            
            //editorMainWindow.ViewModel = "ViewModel";

            this.Visibility = Visibility.Collapsed;
           var ttt = clientMainWindow.ShowDialog();
            if (ttt == false) this.Close();
            else this.Visibility = Visibility.Visible;


        }

        private void ShowImport_Button_Click(object sender, RoutedEventArgs e)
        {
            VMMC_Import.MainWindow importMainWindow = new VMMC_Import.MainWindow(sessionInfo);
            //editorMainWindow.ViewModel = "ViewModel";

            this.Visibility = Visibility.Collapsed;
            var ttt = importMainWindow.ShowDialog();
            if (ttt == false) this.Close();
            else this.Visibility = Visibility.Visible;


            //this.Close();

        }

        private void ShowEditor_Button_Click(object sender, RoutedEventArgs e)
        {
            VMMC_Editor.MainWindow editorMainWindow = new VMMC_Editor.MainWindow(sessionInfo);
            //editorMainWindow.ViewModel = "ViewModel";

            this.Visibility = Visibility.Collapsed;
            var ttt = editorMainWindow.ShowDialog();
            if (ttt == false) this.Close();
            else this.Visibility = Visibility.Visible;


            //this.Close();

        }

        private void DataBase_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sessionInfo.DataBaseName = DataBase_ComboBox.SelectedItem.ToString();
        }

        private void ServerName_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            sessionInfo.ServerName = ServerName_TextBox.Text;
            DataBase_ComboBox.ItemsSource = sessionInfo.AvalibaleDataBaseList;
        }
    }
}
