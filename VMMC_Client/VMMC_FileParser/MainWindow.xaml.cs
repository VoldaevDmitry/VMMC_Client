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

namespace VMMC_FileParser
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
                this.DataContext = new VMMC_FileParser.FileParserViewModel(sessionInfo, "");
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
                DataContext = new VMMC_FileParser.FileParserViewModel(sessionInfo, "");
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {

            FileParserViewModel fileParserViewModel = (FileParserViewModel)DataContext;
            if (fileParserViewModel.HasParentWindow == true)
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
        private async void SelectFolder_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                FileParserViewModel fileParserViewModel = (FileParserViewModel)DataContext;
                dialog.ShowDialog();
                fileParserViewModel = new VMMC_FileParser.FileParserViewModel(sessionInfo, dialog.SelectedPath);

                //fileParserViewModel.DocumentsCollection = null;
                //fileParserViewModel.ComplektsCollection = null;
                //fileParserViewModel.TagsCollection = null;
                //fileParserViewModel.TreeItemsCollection = null;
                //fileParserViewModel.RelationshipsCollection = null;

                DataContext = fileParserViewModel;
                SortListAsync();

                //CheckListAsync();
                //CancellationTokenSource tokenSource = new CancellationTokenSource();
                //Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { ShowLoadingText(tokenSource.Token); }));

                //await Task.Run(() => FillChecksum(fileParserViewModel.FilesCollection)); // your parsing operation

                //tokenSource.Cancel();               
                
            }
        }

        private async void SortListAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { ShowSortListProceessText(tokenSource.Token); }));

            //await Task.Run(() => Thread.Sleep(5000)); // your parsing operation
            FileParserViewModel fileParserViewModel = (FileParserViewModel)DataContext;
            await Task.Run(() => fileParserViewModel.SortLocalFiles());
            tokenSource.Cancel();
            FillChecksumAsync();
        }
        private async void ShowSortListProceessText(CancellationToken token)
        {
            txtLoading.Visibility = Visibility.Visible;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    txtLoading.Content = "Выполняется проверка списка документов";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется проверка списка документов.";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется проверка списка документов..";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется проверка списка документов...";
                    await Task.Delay(500, token);
                }
            }
            catch (TaskCanceledException)
            {
                txtLoading.Visibility = Visibility.Hidden;
                //FillChecksumAsync();
            }
        }
        private async void FillChecksumAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { ShowCalculatingProceessText(tokenSource.Token); }));

            //await Task.Run(() => Thread.Sleep(5000)); // your parsing operation
            FileParserViewModel fileParserViewModel = (FileParserViewModel)DataContext;
            await Task.Run(() => fileParserViewModel.FillLocalFilesChecksum());

            tokenSource.Cancel();
            CheckListAsync();
        }
        private async void ShowCalculatingProceessText(CancellationToken token)
        {
            txtLoading.Visibility = Visibility.Visible;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    txtLoading.Content = "Выполняется вычисление контрольных сумм";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется вычисление контрольных сумм.";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется вычисление контрольных сумм..";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется вычисление контрольных сумм...";
                    await Task.Delay(500, token);
                }
            }
            catch (TaskCanceledException)
            {
                txtLoading.Visibility = Visibility.Hidden;
                //CheckListAsync();
            }
        }
        
        private async void CheckListAsync()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { ShowCheckListText(tokenSource.Token); }));

            //await Task.Run(() => Thread.Sleep(5000)); // your parsing operation
            FileParserViewModel fileParserViewModel = (FileParserViewModel)DataContext;
            await Task.Run(() => fileParserViewModel.CheckLocalFilesList());

            tokenSource.Cancel();
        }
        private async void ShowCheckListText(CancellationToken token)
        {
            txtLoading.Visibility = Visibility.Visible;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    txtLoading.Content = "Выполняется проверка списка";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется проверка списка.";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется проверка списка..";
                    await Task.Delay(500, token);
                    txtLoading.Content = "Выполняется проверка списка...";
                    await Task.Delay(500, token);
                }
            }
            catch (TaskCanceledException)
            {
                txtLoading.Visibility = Visibility.Hidden;                
            }
        }
        
        private void ExportToExcel_MenuItem_Click(object sender, RoutedEventArgs e) 
        {
            FileParserViewModel fileParserViewModel = (FileParserViewModel)DataContext;
            new ExportTo().ExportToExcel(fileParserViewModel.LocalFilesCollection, fileParserViewModel.folderPath);
        }        
        private void documetnsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VMMC_Core.LocalFile selectedItem = (VMMC_Core.LocalFile)documetnsGrid.SelectedItem;
            VMMC_Core.CommonControls.LocalFileViewModel LocalFileViewDataContext = new VMMC_Core.CommonControls.LocalFileViewModel(selectedItem, true, sessionInfo);
            LocalFileView.DataContext = LocalFileViewDataContext;
        }
    }
}

