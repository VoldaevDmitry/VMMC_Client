using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Xml;
using VMMC_Core;
using VMMC_ExcelParcer;

namespace VMMC_Import
{
    public class ImportViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public ImportViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }


        public VMMC_Core.RelayCommand ExecuteListCheckComand
        {
            get
            {
                return executeListCheckComand ??
                  (executeListCheckComand = new RelayCommand(obj => { CheckListAsync(true); }));
            }
        }
        private VMMC_Core.RelayCommand executeListCheckComand;

        public VMMC_Core.RelayCommand ExecutePDFParserComand
        {
            get
            {
                return executePDFParserComand ??
                  (executePDFParserComand = new RelayCommand(obj => { ExecutePdfParcer(); }));
            }
        }
        private VMMC_Core.RelayCommand executePDFParserComand;

        public VMMC_Core.RelayCommand ExecuteExcelParserComand
        {
            get
            {
                return executeExcelParserComand ??
                  (executeExcelParserComand = new RelayCommand(obj => { ExecuteExcelParcer(); }));
            }
        }
        private VMMC_Core.RelayCommand executeExcelParserComand;

        public VMMC_Core.RelayCommand ExecuteFileParserComand
        {
            get
            {
                return executeFileParserComand ??
                  (executeFileParserComand = new RelayCommand(obj => { ExecuteFileParcer(); }));
            }
        }
        private VMMC_Core.RelayCommand executeFileParserComand;

        public VMMC_Core.RelayCommand ExecuteDataBaseParserComand
        {
            get
            {
                return executeDataBaseParserComand ??
                  (executeDataBaseParserComand = new RelayCommand(obj => { ExecuteDataBaseParcer(); }));
            }
        }
        private VMMC_Core.RelayCommand executeDataBaseParserComand;
        public VMMC_Core.RelayCommand StartAsyncImportComand
        {
            get
            {
                return startAsyncImportComand ??
                  (startAsyncImportComand = new RelayCommand(obj => { startImportBtnClk(); }));
            }
        }
        private VMMC_Core.RelayCommand startAsyncImportComand;

        public VMMC_Core.RelayCommand OpenSavedDataComand
        {
            get
            {
                return openSavedDataComand ??
                  (openSavedDataComand = new RelayCommand(obj => { OpenSavedData(); }));
            }
        }
        private VMMC_Core.RelayCommand openSavedDataComand;
        public ObservableCollection<VMMC_Core.Complekt> ComplektsCollection
        {
            get { return complektsCollection; }
            set
            {
                complektsCollection = value;
                OnImportPropertyChanged("ComplektsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Complekt> complektsCollection;

        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnImportPropertyChanged("DocumentsCollection");
                OnImportPropertyChanged("RevisionsCollection");
                OnImportPropertyChanged("FilesCollection");
                if (DocumentsCollection!=null) DocumentsCollection.CollectionChanged += OnDocumentsCollectionChanged;
            }
        }
        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        private void OnDocumentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnImportPropertyChanged("DocumentsCollection");
            OnImportPropertyChanged("RevisionsCollection");
            OnImportPropertyChanged("FilesCollection");
        }

        public ObservableCollection<VMMC_Core.Revision> RevisionsCollection
        {
            get { return getRevisionCollection(); }
        }

        public ObservableCollection<VMMC_Core.Files> FilesCollection
        {
            get { return getFilesCollection(); }
        }

        public ObservableCollection<VMMC_Core.Tag> TagsCollection
        {
            get { return tagsCollection; }
            set
            {
                tagsCollection = value;
                OnImportPropertyChanged("TagsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Tag> tagsCollection;


        public ObservableCollection<VMMC_Core.TreeItem> TreeItemsCollection
        {
            get { return treeItemsCollection; }
            set
            {
                treeItemsCollection = value;
                OnImportPropertyChanged("TreeItemsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.TreeItem> treeItemsCollection;
        
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsCollection
        {
            get { return relationshipsCollection; }
            set
            {
                relationshipsCollection = value;
                OnImportPropertyChanged("RelationshipsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Relationship> relationshipsCollection;

        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValuesCollection
        {
            get { return attributeObjectValuesCollection; }
            set
            {
                attributeObjectValuesCollection = value;
                OnImportPropertyChanged("AttributeObjectValuesCollection");
            }
        }
        private ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesCollection;
        
        public ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> AttributeObjectValueViewModelCollection
        {
            get { return attributeObjectValueViewModelCollection; }
            set
            {
                attributeObjectValueViewModelCollection = value;
                OnImportPropertyChanged("AttributeObjectValueViewModelCollection");
            }
        }
        private ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> attributeObjectValueViewModelCollection;

        public bool IsNeedUploadComplekts
        {
            get { return isNeedUploadComplekts; }
            set
            {
                isNeedUploadComplekts = value;
                OnImportPropertyChanged("IsNeedUploadComplekts");
            }
        }
        private bool isNeedUploadComplekts;
        public bool IsNeedUploadDocuments
        {
            get { return isNeedUploadDocuments; }
            set
            {
                isNeedUploadDocuments = value;
                OnImportPropertyChanged("IsNeedUploadDocuments");
            }
        }
        private bool isNeedUploadDocuments;
        public bool IsNeedUploadRevisions
        {
            get { return isNeedUploadRevisions; }
            set
            {
                isNeedUploadRevisions = value;
                OnImportPropertyChanged("IsNeedUploadRevisions");
            }
        }
        private bool isNeedUploadRevisions;
        public bool IsNeedUploadFiles
        {
            get { return isNeedUploadFiles; }
            set
            {
                isNeedUploadFiles = value;
                OnImportPropertyChanged("IsNeedUploadFiles");
            }
        }
        private bool isNeedUploadFiles;
        public bool IsNeedUploadTags
        {
            get { return isNeedUploadTags; }
            set
            {
                isNeedUploadTags = value;
                OnImportPropertyChanged("IsNeedUploadTags");
            }
        }
        private bool isNeedUploadTags;
        public bool IsNeedUploadRelationships
        {
            get { return isNeedUploadRelationships; }
            set
            {
                isNeedUploadRelationships = value;
                OnImportPropertyChanged("IsNeedUploadRelationships");
            }
        }
        private bool isNeedUploadRelationships;
        public bool IsNeedUploadTreeItems
        {
            get { return isNeedUploadTreeItems; }
            set
            {
                isNeedUploadTreeItems = value;
                OnImportPropertyChanged("IsNeedUploadTreeItems");
            }
        }
        private bool isNeedUploadTreeItems;
        public bool IsNeedUploadAttributes
        {
            get { return isNeedUploadAttributes; }
            set
            {
                isNeedUploadAttributes = value;
                OnImportPropertyChanged("IsNeedUploadAttributes");
            }
        }
        private bool isNeedUploadAttributes;
        public bool SilentMode
        {
            get { return silentMode; }
            set
            {
                silentMode = value;
                OnImportPropertyChanged("SilentMode");
            }
        }
        private bool silentMode;
        public bool HasParentWindow
        {
            get { return hasParentWindow; }
            set
            {
                hasParentWindow = value;
                OnImportPropertyChanged("HasParentWindow");
            }
        }
        private bool hasParentWindow;



        private ObservableCollection<VMMC_Core.Revision> getRevisionCollection()
        {
            if (DocumentsCollection != null)
            {
                ObservableCollection<VMMC_Core.Revision> resultCollection = new ObservableCollection<Revision>();
                foreach (var doc in DocumentsCollection)
                {
                    if (doc != null)
                    {
                        if (doc.Revisions != null)
                        {
                            foreach (VMMC_Core.Revision rev in doc.Revisions)
                            {
                                resultCollection.Add(rev);
                            }
                        }
                    }
                }

                return resultCollection;
            }
            else return null;

        }
        private ObservableCollection<VMMC_Core.Files> getFilesCollection()
        {
            if (DocumentsCollection != null)
            {
                ObservableCollection<VMMC_Core.Files> resultCollection = new ObservableCollection<VMMC_Core.Files>();
                foreach (var doc in DocumentsCollection)
                {
                    if (doc != null)
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
                }

                return resultCollection;
            }
            else return null;

        }

        private void ExecuteExcelParcer()
        {
            VMMC_ExcelParcer.MainWindow excelParserMainWindow = new VMMC_ExcelParcer.MainWindow(sessionInfo);

            VMMC_ExcelParcer.ExcelParserViewModel excelParserViewModel = (VMMC_ExcelParcer.ExcelParserViewModel)excelParserMainWindow.DataContext;
            excelParserViewModel.HasParentWindow = true;

            //this.Visibility = Visibility.Hidden;
            var ttt = excelParserMainWindow.ShowDialog();

            if (ttt == true)
            {
                excelParserViewModel = (VMMC_ExcelParcer.ExcelParserViewModel)excelParserMainWindow.DataContext;
                ComplektsCollection = excelParserViewModel.ComplektsCollection;
                DocumentsCollection = excelParserViewModel.DocumentsCollection;
                //TagsCollection = excelParserViewModel.TagsCollection;
                AttributeObjectValuesCollection = excelParserViewModel.AttributeObjectValuesCollection;
                TreeItemsCollection = excelParserViewModel.TreeItemsCollection;
                AttributeObjectValueViewModelCollection = excelParserViewModel.AttributeObjectValueViewModelCollection;
                TagsCollection = excelParserViewModel.TagsCollection;
                RelationshipsCollection = excelParserViewModel.RelationshipsCollection;
                CheckListAsync(false);
            }
        }        
        private void ExecuteDataBaseParcer()
        {
            VMMC_DataBaseParcer.MainWindow dataBaseParcerMainWindow = new VMMC_DataBaseParcer.MainWindow(sessionInfo);

            VMMC_DataBaseParcer.DataBaseParcerViewModel dataBaseParcerViewModel = (VMMC_DataBaseParcer.DataBaseParcerViewModel)dataBaseParcerMainWindow.DataContext;
            dataBaseParcerViewModel.HasParentWindow = true;


            //this.Visibility = Visibility.Hidden;
            var ttt = dataBaseParcerMainWindow.ShowDialog();

            if (ttt == true)
            {
                dataBaseParcerViewModel = (VMMC_DataBaseParcer.DataBaseParcerViewModel)dataBaseParcerMainWindow.DataContext;
                AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                Guid projectId = Guid.NewGuid();
                DocumentsCollection = dataBaseParcerViewModel.DocumentsCollection;
                //importViewModel.RevisionsCollection = new ObservableCollection<VMMC_Core.Revision>();
                if (dataBaseParcerViewModel.DocumentsCollection != null)
                {
                    foreach (VMMC_Core.Document document in dataBaseParcerViewModel.DocumentsCollection)
                    {
                        foreach (VMMC_Core.Revision revision in document.Revisions)
                        {
                            RevisionsCollection.Add(revision);
                        }
                    }
                }
                if (dataBaseParcerViewModel.ComplektsCollection != null) ComplektsCollection = dataBaseParcerViewModel.ComplektsCollection;
                if (dataBaseParcerViewModel.DocumentsCollection != null) DocumentsCollection = dataBaseParcerViewModel.DocumentsCollection;
                if (dataBaseParcerViewModel.RelationshipsCollection != null) RelationshipsCollection = dataBaseParcerViewModel.RelationshipsCollection;
                if (dataBaseParcerViewModel.TagsCollection != null) TagsCollection = dataBaseParcerViewModel.TagsCollection;
                if (dataBaseParcerViewModel.TreeItemsCollection != null) TreeItemsCollection = dataBaseParcerViewModel.TreeItemsCollection;
                if (dataBaseParcerViewModel.AttributeObjectValuesCollection != null)
                {
                    AttributeObjectValuesCollection = dataBaseParcerViewModel.AttributeObjectValuesCollection;

                    foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                    {
                        //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                    }
                }
                CheckListAsync(false);
            }
        }
        private void ExecuteFileParcer()
        {
            VMMC_FileParser.MainWindow fileParserMainWindow = new VMMC_FileParser.MainWindow(sessionInfo);

            VMMC_FileParser.FileParserViewModel fileParserViewModel = (VMMC_FileParser.FileParserViewModel)fileParserMainWindow.DataContext;
            fileParserViewModel.HasParentWindow = true;

            //this.Visibility = Visibility.Hidden;
            var ttt = fileParserMainWindow.ShowDialog();

            if (ttt == true)
            {
                fileParserViewModel = (VMMC_FileParser.FileParserViewModel)fileParserMainWindow.DataContext;

                ComplektsCollection = fileParserViewModel.ComplektsCollection;
                DocumentsCollection = fileParserViewModel.DocumentsCollection;

                //importViewModel.RevisionsCollection = new ObservableCollection<VMMC_Core.Revision>();
                foreach (VMMC_Core.Document document in fileParserViewModel.DocumentsCollection)
                {
                    foreach (VMMC_Core.Revision revision in document.Revisions)
                    {
                        RevisionsCollection.Add(revision);
                    }
                }

                //importViewModel.FilesCollection = new ObservableCollection<VMMC_Core.Files>();
                foreach (VMMC_Core.LocalFile file in fileParserViewModel.LocalFilesCollection)
                {
                    VMMC_Core.Document resDoc = fileParserViewModel.DocumentsCollection.Where(x => x.DocumentCode == file.Document.DocumentCode).FirstOrDefault();

                    VMMC_Core.Revision resRev = resDoc.Revisions.Where(x => x.DocumentId == resDoc.DocumentId && x.Number == file.Revision.Number).FirstOrDefault();

                    VMMC_Core.Files newFile = new VMMC_Core.Files(sessionInfo)
                    {
                        FileGuid = Guid.NewGuid(),
                        RevisionId = resRev.RevisionId,
                        FileName = file.LocalFileName,
                        FileType = file.LocalFileType,
                        Checksum = file.Checksum,
                        Status = file.Status,
                        StatusInfo = file.StatusInfo
                    };
                    FilesCollection.Add(newFile);
                }

                RelationshipsCollection = fileParserViewModel.RelationshipsCollection;

                CheckListAsync(false);
            }
        }
        private void ExecutePdfParcer()
        {
            VMMC_PdfParcer.MainWindow pdfParcerMainWindow = new VMMC_PdfParcer.MainWindow(sessionInfo);

            VMMC_PdfParcer.PdfParcerViewModel pdfParcerViewModel = (VMMC_PdfParcer.PdfParcerViewModel)pdfParcerMainWindow.DataContext;
            pdfParcerViewModel.HasParentWindow = true;

            //this.Visibility = Visibility.Hidden;
            var ttt = pdfParcerMainWindow.ShowDialog();

            if (ttt == true)
            {
                pdfParcerViewModel = (VMMC_PdfParcer.PdfParcerViewModel)pdfParcerMainWindow.DataContext;
                ComplektsCollection = pdfParcerViewModel.ComplektsCollection;
                DocumentsCollection = pdfParcerViewModel.DocumentsCollection;
                //TagsCollection = excelParserViewModel.TagsCollection;
                AttributeObjectValuesCollection = pdfParcerViewModel.AttributeObjectValuesCollection;
                TreeItemsCollection = pdfParcerViewModel.TreeItemsCollection;
                AttributeObjectValueViewModelCollection = pdfParcerViewModel.AttributeObjectValueViewModelCollection;
                TagsCollection = pdfParcerViewModel.TagsCollection;
                RelationshipsCollection = pdfParcerViewModel.RelationshipsCollection;
                CheckListAsync(false);
            }
        }
        public async void CheckListAsync(bool loadDataFromDB)
        {           
            if (DocumentsCollection != null) await Task.Run(() => CheckDocumentCollection(loadDataFromDB));
            if (RevisionsCollection != null) await Task.Run(() => CheckRevisionCollection(loadDataFromDB));
            if (ComplektsCollection != null) await Task.Run(() => CheckComplektCollection(loadDataFromDB));
            if (TagsCollection != null) await Task.Run(() => CheckTagCollection(loadDataFromDB));
            if (RelationshipsCollection != null) await Task.Run(() => CheckRelationshipCollection(loadDataFromDB));
            if (FilesCollection != null) await Task.Run(() => CheckFilesCollection(loadDataFromDB, true));

        }
        public void CheckDocumentCollection(bool loadDataFromDB)
        {
            //проверка и объединение документов с одинаковым шифром.

            //проверка наличия документа в БД.
            ObservableCollection<VMMC_Core.Document> dbDocCollection = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
            foreach (VMMC_Core.Document doc in DocumentsCollection)
            {
                if (doc != null)
                {
                    VMMC_Core.Document dbDoc = dbDocCollection.Where(x => x.DocumentCode == doc.DocumentCode).FirstOrDefault();
                    if (dbDoc != null)
                    {
                        doc.IsExistInDB = true;
                        doc.StatusInfo = "Документ существует в БД. ";
                        ChangeDocGuid(doc.DocumentId, dbDoc.DocumentId);
                        doc.DocumentCode = dbDoc.DocumentCode;
                        if (loadDataFromDB)
                        {
                            if (doc.DocumentName != dbDoc.DocumentName) doc.StatusInfo += "Указанное наименование отличается от загруженного. ";
                            doc.DocumentName = dbDoc.DocumentName;
                            if (doc.DocumentClassId != dbDoc.DocumentClassId) doc.StatusInfo += "Указанный класс отличается от загруженного. ";
                            doc.DocumentClassId = dbDoc.DocumentClassId;
                        }
                        else
                        {
                            if (doc.DocumentName != dbDoc.DocumentName) doc.StatusInfo += "Указанное наименование отличается от загруженного. ";
                            if (doc.DocumentClassId != dbDoc.DocumentClassId) doc.StatusInfo += "Указанный класс отличается от загруженного. ";
                        }
                    }
                }
            }
        }        
        public void CheckRevisionCollection(bool loadDataFromDB)
        {
            ObservableCollection<VMMC_Core.Revision> dbRevCollection = new VMMC_Core.Revision(sessionInfo).GetDbRevisionsList();
            foreach (VMMC_Core.Revision rev in RevisionsCollection)
            {
                VMMC_Core.Revision dbRev = dbRevCollection.Where(x => x.DocumentId == rev.DocumentId && x.Number == rev.Number).FirstOrDefault();
                if (dbRev != null)
                {
                    ChangeRevGuid(rev.RevisionId, dbRev.RevisionId);
                    rev.IsCurrent = dbRev.IsCurrent;
                    rev.RevisionDate = dbRev.RevisionDate;
                    rev.StatusInfo = "ревизия существует в БД";
                    rev.IsExistInDB = true;
                }
            }
        }
        public void CheckComplektCollection(bool loadDataFromDB)
        {
            ObservableCollection<VMMC_Core.Complekt> dbComCollection = new VMMC_Core.Complekt(sessionInfo).GetDbComplektsList();
            if(dbComCollection == null) dbComCollection = new ObservableCollection<VMMC_Core.Complekt>();
            foreach (VMMC_Core.Complekt com in ComplektsCollection)
            {
                VMMC_Core.Complekt dbCom = dbComCollection.Where(x => x.ComplektCode == com.ComplektCode).FirstOrDefault();
                if (dbCom != null)
                {
                    ChangeComGuid(com.ComplektId, dbCom.ComplektId);
                    com.ComplektName = dbCom.ComplektName;
                    com.ComplektCode = dbCom.ComplektCode;
                    com.ComplektClassId = dbCom.ComplektClassId;
                    com.StatusInfo = "комплект существует в БД";
                    com.IsExistInDB = true;
                }
            }
        }
        public void CheckTagCollection(bool loadDataFromDB)
        {
            List<VMMC_Core.Tag> dbTagCollection = new VMMC_Core.Tag(sessionInfo).GetDbTagsList();
            foreach (VMMC_Core.Tag tag in TagsCollection)
            {
                VMMC_Core.Tag dbTag = dbTagCollection.Where(x => x.Position == tag.Position).FirstOrDefault();
                if (dbTag != null)
                {
                    ChangeTagGuid(tag.TagId, dbTag.TagId);

                    tag.TagId = dbTag.TagId;
                    tag.Position = dbTag.Position;
                    tag.TagName = dbTag.TagName;
                    tag.Characteristic = dbTag.Characteristic;
                    tag.TreeItemId = dbTag.TreeItemId;
                    tag.TagClassId = dbTag.TagClassId;
                    tag.StatusInfo = "тег существует в БД";
                    tag.IsExistInDB = true;
                }
            }
        }
        public void CheckRelationshipCollection(bool loadDataFromDB)
        {
            ObservableCollection<VMMC_Core.Relationship> dbRelCollection = new VMMC_Core.Relationship(sessionInfo).GetDbRelationshipsList();
            foreach (VMMC_Core.Relationship rel in RelationshipsCollection)
            {
                if (rel.LeftObjectId == Guid.Parse("00000000-0000-0000-0000-000000000000") && rel.LeftObject!=null) rel.LeftObjectId = rel.LeftObject.ObjectId;
                if (rel.RightObjectId == Guid.Parse("00000000-0000-0000-0000-000000000000") && rel.RightObject != null) rel.RightObjectId = rel.RightObject.ObjectId;
                VMMC_Core.Relationship dbRel = dbRelCollection.Where(x => x.LeftObjectId == rel.LeftObjectId && x.RightObjectId == rel.RightObjectId).FirstOrDefault();
                if (dbRel != null)
                {

                    rel.RelationshipId = dbRel.RelationshipId;
                    rel.RelTypeId = dbRel.RelTypeId;
                    rel.LeftObjectId = dbRel.LeftObjectId;
                    rel.RightObjectId = dbRel.RightObjectId;
                    rel.StatusInfo = "отношение существует в БД";
                    rel.IsExistInDB = true;
                }
            }
        }
        public void CheckFilesCollection(bool loadDataFromDB, bool ignoreChecksum)
        {
            ObservableCollection<VMMC_Core.Files> dbComCollection = new VMMC_Core.Files(sessionInfo).GetDbFiles();
            foreach (VMMC_Core.Files files in FilesCollection)
            {
                VMMC_Core.Files existDbFile = dbComCollection.Where(x => x.RevisionId == files.RevisionId && x.FileName == files.FileName).FirstOrDefault();
                if(existDbFile == null && !ignoreChecksum) existDbFile = dbComCollection.Where(x => x.Checksum == files.Checksum).FirstOrDefault();
                if (existDbFile != null)
                {
                    files.FileId = existDbFile.FileId;
                    files.FileGuid = existDbFile.FileGuid;
                    files.RevisionId = existDbFile.RevisionId;
                    files.FileName = existDbFile.FileName;
                    files.FileSize = existDbFile.FileSize;
                    files.FileType = existDbFile.FileType;
                    files.StatusInfo = "файл существует в БД";
                    files.IsExistInDB = true;
                }
            }
        }

        private void ChangeObjectGuidInRelationships(Guid oldGuid, Guid newGuid)
        {
            IEnumerable<VMMC_Core.Relationship> targetRelCollection = RelationshipsCollection.Where(x => x.LeftObjectId == oldGuid || x.RightObjectId == oldGuid);
            foreach (VMMC_Core.Relationship relationship in targetRelCollection)
            {
                if (relationship.LeftObjectId == oldGuid) relationship.LeftObjectId = newGuid;
                if (relationship.RightObjectId == oldGuid) relationship.RightObjectId = newGuid;
            }
        }
        private void ChangeObjectGuidInAttributess(Guid oldGuid, Guid newGuid)
        {            
            IEnumerable<VMMC_Core.AttributeObjectValue> targetAtrCollection = AttributeObjectValuesCollection.Where(x => x.Object.ObjectId == oldGuid);
            foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in targetAtrCollection)
            {
                attributeObjectValue.Object.ObjectId = newGuid;                
            }
        }
        private void ChangeDocGuid(Guid oldGuid, Guid newGuid)
        {
            IEnumerable<VMMC_Core.Document> targetDocCollection = DocumentsCollection.Where(x => x.DocumentId == oldGuid);
            foreach (VMMC_Core.Document document in targetDocCollection)
            {
                document.DocumentId = newGuid;
            }

            IEnumerable<VMMC_Core.Revision> targetRevCollection = RevisionsCollection.Where(x => x.DocumentId == oldGuid);
            foreach (VMMC_Core.Revision revision in targetRevCollection)
            {
                revision.DocumentId = newGuid;
            }

            IEnumerable<VMMC_Core.Relationship> targetRelCollection = RelationshipsCollection.Where(x => x.LeftObjectId == oldGuid || x.RightObjectId == oldGuid);
            foreach (VMMC_Core.Relationship relationship in targetRelCollection)
            {
                if (relationship.LeftObjectId == oldGuid) relationship.LeftObjectId = newGuid;
                if (relationship.RightObjectId == oldGuid) relationship.RightObjectId = newGuid;
            }
        }
        private void ChangeRevGuid(Guid oldGuid, Guid newGuid)
        {
            IEnumerable<VMMC_Core.Revision> targetRevCollection = RevisionsCollection.Where(x => x.RevisionId == oldGuid);
            foreach (VMMC_Core.Revision revision in targetRevCollection)
            {
                revision.RevisionId = newGuid;
            }

            if (FilesCollection != null)
            {
                IEnumerable<VMMC_Core.Files> targetFilesCollection = FilesCollection.Where(x => x.RevisionId == oldGuid);
                foreach (VMMC_Core.Files file in targetFilesCollection)
                {
                    file.RevisionId = newGuid;
                }
            }
        }
        private void ChangeComGuid(Guid oldGuid, Guid newGuid)
        {
            if (ComplektsCollection != null)
            {
                IEnumerable<VMMC_Core.Complekt> targetDocCollection = ComplektsCollection.Where(x => x.ComplektId == oldGuid);
                foreach (VMMC_Core.Complekt complekt in targetDocCollection)
                {
                    complekt.ComplektId = newGuid;
                }
            }
            if (RelationshipsCollection != null)
            {
                IEnumerable<VMMC_Core.Relationship> targetRelCollection = RelationshipsCollection.Where(x => x.LeftObjectId == oldGuid || x.RightObjectId == oldGuid);
                foreach (VMMC_Core.Relationship relationship in targetRelCollection)
                {
                    if (relationship.LeftObjectId == oldGuid) relationship.LeftObjectId = newGuid;
                    if (relationship.RightObjectId == oldGuid) relationship.RightObjectId = newGuid;
                }
            }
        }
        private void ChangeTagGuid(Guid oldGuid, Guid newGuid)
        {
            IEnumerable<VMMC_Core.Tag> targetTagCollection = TagsCollection.Where(x => x.TagId == oldGuid);
            foreach (VMMC_Core.Tag tag in targetTagCollection)
            {
                tag.TagId = newGuid;
            }

            IEnumerable<VMMC_Core.Relationship> targetRelCollection = RelationshipsCollection.Where(x => x.LeftObjectId == oldGuid || x.RightObjectId == oldGuid);
            foreach (VMMC_Core.Relationship relationship in targetRelCollection)
            {
                if (relationship.LeftObjectId == oldGuid) relationship.LeftObjectId = newGuid;
                if (relationship.RightObjectId == oldGuid) relationship.RightObjectId = newGuid;
            }
        }

        private void startImportBtnClk() 
        {
            bool isUpdateDBData = false;

            string sMessageBoxText = "Обновить существующие записи в БД?\n\nДА-существующие в БД данные обновятся согласно указанным в списке\n\nНЕТ-будут дозагружены только не существующие в БД данные";

            string sCaption = "My Test Application";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    ImportAsync(true);
                    break;

                case MessageBoxResult.No:
                    ImportAsync(false);
                    break;

                case MessageBoxResult.Cancel:
                    /* ... */
                    break;
            }
        }

        private async void ImportAsync(bool isUpdateDBData)
        {
            if (IsNeedUploadComplekts)
            {
                await Task.Run(() => ImportComplekts());
            }
            if (IsNeedUploadDocuments)
            {
                await Task.Run(() => ImportDocuments());
            }
            if ((bool)IsNeedUploadRevisions)
            {
                await Task.Run(() => ImportRevisions());
            }
            if (IsNeedUploadFiles)
            {
                await Task.Run(() => ImportFiles());
            }
            if (IsNeedUploadTags)
            {
                await Task.Run(() => ImportTags());
            }
            if (IsNeedUploadRelationships)
            {
                await Task.Run(() => ImportRelationships());
            }
            if (IsNeedUploadTreeItems)
            {
                await Task.Run(() => ImportTreeItems());
            }
            if (IsNeedUploadAttributes)
            {
                await Task.Run(() => ImportAttributes());
            }            
        }
        public void ImportComplekts()
        {
            if (ComplektsCollection != null)
            {
                if (ComplektsCollection.Count > 0)
                {
                    foreach (VMMC_Core.Complekt complekt in ComplektsCollection)
                    {
                        if (!complekt.IsExistInDB)
                            complekt.CreateDBComplekt();
                        else complekt.CreateDBComplekt();
                    }
                }
            }
        }
        public void ImportDocuments()
        {
            if (DocumentsCollection != null)
            {
                if (DocumentsCollection.Count > 0)
                {
                    foreach (VMMC_Core.Document document in DocumentsCollection)
                    {
                        if (!document.IsExistInDB)
                            document.CreateDBDocument();
                        else document.UpdateDocument();
                    }
                }
            }
        }
        public void ImportRevisions()
        {
            if (RevisionsCollection != null)
            {
                if (RevisionsCollection.Count > 0)
                {
                    int i = 0;
                    foreach (VMMC_Core.Revision revision in RevisionsCollection)
                    {
                        i++;
                        if (!revision.IsExistInDB)
                        {
                            revision.CreateDBRevision();
                        }
                    }
                }
            }
        }
        public void ImportTags()
        {
            if (TagsCollection != null)
            {
                if (TagsCollection.Count > 0)
                {
                    foreach (VMMC_Core.Tag tag in TagsCollection)
                    {
                        if (!tag.IsExistInDB)
                        {
                            tag.CreateDBTag();
                        }
                    }
                }
            }
        }
        public void ImportFiles()
        {
            if (FilesCollection != null)
            {
                if (FilesCollection.Count > 0)
                {
                    int i = 0;
                    foreach (VMMC_Core.Files files in FilesCollection)
                    {
                        i++;
                        if (!files.IsExistInDB)
                            files.CreateDbFile(true);
                    }
                }
            }
        }
        public void ImportRelationships()
        {
            if (RelationshipsCollection != null)
            {
                if (RelationshipsCollection.Count > 0)
                {
                    foreach (VMMC_Core.Relationship relationship in RelationshipsCollection)
                    {
                        if (!relationship.IsExistInDB)
                            relationship.CreateDBRelationship();
                    }
                }
            }
        }
        public void ImportTreeItems()
        {
            if (TreeItemsCollection != null)
            {
                if (TreeItemsCollection.Count > 0)
                {
                    foreach (VMMC_Core.TreeItem treeItem in TreeItemsCollection)
                    {
                        if (!treeItem.IsExistInDB)
                            treeItem.CreateDBTreeItem();
                    }
                }
            }
        }
        public void ImportAttributes()
        {
            if (AttributeObjectValuesCollection != null)
            {
                if (AttributeObjectValuesCollection.Count > 0)
                {
                    foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                    {
                        if (!aov.IsExistInDB)
                            aov.CreateDBAttributeObjectValue();
                            //aov. CreateDBTreeItem();
                    }
                }
            }
        }



        public void OpenSavedData()
        {
            
                //try
                //{
                //    if (dialogService.OpenFileDialog() == true)
                //    {
                //        var phones = fileService.Open(dialogService.FilePath);
                //        //LoadedFilesCollection.Clear();
                //        SavedLoadedFilesCollection.Clear();
                //        foreach (var p in phones)
                //        {
                //            //LoadedFilesCollection.Add(p);
                //            SavedLoadedFilesCollection.Add(p);
                //        }

                //        dialogService.ShowMessage("Файл открыт");


                //    }
                //}
                //catch (Exception ex)
                //{
                //    dialogService.ShowMessage(ex.Message);
                //}


            using (var dialog = new System.Windows.Forms.OpenFileDialog())
            {
                dialog.Multiselect = true;

                DocumentsCollection = null;
                ComplektsCollection = null;
                TagsCollection = null;
                TreeItemsCollection = null;
                RelationshipsCollection = null;

                dialog.ShowDialog();


                if (dialog.FileNames != null)
                {
                    foreach (string fileName in dialog.FileNames)
                    {
                        var xml = File.ReadAllText(fileName);
                        var parsed = new XmlDocument();
                        parsed.LoadXml(xml);

                        //var phones = fileService.Open(dialogService.FilePath);
                        ////LoadedFilesCollection.Clear();
                        //SavedLoadedFilesCollection.Clear();
                        //foreach (var p in phones)
                        //{
                        //    //LoadedFilesCollection.Add(p);
                        //    SavedLoadedFilesCollection.Add(p);
                        //}

                        //AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        //ExcelParserViewModel newExcelParserViewModel = GetBKFILLTagsFromExcel(fileName);

                        //if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        //else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        //if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        //else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        //if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        //else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        //if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        //else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        //if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        //else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        //if (AttributeObjectValuesCollection != null)
                        //{
                        //    foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                        //    {
                        //        AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel() { AttributeObjectValue = aov });
                        //    }
                        //}
                        //if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        //else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnImportPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
