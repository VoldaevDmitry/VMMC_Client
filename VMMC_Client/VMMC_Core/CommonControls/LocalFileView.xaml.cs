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

namespace VMMC_Core.CommonControls
{
    /// <summary>
    /// Логика взаимодействия для LocalFileView.xaml
    /// </summary>
    public partial class LocalFileView : UserControl
    {
        private VMMC_Core.SessionInfo sessionInfo;
        public LocalFileView()
        {
            InitializeComponent();

            //sessionInfo = session;

            this.DataContext = new VMMC_Core.CommonControls.LocalFileViewModel(null, false, sessionInfo);

            this.DataContextChanged += new DependencyPropertyChangedEventHandler(LocalFileViewDataContextChanged);
        }
        private void LocalFileViewDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) 
        {
            if (this.DataContext != null)
            {
                VMMC_Core.CommonControls.LocalFileViewModel LocalFileViewDataContext = (VMMC_Core.CommonControls.LocalFileViewModel)this.DataContext;

                if (LocalFileViewDataContext.LocalFile != null)
                {
                    VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(LocalFileViewDataContext.LocalFile.Document, LocalFileViewDataContext.LocalFile.Revision, null, !LocalFileViewDataContext.LocalFile.Document.IsExistInDB, LocalFileViewDataContext.LocalFile.Document.sessionInfo);
                    DocumentViewControl.DataContext = DocumentViewControl_DataContext;
                }
            }

        }
    }
}
