using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace VMMC_PdfParcer
{
    /// <summary>
    /// Логика взаимодействия для AnalizePdfPagesView.xaml
    /// </summary>
    public partial class AnalizePdfPagesView : UserControl
    {
        public AnalizePdfPagesView()
        {
            InitializeComponent();
        }


        private void documentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            VMMC_PdfParcer.AnalizePdfPagesViewModel analizePdfPagesViewModel = (VMMC_PdfParcer.AnalizePdfPagesViewModel)this.DataContext;

            if (analizePdfPagesViewModel.SourcePdfPath != null)
            {
                try
                {
                    FileInfo fileInfo = new FileInfo(analizePdfPagesViewModel.SourcePdfPath);
                    if (fileInfo.Exists)
                    {
                        VMMC_Core.Document doc = (VMMC_Core.Document)documentDataGrid.SelectedItems[0];
                        string firstPage = doc.StatusInfo.Split('-')[0];
                        string ttt = "file:///" + analizePdfPagesViewModel.SourcePdfPath.Replace("\\", "/") + "#page=" + firstPage;


                        pdfBrowser.Source = new Uri("file:///" + analizePdfPagesViewModel.SourcePdfPath.Replace("\\", "/") + "#page=" + firstPage);
                        //pdfBrowser.Navigate(new Uri("file:///" + analizePdfPagesViewModel.SourcePdfPath.Replace("\\", "/") + "#page=" + firstPage));
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
    }
}
