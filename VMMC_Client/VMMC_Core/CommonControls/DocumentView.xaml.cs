using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

namespace VMMC_Core.CommonControls
{
    /// <summary>
    /// Логика взаимодействия для DocumentView.xaml
    /// </summary>
    public partial class DocumentView : UserControl
    {
        private VMMC_Core.CommonControls.DocumentViewModel BufferedDocumentViewDataContext;
        public DocumentView()
        {
            InitializeComponent();
            this.DataContext = new VMMC_Core.CommonControls.DocumentViewModel(null, null, null, false, null);
            this.DataContextChanged += new DependencyPropertyChangedEventHandler(DocumentViewDataContextChanged);

            //VMMC_Core.CommonControls.DocumentViewModel documentViewModel = (VMMC_Core.CommonControls.DocumentViewModel)this.DataContext;
            //VMMC_Core.CommonControls.AttributeViewModel attributeViewControl_DataContext = new VMMC_Core.CommonControls.AttributeViewModel(documentViewModel.AttributeObjectValueList);
            //AttributeViewControl.DataContext = attributeViewControl_DataContext;

            //this.DataContextChanged += new DependencyPropertyChangedEventHandler(DocumentViewDataContextChanged);


        }
        private void DocumentViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                //VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(LocalFileViewDataContext.LocalFile.Document, true);
                //this.DataContext = DocumentViewControl_DataContext;

                VMMC_Core.CommonControls.DocumentViewModel documentViewDataContext = (VMMC_Core.CommonControls.DocumentViewModel)this.DataContext;

                if (documentViewDataContext.Document != null)
                {
                    VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(documentViewDataContext.Document, documentViewDataContext.Revision, documentViewDataContext.FilesList, documentViewDataContext.IsDocumentEditMode, documentViewDataContext.Document.sessionInfo);
                    //this.DataContext = DocumentViewControl_DataContext;
                }
            }

        }

        private void LocalFileViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.DataContext != null)
            {
                VMMC_Core.CommonControls.DocumentViewModel documentViewDataContext = (VMMC_Core.CommonControls.DocumentViewModel)this.DataContext;

                if (documentViewDataContext.Document != null)
                {
                    VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(documentViewDataContext.Document, documentViewDataContext.Revision, null, documentViewDataContext.IsDocumentEditMode, documentViewDataContext.Document.sessionInfo);
                    this.DataContext = DocumentViewControl_DataContext;
                }
            }

        }
        private void DocumentCode_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //DocumentViewModel DocumentViewDataContext = (DocumentViewModel)this.DataContext;
            //DocumentViewDataContext.Document.DocumentCode = DocumentCode_TextBox.Text;
            //this.DataContext = DocumentViewDataContext;

        }

        private void DocumentRevision_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VMMC_Core.CommonControls.DocumentViewModel documentViewModelDataContext = (DocumentViewModel)this.DataContext;
            //documentViewModelDataContext.Revision = (Revision)DocumentRevision_ComboBox.SelectedItem;
            if (documentViewModelDataContext.Revision != null) documentViewModelDataContext.FilesList = new VMMC_Core.Files(documentViewModelDataContext.Revision.sessionInfo).GetFilesByRevision(documentViewModelDataContext.Revision.RevisionId);
            this.DataContext = documentViewModelDataContext;
            DocumentFiles_DataGrid.ItemsSource = documentViewModelDataContext.FilesList;
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {

            DocumentViewModel documentViewDataContext = (DocumentViewModel)this.DataContext;

            VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(documentViewDataContext.Document, documentViewDataContext.Revision, null, documentViewDataContext.IsDocumentEditMode, documentViewDataContext.Document.sessionInfo);
            BufferedDocumentViewDataContext = new VMMC_Core.CommonControls.DocumentViewModel(documentViewDataContext.Document, documentViewDataContext.Revision, null, documentViewDataContext.IsDocumentEditMode, documentViewDataContext.Document.sessionInfo);

            DocumentViewControl_DataContext.IsDocumentEditMode = true;
            DocumentViewControl_DataContext.IsRevisionEditMode = true;

            this.DataContext = DocumentViewControl_DataContext;

            //this.DataContext = documentViewDataContext;

            //DocumentName_TextBox.IsEnabled = true;
            //DocumentCode_TextBox.IsEnabled = true;
            //DocumentClass_Combobox.IsEnabled = true;


        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {            
            BufferedDocumentViewDataContext.IsDocumentEditMode = false;
            BufferedDocumentViewDataContext.IsRevisionEditMode = false;

            this.DataContext = BufferedDocumentViewDataContext;

        }

        private void DocumentFiles_DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid t = (DataGrid)e.Source;
            VMMC_Core.Files selectedFile = (VMMC_Core.Files)(t.SelectedItem);
            DocumentViewModel documentViewDataContext = (DocumentViewModel)this.DataContext;
            string filePath = @"C:\temp\FileParserCach\OpenedFiles\temp\"+ selectedFile.FileName;
            documentViewDataContext.OpenFileEvent(selectedFile, filePath);
            //System.Diagnostics.Process.Start("VMMC_ExcelParcer", sessionInfo.ServerName + " " + sessionInfo.DataBaseName);
            //System.Diagnostics.Process.Start("VMMC_Import", sessionInfo.ServerName + " " + sessionInfo.DataBaseName);
            //this.Close();

           


            //var result = MessageBox.Show("Сохранить открываемый файл?", "Сохранить открываемый файл?").;
            //if (result == true)
            //{
            //    using (SaveFileDialog saveDialog = new SaveFileDialog())
            //    {
            //        saveDialog.FileName = fileName;

            //        if (saveDialog.ShowDialog() == DialogResult.OK)
            //        {
            //            if (saveDialog.FileName != null)
            //            {
            //                if (saveDialog.FileName.IndexOf(fileType) > 0) filePath = saveDialog.FileName;
            //                else filePath = saveDialog.FileName + fileType;

            //                if (ReadFileStream(fileId, filePath)) System.Diagnostics.Process.Start(filePath);
                            
            //            }
            //        }
            //    }
            //}
            //else if (result == DialogResult.No)
            //{
            //    if (fileName.IndexOf(fileType) > 0) filePath = @"C:\temp\FileParserCache\OpenedFiles\" + fileName;
            //    else filePath = @"C:\temp\FileParserCach\OpenedFiles\" + fileName + fileType;

            //    if (ReadFileStream(fileId, filePath)) System.Diagnostics.Process.Start(filePath);
            //    //if (ReadFileStream2(builder, fileId, filePath)) System.Diagnostics.Process.Start(filePath);
            //}
        }
    }
}
