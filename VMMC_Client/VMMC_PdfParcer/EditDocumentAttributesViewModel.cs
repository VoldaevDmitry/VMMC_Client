using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace VMMC_PdfParcer
{
    public class EditDocumentAttributesViewModel : INotifyPropertyChanged
    {

        public VMMC_Core.SessionInfo sessionInfo;

        private VMMC_Core.CommonControls.DocumentViewModel documentViewModel;
        public VMMC_Core.CommonControls.DocumentViewModel DocumentViewModel
        {
            get { return documentViewModel; }
            set
            {
                documentViewModel = value;
                OnEditDocumentAttributesViewPropertyChanged("DocumentViewModel");
            }
        }

        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnEditDocumentAttributesViewPropertyChanged("DocumentsCollection");
            }
        }
        private Visibility editDocumentAttributesViewVisibility;
        public Visibility EditDocumentAttributesViewVisibility
        {
            get { return editDocumentAttributesViewVisibility; }
            set
            {
                editDocumentAttributesViewVisibility = value;
                OnEditDocumentAttributesViewPropertyChanged("EditDocumentAttributesViewVisibility");
            }
        }
        public EditDocumentAttributesViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;


        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnEditDocumentAttributesViewPropertyChanged([CallerMemberName] string prop = "")
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
