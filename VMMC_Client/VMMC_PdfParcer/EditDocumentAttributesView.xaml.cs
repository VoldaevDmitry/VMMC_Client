using System;
using System.Collections.Generic;
using System.IO;
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
using VMMC_Core.CommonControls;
using VMMC_Core;

namespace VMMC_PdfParcer
{
    /// <summary>
    /// Логика взаимодействия для EditDocumentAttributesView.xaml
    /// </summary>
    public partial class EditDocumentAttributesView : UserControl
    {
        public EditDocumentAttributesView()
        {
            InitializeComponent();
        }
        private void documentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            VMMC_PdfParcer.EditDocumentAttributesViewModel editDocumentAttributesViewModel = (VMMC_PdfParcer.EditDocumentAttributesViewModel)this.DataContext;
            VMMC_Core.Document doc = (VMMC_Core.Document)documentDataGrid.SelectedItems[0];

            if (doc.Revisions[0].Files[0].LocalPath != null)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(doc.Revisions[0].Files[0].LocalPath);
                    if (fileInfo.Exists)
                    {
                        pdfBrowser.Source = new Uri(doc.Revisions[0].Files[0].LocalPath);
                        documentDataGrid.Focus();
                    }
                }
                catch
                {

                }
            }
            else
            {

            }
        }

        private void documentDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VMMC_PdfParcer.EditDocumentAttributesViewModel editDocumentAttributesViewModel = (VMMC_PdfParcer.EditDocumentAttributesViewModel)this.DataContext;

            VMMC_Core.Document selectedDocument = (VMMC_Core.Document)documentDataGrid.SelectedItem;
            if (selectedDocument != null)
            {                
                if (selectedDocument.Revisions == null) selectedDocument.Revisions = new VMMC_Core.Revision(selectedDocument.sessionInfo).GetDbDocumentRevisionsList(selectedDocument.DocumentId);

                editDocumentAttributesViewModel.DocumentViewModel = new VMMC_Core.CommonControls.DocumentViewModel(selectedDocument, selectedDocument.Revisions[0], selectedDocument.Revisions[0].Files, !selectedDocument.IsExistInDB, selectedDocument.sessionInfo);
                
            }

        }
    }
}
