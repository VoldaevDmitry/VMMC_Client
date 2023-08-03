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
using System.Threading;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace VMMC_Import
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
                ImportViewModel importViewModel = new ImportViewModel(sessionInfo);
                DataContext = importViewModel;
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
                ImportViewModel importViewModel = new ImportViewModel(sessionInfo);
                DataContext = importViewModel;
            }
        }

        //private void FileParser_MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    VMMC_FileParser.MainWindow fileParserMainWindow = new VMMC_FileParser.MainWindow(sessionInfo);
        //    ImportViewModel importViewModel = (ImportViewModel)this.DataContext;

        //    this.Visibility = Visibility.Hidden;
        //    var ttt = fileParserMainWindow.ShowDialog();

        //    if (ttt == true)
        //    {
        //        VMMC_FileParser.FileParserViewModel fileParserViewModel = (VMMC_FileParser.FileParserViewModel)fileParserMainWindow.DataContext;                

        //        importViewModel.ComplektCollection = fileParserViewModel.ComplektsCollection;
        //        importViewModel.DocumentsCollection = fileParserViewModel.DocumentsCollection;

        //        //importViewModel.RevisionsCollection = new ObservableCollection<VMMC_Core.Revision>();
        //        foreach (VMMC_Core.Document document in fileParserViewModel.DocumentsCollection)
        //        {
        //            foreach (VMMC_Core.Revision revision in document.Revisions)
        //            {
        //                importViewModel.RevisionsCollection.Add(revision);
        //            }
        //        }

        //        //importViewModel.FilesCollection = new ObservableCollection<VMMC_Core.Files>();
        //        foreach (VMMC_Core.LocalFile file in fileParserViewModel.LocalFilesCollection)
        //        {
        //            VMMC_Core.Document resDoc = fileParserViewModel.DocumentsCollection.Where(x => x.DocumentCode == file.Document.DocumentCode).FirstOrDefault();
                    
        //            VMMC_Core.Revision resRev = resDoc.Revisions.Where(x => x.DocumentId == resDoc.DocumentId && x.Number == file.Revision.Number).FirstOrDefault();

        //            VMMC_Core.Files newFile = new VMMC_Core.Files(sessionInfo)
        //            {
        //                FileGuid = Guid.NewGuid(),
        //                RevisionId = resRev.RevisionId,
        //                FileName = file.LocalFileName,
        //                FileType = file.LocalFileType,                        
        //                Checksum = file.Checksum,
        //                Status = file.Status,
        //                StatusInfo = file.StatusInfo
        //            };
        //            importViewModel.FilesCollection.Add(newFile);
        //        }

        //        importViewModel.RelationshipsCollection = fileParserViewModel.RelationshipsCollection;

        //        ComplektDataGrid.ItemsSource = importViewModel.ComplektCollection;
        //        DocumentDataGrid.ItemsSource = importViewModel.DocumentsCollection;
        //        RevisionDataGrid.ItemsSource = importViewModel.RevisionsCollection;
        //        RelationshipGrid.ItemsSource = importViewModel.RelationshipsCollection;
        //        FileDataGrid.ItemsSource = importViewModel.FilesCollection;
        //    }
        //    DataContext = importViewModel;
        //    CheckListAsync();
        //    this.ShowDialog();
        //}
        //private void ExcelParser_MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    VMMC_ExcelParcer.MainWindow excelParserMainWindow = new VMMC_ExcelParcer.MainWindow(sessionInfo);
        //    VMMC_ExcelParcer.ExcelParserViewModel excelParserViewModel = (VMMC_ExcelParcer.ExcelParserViewModel)excelParserMainWindow.DataContext;
        //    excelParserViewModel.HasParentWindow = true;

        //    this.Visibility = Visibility.Hidden;
        //    var ttt = excelParserMainWindow.ShowDialog();

        //    ImportViewModel importViewModel = (ImportViewModel)this.DataContext;
        //    if (ttt == true)
        //    {
        //        excelParserViewModel = (VMMC_ExcelParcer.ExcelParserViewModel)excelParserMainWindow.DataContext;                 
        //        importViewModel.DocumentsCollection = excelParserViewModel.DocumentsCollection;
        //        //importViewModel.RevisionsCollection = new ObservableCollection<VMMC_Core.Revision>();
        //        if (excelParserViewModel.DocumentsCollection != null)
        //        {
        //            foreach (VMMC_Core.Document document in excelParserViewModel.DocumentsCollection)
        //            {
        //                foreach (VMMC_Core.Revision revision in document.Revisions)
        //                {
        //                    importViewModel.RevisionsCollection.Add(revision);
        //                }
        //            }
        //        }
        //        importViewModel.RelationshipsCollection = excelParserViewModel.RelationshipsCollection;
        //        importViewModel.TagsCollection = excelParserViewModel.TagsCollection;

        //        ComplektDataGrid.ItemsSource = importViewModel.ComplektCollection;
        //        DocumentDataGrid.ItemsSource = importViewModel.DocumentsCollection;
        //        RevisionDataGrid.ItemsSource = importViewModel.RevisionsCollection;
        //        TagDataGrid.ItemsSource = importViewModel.TagsCollection;
        //        RelationshipGrid.ItemsSource = importViewModel.RelationshipsCollection;
        //        FileDataGrid.ItemsSource = importViewModel.FilesCollection;
        //    }

        //    DataContext = importViewModel;
        //    CheckListAsync();
        //    this.Visibility = Visibility.Visible;
        //}
        //private void DataBaseParser_MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //    VMMC_DataBaseParcer.MainWindow dataBaseParcerMainWindow = new VMMC_DataBaseParcer.MainWindow(sessionInfo);
        //    ImportViewModel importViewModel = (ImportViewModel)this.DataContext;

        //    this.Visibility = Visibility.Hidden;
        //    var ttt = dataBaseParcerMainWindow.ShowDialog();

        //    if (ttt == true)
        //    {
        //        VMMC_DataBaseParcer.DataBaseParcerViewModel dataBaseParcerViewModel = (VMMC_DataBaseParcer.DataBaseParcerViewModel)dataBaseParcerMainWindow.DataContext;
        //        List<VMMC_Core.CommonControls.AttributeObjectValueViewModel> AttributeObjectValueViewModelCollection = new List<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

        //        Guid projectId = Guid.NewGuid();
        //        importViewModel.DocumentsCollection = dataBaseParcerViewModel.DocumentsCollection;
        //        //importViewModel.RevisionsCollection = new ObservableCollection<VMMC_Core.Revision>();
        //        if (dataBaseParcerViewModel.DocumentsCollection != null)
        //        {
        //            foreach (VMMC_Core.Document document in dataBaseParcerViewModel.DocumentsCollection)
        //            {
        //                foreach (VMMC_Core.Revision revision in document.Revisions)
        //                {
        //                    importViewModel.RevisionsCollection.Add(revision);
        //                }
        //            }
        //        }
        //        if(dataBaseParcerViewModel.ComplektsCollection!=null) importViewModel.ComplektCollection = dataBaseParcerViewModel.ComplektsCollection;
        //        if (dataBaseParcerViewModel.DocumentsCollection != null) importViewModel.DocumentsCollection = dataBaseParcerViewModel.DocumentsCollection;
        //        if (dataBaseParcerViewModel.RelationshipsCollection != null) importViewModel.RelationshipsCollection = dataBaseParcerViewModel.RelationshipsCollection;
        //        if (dataBaseParcerViewModel.TagsCollection != null) importViewModel.TagsCollection = dataBaseParcerViewModel.TagsCollection;
        //        if (dataBaseParcerViewModel.TreeItemsCollection != null) importViewModel.TreeItemsCollection = dataBaseParcerViewModel.TreeItemsCollection;
        //        if (dataBaseParcerViewModel.AttributeObjectValuesCollection != null)
        //        {
        //            importViewModel.AttributeObjectValuesCollection = dataBaseParcerViewModel.AttributeObjectValuesCollection;

        //            foreach (VMMC_Core.AttributeObjectValue aov in importViewModel.AttributeObjectValuesCollection)
        //            {
        //                AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel() { AttributeObjectValue = aov });
        //            }
        //        }

        //        ComplektDataGrid.ItemsSource = importViewModel.ComplektCollection;
        //        DocumentDataGrid.ItemsSource = importViewModel.DocumentsCollection;
        //        RevisionDataGrid.ItemsSource = importViewModel.RevisionsCollection;
        //        TagDataGrid.ItemsSource = importViewModel.TagsCollection;
        //        RelationshipGrid.ItemsSource = importViewModel.RelationshipsCollection;
        //        FileDataGrid.ItemsSource = importViewModel.FilesCollection;

        //        AttributesObjectValueGrid.ItemsSource = AttributeObjectValueViewModelCollection;

                
        //    }

        //    DataContext = importViewModel;
        //    CheckListAsync();
        //    this.Visibility = Visibility.Visible;
        //    //this.Close();
        //}


        //private async void StartCheckListAsync()
        //{
        //    ImportViewModel importViewModel = (ImportViewModel)this.DataContext;
        //    CancellationTokenSource tokenSource = new CancellationTokenSource();
        //    Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { ShowCheckListProceessText(tokenSource.Token); }));

        //    importViewModel.CheckListAsync();

        //    tokenSource.Cancel();
        //    DataContext = importViewModel;
        //}
        //private async void ShowCheckListProceessText(CancellationToken token)
        //{
        //    txtLoading.Visibility = Visibility.Visible;

        //    try
        //    {
        //        while (!token.IsCancellationRequested)
        //        {
        //            txtLoading.Content = "Выполняется проверка списка документов";
        //            await Task.Delay(500, token);
        //            txtLoading.Content = "Выполняется проверка списка документов.";
        //            await Task.Delay(500, token);
        //            txtLoading.Content = "Выполняется проверка списка документов..";
        //            await Task.Delay(500, token);
        //            txtLoading.Content = "Выполняется проверка списка документов...";
        //            await Task.Delay(500, token);
        //        }
        //    }
        //    catch (TaskCanceledException)
        //    {
        //        txtLoading.Visibility = Visibility.Hidden;
        //        //FillChecksumAsync();
        //    }
        //}


        //private void Upload_Button_Click(object sender, RoutedEventArgs e)
        //{
        //    StartImportAsync();
        //}
        //private async void StartImportAsync()
        //{
        //    ImportViewModel importViewModel = (ImportViewModel)this.DataContext;
        //    CancellationTokenSource tokenSource = new CancellationTokenSource();
        //    Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate { ShowImportProceessText(tokenSource.Token); }));

        //    if ((bool)IsComplektsUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportComplekts());
        //    }
        //    if ((bool)IsDocumentsUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportDocuments());
        //    }
        //    if ((bool)IsRevisionsUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportRevisions());
        //    }
        //    if ((bool)IsFilesUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportFiles());
        //    }
        //    if ((bool)IsTagsUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportTags());
        //    }
        //    if ((bool)IsRelationshipsUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportRelationships());
        //    }
        //    if ((bool)IsTreeItemsUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportTreeItems());
        //    }
        //    if ((bool)IsAttributesUpload_CheckBox.IsChecked)
        //    {
        //        await Task.Run(() => importViewModel.ImportAttributes());
        //    }

        //    tokenSource.Cancel();
                        
        //    DataContext = importViewModel;
        //}
        //private async void ShowImportProceessText(CancellationToken token)
        //{
        //    txtLoading.Visibility = Visibility.Visible;

        //    try
        //    {
        //        while (!token.IsCancellationRequested)
        //        {
        //            txtLoading.Content = "Выполняется загрузка";
        //            await Task.Delay(500, token);
        //            txtLoading.Content = "Выполняется загрузка.";
        //            await Task.Delay(500, token);
        //            txtLoading.Content = "Выполняется загрузка..";
        //            await Task.Delay(500, token);
        //            txtLoading.Content = "Выполняется загрузка...";
        //            await Task.Delay(500, token);
        //        }
        //    }
        //    catch (TaskCanceledException)
        //    {
        //        txtLoading.Visibility = Visibility.Hidden;
        //        txtLoading.Content = "Загрузка завершена.";
        //        //FillChecksumAsync();
        //    }
        //}
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
