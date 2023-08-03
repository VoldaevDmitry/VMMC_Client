using NPOI.POIFS.FileSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.DirectoryServices;




namespace VMMC_ExcelParcer
{
    public partial class MainWindow : Window
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public MainWindow(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
            if (sessionInfo == null) this.Close();
            else
            {
                InitializeComponent();
                DataContext = new VMMC_ExcelParcer.ExcelParserViewModel(sessionInfo);
            }
        }
        public MainWindow()
        {
            sessionInfo = new VMMC_Core.SessionInfo();
            
            String[] args = App.Args;
            if (args != null)
            {
                sessionInfo.ServerName = args[0];
                sessionInfo.DataBaseName = args[1];
                sessionInfo.HostName = Environment.MachineName;
                sessionInfo.UserName = Environment.UserName;
                sessionInfo.UserFIO = new System.DirectoryServices.DirectoryEntry("WinNT://" + Environment.UserDomainName + "/" + Environment.UserName).Properties["FullName"].Value.ToString();

            }

            if (sessionInfo.DataBaseName == null)
            {
                VMMC_Core.CommonControls.LoginForm loginForm = new VMMC_Core.CommonControls.LoginForm();
                loginForm.ShowDialog();
                sessionInfo = loginForm.sessionInfo;
            }

            if (sessionInfo == null) this.Close();
            else
            {
                InitializeComponent();
                DataContext = new VMMC_ExcelParcer.ExcelParserViewModel(sessionInfo);
            }
        }


        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            ExcelParserViewModel excelParserViewModel = (ExcelParserViewModel)DataContext;
            if ( excelParserViewModel.HasParentWindow == true)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                System.Diagnostics.Process.Start("VMMC_ExcelParcer", sessionInfo.ServerName + " "+sessionInfo.DataBaseName);
                System.Diagnostics.Process.Start("VMMC_Import", sessionInfo.ServerName + " "+sessionInfo.DataBaseName);
                this.Close();
            }
            
        }
        private void documetnsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VMMC_Core.Document selectedItem = (VMMC_Core.Document)Documents_DataGrid.SelectedItem;
            if (selectedItem != null)
            {
                VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext;


                if (selectedItem.Revisions.Count > 0) DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(selectedItem, selectedItem.Revisions[0], null, !selectedItem.IsExistInDB, sessionInfo);
                else DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(selectedItem, null, null, !selectedItem.IsExistInDB, sessionInfo);

                DocumentViewControl.DataContext = DocumentViewControl_DataContext;
            }
        }
    }
}
