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


using NPOI.XSSF.UserModel;//apache 2.0

using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Threading;
using System.Data.SqlClient;
using System.Data.SqlTypes;


namespace VMMC_PdfParcer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
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
                this.DataContext = new VMMC_PdfParcer.PdfParcerViewModel(sessionInfo);
            }

        }

        public MainWindow()
        {
            sessionInfo = new VMMC_Core.SessionInfo();
            VMMC_Core.CommonControls.LoginForm loginForm = new VMMC_Core.CommonControls.LoginForm();
            loginForm.ShowDialog();
            sessionInfo = loginForm.sessionInfo;
            if (sessionInfo == null) this.Close();
            else
            {
                InitializeComponent();
                DataContext = new VMMC_PdfParcer.PdfParcerViewModel(sessionInfo);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {


            VMMC_PdfParcer.PdfParcerViewModel pdfParcerViewModel = (VMMC_PdfParcer.PdfParcerViewModel)this.DataContext;

            if (pdfParcerViewModel.IsPDFselected && pdfParcerViewModel.IsPDFparsed && pdfParcerViewModel.IsPDFsplited)
            {

                if (pdfParcerViewModel.HasParentWindow == true)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    System.Diagnostics.Process.Start("VMMC_ExcelParcer", sessionInfo.ServerName + " " + sessionInfo.DataBaseName);
                    System.Diagnostics.Process.Start("VMMC_Import", sessionInfo.ServerName + " " + sessionInfo.DataBaseName);
                    this.Close();
                }
            }
            else
            {
                pdfParcerViewModel.NextCommand();
            }
            //VMMC_Import.MainWindow importMainWindow = new VMMC_Import.MainWindow(sessionInfo);
        }

    }
}
