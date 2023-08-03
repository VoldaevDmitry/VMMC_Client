using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core.Model
{
    public class IportDataSet : INotifyPropertyChanged
    {

        private ObservableCollection<VMMC_Core.Complekt> complektsCollection;
        public ObservableCollection<VMMC_Core.Complekt> ComplektCollection
        {
            get { return complektsCollection; }
            set
            {
                complektsCollection = value;
                OnIportDataSetPropertyChanged("ComplektCollection");
            }
        }


        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnIportDataSetPropertyChanged("DocumentsCollection");
                OnIportDataSetPropertyChanged("RevisionsCollection");
                DocumentsCollection.CollectionChanged += OnDocumentsCollectionChanged;
            }
        }
        private void OnDocumentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnIportDataSetPropertyChanged("DocumentsCollection");
            OnIportDataSetPropertyChanged("RevisionsCollection");
            OnIportDataSetPropertyChanged("FilesCollection");
        }

        public ObservableCollection<VMMC_Core.Revision> RevisionsCollection
        {
            get { return getRevisionCollection(); }
        }
        private ObservableCollection<VMMC_Core.Revision> getRevisionCollection()
        {
            if (DocumentsCollection != null)
            {
                ObservableCollection<VMMC_Core.Revision> resultCollection = new ObservableCollection<Revision>();
                foreach (var doc in DocumentsCollection)
                {
                    foreach (VMMC_Core.Revision rev in doc.Revisions)
                    {
                        resultCollection.Add(rev);
                    }
                }

                return resultCollection;
            }
            else return null;

        }

        public ObservableCollection<VMMC_Core.Files> FilesCollection
        {
            get { return getFilesCollection(); }
        }
        private ObservableCollection<VMMC_Core.Files> getFilesCollection()
        {
            if (DocumentsCollection != null)
            {
                ObservableCollection<VMMC_Core.Files> resultCollection = new ObservableCollection<VMMC_Core.Files>();
                foreach (var doc in DocumentsCollection)
                {
                    if (doc.Revisions != null)
                    {
                        foreach (VMMC_Core.Revision rev in doc.Revisions)
                        {
                            if (rev.Files != null)
                            {
                                foreach (VMMC_Core.Files file in rev.Files)
                                {
                                    resultCollection.Add(file);
                                }
                            }
                        }
                    }
                }

                return resultCollection;
            }
            else return null;

        }


        private ObservableCollection<VMMC_Core.Tag> tagsCollection;
        public ObservableCollection<VMMC_Core.Tag> TagsCollection
        {
            get { return tagsCollection; }
            set
            {
                tagsCollection = value;
                OnIportDataSetPropertyChanged("TagsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.TreeItem> treeItemsCollection;
        public ObservableCollection<VMMC_Core.TreeItem> TreeItemsCollection
        {
            get { return treeItemsCollection; }
            set
            {
                treeItemsCollection = value;
                OnIportDataSetPropertyChanged("TreeItemsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.Relationship> relationshipsCollection;
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsCollection
        {
            get { return relationshipsCollection; }
            set
            {
                relationshipsCollection = value;
                OnIportDataSetPropertyChanged("RelationshipsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesCollection;
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValuesCollection
        {
            get { return attributeObjectValuesCollection; }
            set
            {
                attributeObjectValuesCollection = value;
                OnIportDataSetPropertyChanged("AttributeObjectValuesCollection");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;

        public void OnIportDataSetPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
