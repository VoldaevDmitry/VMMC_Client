using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VMMC_Core;

namespace VMMC_PdfParcer
{
    public class SelectSourcePdfViewModel : INotifyPropertyChanged
    {

        public VMMC_Core.SessionInfo sessionInfo;

        private VMMC_Core.RelayCommand addComplektRDCommand;
        public VMMC_Core.RelayCommand AddComplektRDCommand
        {
            get
            {
                return addComplektRDCommand ??
                  (addComplektRDCommand = new RelayCommand(obj => { AddComplektRD(); }));
            }
        }

        private VMMC_Core.RelayCommand getSourcePdfCommand;
        public VMMC_Core.RelayCommand GetSourcePdfCommand
        {
            get
            {
                return getSourcePdfCommand ??
                  (getSourcePdfCommand = new RelayCommand(obj => { GetSourcePdf(); }));
            }
        }

        private string sourcePdfPath;
        public string SourcePdfPath
        {
            get { return sourcePdfPath; }
            set
            {
                sourcePdfPath = value;
                OnSelectSourcePdfViewPropertyChanged("SourcePdfPath");
            }
        }

        private Visibility selectSourcePdfViewVisibility;
        public Visibility SelectSourcePdfViewVisibility
        {
            get { return selectSourcePdfViewVisibility; }
            set
            {
                selectSourcePdfViewVisibility = value;
                OnSelectSourcePdfViewPropertyChanged("SelectSourcePdfViewVisibility");
            }
        }

        private VMMC_Core.Complekt complektID;
        public VMMC_Core.Complekt ComplektID
        {
            get { return complektID; }
            set
            {
                complektID = value;
                OnSelectSourcePdfViewPropertyChanged("ComplektID");
            }
        }
        private VMMC_Core.Complekt complektRD;
        public VMMC_Core.Complekt ComplektRD
        {
            get { return complektRD; }
            set
            {
                complektRD = value;
                OnSelectSourcePdfViewPropertyChanged("ComplektRD");
            }
        }
        private ObservableCollection<VMMC_Core.Complekt> complektsRDCollection;
        public ObservableCollection<VMMC_Core.Complekt> ComplektsRDCollection
        {
            get { return complektsRDCollection; }
            set
            {
                complektsRDCollection = value;
                OnSelectSourcePdfViewPropertyChanged("ComplektsRDCollection");
            }
        }

        public SelectSourcePdfViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;


        }


        private void GetSourcePdf()
        {
            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.Filter = "(*.pdf)|*.PDF";
                dialog.ShowDialog();

                if (dialog.FileNames != null)
                {
                    foreach (string fileName in dialog.FileNames)
                    {
                        FileInfo fileInfo = new FileInfo(fileName);
                        if (fileInfo.Exists)
                        {
                            SourcePdfPath = fileName;
                        }   
                    }
                }
            }
        }
        private void AddComplektRD()
        {
            VMMC_Core.Complekt complektRD = new Complekt(sessionInfo);
            complektRD.ComplektCode = ComplektRD.ComplektCode;
            ComplektsRDCollection.Add( complektRD );
            ComplektRD.ComplektCode = string.Empty;
            OnSelectSourcePdfViewPropertyChanged("ComplektRD");
            OnSelectSourcePdfViewPropertyChanged("ComplektsRDCollection");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnSelectSourcePdfViewPropertyChanged([CallerMemberName] string prop = "")
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}


