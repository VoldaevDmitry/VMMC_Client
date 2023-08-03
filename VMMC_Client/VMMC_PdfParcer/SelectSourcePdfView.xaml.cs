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

namespace VMMC_PdfParcer
{
    /// <summary>
    /// Логика взаимодействия для SelectSourcePdfView.xaml
    /// </summary>
    public partial class SelectSourcePdfView : UserControl
    {
        public SelectSourcePdfView()
        {
            InitializeComponent();
        }

        private void pdfPath_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(pdfPath.Text);
                if (fileInfo.Exists)
                {
                    pdfBrowser.Navigate(new Uri(pdfPath.Text));
                }
            }
            catch 
            {

            }
        }

    }
}
