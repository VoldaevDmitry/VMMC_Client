using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module.Model
{
    public class IportDataSet : INotifyPropertyChanged
    {

        private ObservableCollection<Core_Module.Complekt> complektsCollection;
        public ObservableCollection<Core_Module.Complekt> ComplektCollection
        {
            get { return complektsCollection; }
            set
            {
                complektsCollection = value;
                OnIportDataSetPropertyChanged("ComplektCollection");
            }
        }


        private ObservableCollection<Core_Module.Document> documentsCollection;
        public ObservableCollection<Core_Module.Document> DocumentsCollection
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

        public ObservableCollection<Core_Module.Revision> RevisionsCollection
        {
            get { return getRevisionCollection(); }
        }
        private ObservableCollection<Core_Module.Revision> getRevisionCollection()
        {
            if (DocumentsCollection != null)
            {
                ObservableCollection<Core_Module.Revision> resultCollection = new ObservableCollection<Revision>();
                foreach (var doc in DocumentsCollection)
                {
                    foreach (Core_Module.Revision rev in doc.Revisions)
                    {
                        resultCollection.Add(rev);
                    }
                }

                return resultCollection;
            }
            else return null;

        }

        public ObservableCollection<Core_Module.Files> FilesCollection
        {
            get { return getFilesCollection(); }
        }
        private ObservableCollection<Core_Module.Files> getFilesCollection()
        {
            if (DocumentsCollection != null)
            {
                ObservableCollection<Core_Module.Files> resultCollection = new ObservableCollection<Core_Module.Files>();
                foreach (var doc in DocumentsCollection)
                {
                    if (doc.Revisions != null)
                    {
                        foreach (Core_Module.Revision rev in doc.Revisions)
                        {
                            if (rev.Files != null)
                            {
                                foreach (Core_Module.Files file in rev.Files)
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


        private ObservableCollection<Core_Module.Tag> tagsCollection;
        public ObservableCollection<Core_Module.Tag> TagsCollection
        {
            get { return tagsCollection; }
            set
            {
                tagsCollection = value;
                OnIportDataSetPropertyChanged("TagsCollection");
            }
        }


        private ObservableCollection<Core_Module.TreeItem> treeItemsCollection;
        public ObservableCollection<Core_Module.TreeItem> TreeItemsCollection
        {
            get { return treeItemsCollection; }
            set
            {
                treeItemsCollection = value;
                OnIportDataSetPropertyChanged("TreeItemsCollection");
            }
        }


        private ObservableCollection<Core_Module.Relationship> relationshipsCollection;
        public ObservableCollection<Core_Module.Relationship> RelationshipsCollection
        {
            get { return relationshipsCollection; }
            set
            {
                relationshipsCollection = value;
                OnIportDataSetPropertyChanged("RelationshipsCollection");
            }
        }


        private ObservableCollection<Core_Module.AttributeObjectValue> attributeObjectValuesCollection;
        public ObservableCollection<Core_Module.AttributeObjectValue> AttributeObjectValuesCollection
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
