using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace VMMC_Core.CommonControls
{
    public class DocumentViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public bool IsDocumentEditMode { get; set; }
        public bool IsRevisionEditMode { get; set; }

        public bool EditMode { get; set; }



        private VMMC_Core.Document document;
        public VMMC_Core.Document Document
        {
            get { return document; }
            set
            {
                document = value;
                OnDocumentViewModelPropertyChanged("Document");
            }
        }
        private ObservableCollection<VMMC_Core.Class> avalibleDocumentClasses;
        public ObservableCollection<VMMC_Core.Class> AvalibleDocumentClasses
        {
            get { return avalibleDocumentClasses; }
            set
            {
                avalibleDocumentClasses = value;
                OnDocumentViewModelPropertyChanged("AvalibleDocumentClasses");
            }
        }
        private VMMC_Core.Class selectedDocumentClass;
        public VMMC_Core.Class SelectedDocumentClass
        {
            get { return selectedDocumentClass; }
            set
            {
                selectedDocumentClass = value;
                OnDocumentViewModelPropertyChanged("SelectedDocumentClass");
            }
        }
        private VMMC_Core.Revision revision;
        public VMMC_Core.Revision Revision
        {
            get { return revision; }
            set
            {
                revision = value;
                OnDocumentViewModelPropertyChanged("Revision");
            }
        }
        private ObservableCollection<VMMC_Core.Files> filesList;
        public ObservableCollection<VMMC_Core.Files> FilesList
        {
            get { return filesList; }
            set
            {
                filesList = value;
                OnDocumentViewModelPropertyChanged("FilesList");
            }
        }
        private ObservableCollection<VMMC_Core.Relationship> relationshipsList;
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsList
        {
            get { return relationshipsList; }
            set
            {
                relationshipsList = value;
                OnDocumentViewModelPropertyChanged("RelationshipsList");
            }
        }
        private ObservableCollection<VMMC_Core.DbObject> relatedObjectsList;
        public ObservableCollection<VMMC_Core.DbObject> RelatedObjectsList
        {
            get { return relatedObjectsList; }
            set
            {
                relatedObjectsList = value;
                OnDocumentViewModelPropertyChanged("RelatedObjectsList");
            }
        }
        private ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValueList;
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValueList
        {
            get { return attributeObjectValueList; }
            set
            {
                attributeObjectValueList = value;
                OnDocumentViewModelPropertyChanged("AttributeObjectValueList");
            }
        }
        private VMMC_Core.CommonControls.AttributeViewModel attributeViewModel;
        public VMMC_Core.CommonControls.AttributeViewModel AttributeViewModel
        {
            get { return attributeViewModel; }
            set
            {
                attributeViewModel = value;
                OnDocumentViewModelPropertyChanged("AttributeViewModel");
            }
        }
        private VMMC_Core.CommonControls.RelationshipViewModel relationshipViewModel;
        public VMMC_Core.CommonControls.RelationshipViewModel RelationshipViewModel
        {
            get { return relationshipViewModel; }
            set
            {
                relationshipViewModel = value;
                OnDocumentViewModelPropertyChanged("RelationshipViewModel");
            }
        }

        public void OpenFileEvent(VMMC_Core.Files selectedFile, string filePath)
        {
            try
            {
                int fileId = selectedFile.FileId;

                FileGetterServiceReference.FileGetterServiceClient ttt = new FileGetterServiceReference.FileGetterServiceClient();
                FileGetterServiceReference.GetFileResult result = ttt.GetFile(fileId, sessionInfo.ProjectCode);
                //result = ttt.GetFile(154, "D33");
                //if (!result.IsGetSuccess && result.FileName != null) result.IsGetSuccess = true;
                if (result.IsGetSuccess)
                {
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileStream.Write(result.FileData, 0, result.FileData.Length);
                    }

                    //return true;
                }
                else
                {

                    //return false;
                }
            }
            catch (Exception expt)
            {
                MessageBox.Show("Error writing data.", "MyProgram");
                MessageBox.Show(expt.Message, "MyProgram");
                MessageBox.Show(expt.StackTrace, "MyProgram");


                
                //return false;
            }
        }


        //public List<VMMC_Core.Revision> Revisions { get; set; }

        public DocumentViewModel(VMMC_Core.Document document, VMMC_Core.Revision revision, ObservableCollection<VMMC_Core.Files> files, bool editMode, VMMC_Core.SessionInfo session)
        {
            Document = document;
            EditMode = editMode;
            sessionInfo = session;
            if (Document != null)
            {
                IsDocumentEditMode = !Document.IsExistInDB;
                if (revision != null)
                {
                    Revision = document.Revisions.Where(x => x.RevisionId == revision.RevisionId).FirstOrDefault();
                    if (Revision != null) IsRevisionEditMode = !Revision.IsExistInDB;
                }
                else Revision = document.Revisions.Where(x => x.IsCurrent == true).FirstOrDefault();

                if (files != null)
                {
                    FilesList = files;
                }
                else
                {
                    if (Revision != null) FilesList = new VMMC_Core.Files(Revision.sessionInfo).GetFilesByRevision(Revision.RevisionId);
                }
                RelationshipsList = new VMMC_Core.Relationship(Document.sessionInfo).GetDbObjectRelationshipsList(Document.DocumentId);

                RelatedObjectsList = new ObservableCollection<DbObject>();
                foreach (VMMC_Core.Relationship rel in RelationshipsList)
                {
                    VMMC_Core.DbObject relObj = new DbObject(rel.sessionInfo);
                    if (rel.LeftObjectId == Document.DocumentId)
                    {
                        relObj = relObj.GetObject(rel.RightObjectId);
                        rel.RightObject = relObj;
                    }
                    else if (rel.RightObjectId == Document.DocumentId)
                    {
                        relObj = relObj.GetObject(rel.LeftObjectId);
                        rel.LeftObject = relObj;
                    }
                    RelatedObjectsList.Add(relObj);
                }

                AttributeObjectValueList = new VMMC_Core.AttributeObjectValue(Document.sessionInfo).GetDbAttributeObjectValuesList(Document.DocumentId);
                

                AttributeViewModel = new VMMC_Core.CommonControls.AttributeViewModel(AttributeObjectValueList);
                RelationshipViewModel = new VMMC_Core.CommonControls.RelationshipViewModel(RelatedObjectsList);

                AvalibleDocumentClasses = new VMMC_Core.Class(sessionInfo).getDocumentClasses();
                SelectedDocumentClass = AvalibleDocumentClasses.Where(x => x.ClassId == document.DocumentClassId).FirstOrDefault();
                
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnDocumentViewModelPropertyChanged([CallerMemberName] string prop = "")
        {

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
