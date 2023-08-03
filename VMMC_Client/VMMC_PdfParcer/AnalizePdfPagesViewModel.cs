using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMMC_Core;

namespace VMMC_PdfParcer
{
    public class AnalizePdfPagesViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.SessionInfo sessionInfo;

        private string sourcePdfPath;
        public string SourcePdfPath
        {
            get { return sourcePdfPath; }
            set
            {
                sourcePdfPath = value;
                OnAnalizePdfPagesViewPropertyChanged("SourcePdfPath");
            }
        }

        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnAnalizePdfPagesViewPropertyChanged("DocumentsCollection");
            }
        }
        private Visibility analizePdfPagesViewVisibility;
        public Visibility AnalizePdfPagesViewVisibility
        {
            get { return analizePdfPagesViewVisibility; }
            set
            {
                analizePdfPagesViewVisibility = value;
                OnAnalizePdfPagesViewPropertyChanged("AnalizePdfPagesViewVisibility");
            }
        }
        public AnalizePdfPagesViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;


        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnAnalizePdfPagesViewPropertyChanged([CallerMemberName] string prop = "")
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
