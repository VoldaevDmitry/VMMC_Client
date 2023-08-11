using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VMMC_Core;

namespace VMMC_ExcelParcer
{
    public class ExcelParserViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.SessionInfo sessionInfo;

        private ObservableCollection<VMMC_Core.Complekt> complektsCollection;
        public ObservableCollection<VMMC_Core.Complekt> ComplektsCollection
        {
            get { return complektsCollection; }
            set
            {
                complektsCollection = value;
                OnExcelParserPropertyChanged("ComplektsCollection");
            }
        }

        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnExcelParserPropertyChanged("DocumentsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesCollection;
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValuesCollection
        {
            get { return attributeObjectValuesCollection; }
            set
            {
                attributeObjectValuesCollection = value;
                OnExcelParserPropertyChanged("AttributeObjectValuesCollection");
            }
        }



        private ObservableCollection<VMMC_Core.Tag> tagsCollection;
        public ObservableCollection<VMMC_Core.Tag> TagsCollection
        {
            get { return tagsCollection; }
            set
            {
                tagsCollection = value;
                OnExcelParserPropertyChanged("TagsCollection");
            }
        }

        private ObservableCollection<VMMC_Core.TreeItem> treeItemsCollection;
        public ObservableCollection<VMMC_Core.TreeItem> TreeItemsCollection
        {
            get { return treeItemsCollection; }
            set
            {
                treeItemsCollection = value;
                OnExcelParserPropertyChanged("TreeItemsCollection");
            }
        }

        private ObservableCollection<VMMC_Core.Relationship> relationshipsCollection;
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsCollection
        {
            get { return relationshipsCollection; }
            set
            {
                relationshipsCollection = value;
                OnExcelParserPropertyChanged("RelationshipsCollection");
            }
        }

        private ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> attributeObjectValueViewModelCollection;
        public ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> AttributeObjectValueViewModelCollection
        {
            get { return attributeObjectValueViewModelCollection; }
            set
            {
                attributeObjectValueViewModelCollection = value;
                OnExcelParserPropertyChanged("AttributeObjectValueViewModelCollection");
            }
        }

        private bool silentMode;
        public bool SilentMode
        {
            get { return silentMode; }
            set
            {
                silentMode = value;
                OnExcelParserPropertyChanged("SilentMode");
            }
        }

        private bool hasParentWindow;
        public bool HasParentWindow
        {
            get { return hasParentWindow; }
            set
            {
                hasParentWindow = value;
                OnExcelParserPropertyChanged("HasParentWindow");
            }
        }

        private VMMC_Core.RelayCommand getDocuments3DTreeItemFromExcelCommand;
        public VMMC_Core.RelayCommand GetDocuments3DTreeItemFromExcelCommand
        {
            get
            {
                return getDocuments3DTreeItemFromExcelCommand ??
                  (getDocuments3DTreeItemFromExcelCommand = new RelayCommand(obj => { GetDocuments3DTreeItemFromExcel(); }));
            }
        }


        private VMMC_Core.RelayCommand getDocuments3DDocumentsRdFromExcelCommand;
        public VMMC_Core.RelayCommand GetDocuments3DDocumentsRdFromExcelCommand
        {
            get
            {
                return getDocuments3DDocumentsRdFromExcelCommand ??
                  (getDocuments3DDocumentsRdFromExcelCommand = new RelayCommand(obj => { GetDocuments3DDocumentsRdFromExcel(); }));
            }
        }


        private VMMC_Core.RelayCommand getDocuments3DTagFromExcelCommand;
        public VMMC_Core.RelayCommand GetDocuments3DTagFromExcelCommand
        {
            get
            {
                return getDocuments3DTagFromExcelCommand ??
                  (getDocuments3DTagFromExcelCommand = new RelayCommand(obj => { GetDocuments3DTagFromExcel(); }));
            }
        }


        private VMMC_Core.RelayCommand getComplektsWithRevisionsListCommand;
        public VMMC_Core.RelayCommand GetComplektsWithRevisionsListCommand
        {
            get
            {
                return getComplektsWithRevisionsListCommand ??
                  (getComplektsWithRevisionsListCommand = new RelayCommand(obj => { GetComplektsWithRevisionsList(); }));
            }
        }


        private VMMC_Core.RelayCommand getDocumentsRdTagsRelationshipsFromExcelCommand;
        public VMMC_Core.RelayCommand GetDocumentsRdTagsRelationshipsFromExcelCommand
        {
            get
            {
                return getDocumentsRdTagsRelationshipsFromExcelCommand ??
                  (getDocumentsRdTagsRelationshipsFromExcelCommand = new RelayCommand(obj => { GetDocumentsRdTagsRelationshipsFromExcel(); }));
            }
        }


        private VMMC_Core.RelayCommand getDocumentsRdFromMDRExcelCommand;
        public VMMC_Core.RelayCommand GetDocumentsRdFromMDRExcelCommand
        {
            get
            {
                return getDocumentsRdFromMDRExcelCommand ??
                  (getDocumentsRdFromMDRExcelCommand = new RelayCommand(obj => { GetDocumentsRdFromMDRExcel(); }));
            }
        }


        private VMMC_Core.RelayCommand getBKFILLTagsFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLTagsFromExcelCommand
        {
            get
            {
                return getBKFILLTagsFromExcelCommand ??
                  (getBKFILLTagsFromExcelCommand = new RelayCommand(obj => { GetBKFILLTagsFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLTagTagRelFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLTagTagRelFromExcelCommand
        {
            get
            {
                return getBKFILLTagTagRelFromExcelCommand ??
                  (getBKFILLTagTagRelFromExcelCommand = new RelayCommand(obj => { GetBKFILLTagTagRelFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLTagsAttributesFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLTagsAttributesFromExcelCommand
        {
            get
            {
                return getBKFILLTagsAttributesFromExcelCommand ??
                  (getBKFILLTagsAttributesFromExcelCommand = new RelayCommand(obj => { GetBKFILLTagsAttributesFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLDocTreeFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLDocTreeFromExcelCommand
        {
            get
            {
                return getBKFILLDocTreeFromExcelCommand ??
                  (getBKFILLDocTreeFromExcelCommand = new RelayCommand(obj => { GetBKFILLDocTreeFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLDocumentsFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLDocumentsFromExcelCommand
        {
            get
            {
                return getBKFILLDocumentsFromExcelCommand ??
                  (getBKFILLDocumentsFromExcelCommand = new RelayCommand(obj => { GetBKFILLDocumentsFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLDocumentsRelationshipFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLDocumentsRelationshipFromExcelCommand
        {
            get
            {
                return getBKFILLDocumentsRelationshipFromExcelCommand ??
                  (getBKFILLDocumentsRelationshipFromExcelCommand = new RelayCommand(obj => { GetBKFILLDocumentsRelationshipFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLDocumentsFilesFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLDocumentsFilesFromExcelCommand
        {
            get
            {
                return getBKFILLDocumentsFilesFromExcelCommand ??
                  (getBKFILLDocumentsFilesFromExcelCommand = new RelayCommand(obj => { GetBKFILLDocumentsFilesFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLTagsEnumAttributeValuesFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLTagsEnumAttributeValuesFromExcelCommand
        {
            get
            {
                return getBKFILLTagsEnumAttributeValuesFromExcelCommand ??
                  (getBKFILLTagsEnumAttributeValuesFromExcelCommand = new RelayCommand(obj => { GetBKFILLTagsEnumAttributeValuesFromExcel(); }));
            }
        }

        private VMMC_Core.RelayCommand getBKFILLAttributeFromExcelCommand;
        public VMMC_Core.RelayCommand GetBKFILLAttributeFromExcelCommand
        {
            get
            {
                return getBKFILLAttributeFromExcelCommand ??
                  (getBKFILLAttributeFromExcelCommand = new RelayCommand(obj => { GetBKFILLAttributeFromExcel(); }));
            }
        }


        private VMMC_Core.RelayCommand exportToExcelCommand;
        public VMMC_Core.RelayCommand ExportToExcelCommand
        {
            get
            {
                return exportToExcelCommand ??
                  (exportToExcelCommand = new RelayCommand(obj => { new ExportTo().ExportToExcel2(ComplektsCollection); }));
            }
        }
        public ExcelParserViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }


        private void GetDocuments3DTreeItemFromExcel()
        {
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
                        ExcelParserViewModel newExcelParserViewModel = GetDocuments3DTreeItemFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);
                    }
                }
            }
        }
        public ExcelParserViewModel GetDocuments3DTreeItemFromExcel(string fileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(fileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    VMMC_Core.Document newDoc = new VMMC_Core.Document(sessionInfo);
                    newDoc.DocumentId = Guid.NewGuid();
                    newDoc.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("3D-Модель").ClassId;

                    newDoc.Status = new FileInfo(fileName).Name;

                    VMMC_Core.Revision newRev = new VMMC_Core.Revision(sessionInfo)
                    {
                        RevisionId = Guid.NewGuid(),
                        DocumentId = newDoc.DocumentId,
                        Number = 0,
                        IsCurrent = true
                    };

                    newDoc.Revisions = new ObservableCollection<VMMC_Core.Revision> { newRev };

                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "Шифр модели":
                                newDoc.DocumentCode = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Описание модели":
                                newDoc.DocumentName = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", "");
                                break;

                            case "Файл - шифр":
                                newDoc.DocumentName = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", "");
                                break;

                            default:
                                break;
                        }
                    }
                    if (newDoc.DocumentCode != null)
                    {
                        if (newDoc.DocumentCode != "") docList.Add(newDoc);
                    }
                }
            }

            ObservableCollection<VMMC_Core.Document> dbDocumentsCollection = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
            foreach (VMMC_Core.Document doc in docList)
            {
                VMMC_Core.Document dbDoc = dbDocumentsCollection.Where(x => x.DocumentCode == doc.DocumentCode).FirstOrDefault();
                if (dbDoc != null)
                {

                    dbDoc.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(dbDoc.DocumentId);
                    foreach (VMMC_Core.Revision rev in doc.Revisions)
                    {
                        VMMC_Core.Revision newRev = dbDoc.Revisions.Where(x => x.Number == rev.Number).FirstOrDefault();
                        rev.DocumentId = dbDoc.DocumentId;
                        if (newRev == null) dbDoc.Revisions.Add(rev);
                        //else doc.Revisions.Add(newRev);
                    }

                    //localFile.Document = dbDoc;
                    doc.DocumentId = dbDoc.DocumentId;
                    doc.DocumentCode = dbDoc.DocumentCode;
                    doc.DocumentName = dbDoc.DocumentName;
                    doc.DocumentClassId = dbDoc.DocumentClassId;
                    doc.Revisions = dbDoc.Revisions;
                    //doc.Status = FileName;
                    doc.StatusInfo = "Документ уже существует в БД";
                    doc.IsExistInDB = true;
                }
            }

            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.Relationship> relationshipList = new ObservableCollection<VMMC_Core.Relationship>();
            relationshipList.Clear();
            foreach (VMMC_Core.Document document in docList)
            {
                if (document.DocumentCode != null)
                {
                    if (document.DocumentCode != "")
                    {
                        VMMC_Core.DbObject docObj = new VMMC_Core.DbObject(sessionInfo)
                        {
                            ObjectId = document.DocumentId,
                            ObjectCode = document.DocumentCode,
                            ObjectName = document.DocumentName,
                            CreatedBy = sessionInfo.UserName,
                            LastModifiedBy = sessionInfo.UserName,

                        };

                        string[] items = document.DocumentCode.Replace("--", "-").Split('-');
                        if (items.Length > 7)
                        {
                            string[] newItem = new string[]
                            {
                                items[0],
                                items[1],
                                items[2],
                                items[3],
                                items[4] +"-"+items[5],
                                items[6],
                                items[7]
                            };
                            //if (newItem[4].IndexOf('(') > 0) newItem[4] = newItem[4].Remove(newItem[4].IndexOf('('));
                            //newItem[4] = newItem[4].Remove(newItem[4].IndexOf('('), newItem[4].Length - newItem[4].IndexOf('('));

                            items = newItem;
                        }


                        if (items.Length == 7)
                        {
                            VMMC_Core.TreeItem treeItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem("ELEMTREE", null);
                            if (items[1].Length == 2) items[1] = "00" + items[1];
                            VMMC_Core.TreeItem buildItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[1], treeItem.TreeItemId.ToString());
                            if (buildItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject buildObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = buildItem.TreeItemId,
                                    ObjectCode = buildItem.TreeItemCode,
                                    ObjectName = buildItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                    LeftObject = buildObj,
                                    RightObject = docObj
                                };
                                relationshipList.Add(relDoc);
                            }
                            else
                            {
                                if (items[1] == "") document.StatusInfo += "Шифр строения не обнаружен в коде. ";
                                else document.StatusInfo += "Строение " + items[1] + " не найдено. ";

                            }

                            string systemItemCode = items[4];
                            if (systemItemCode.IndexOf('-') > 0 & items[3] == "HV") systemItemCode = "SOV";
                            else if (systemItemCode.Length != 3 & items[3] == "HV") systemItemCode = "SOV";
                            //else { }

                            VMMC_Core.TreeItem systemItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(systemItemCode, buildItem.TreeItemId.ToString());
                            if (systemItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject systemObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = systemItem.TreeItemId,
                                    ObjectCode = systemItem.TreeItemCode,
                                    ObjectName = systemItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                    LeftObject = systemObj,
                                    RightObject = docObj

                                };
                                relationshipList.Add(relDoc);
                            }
                            else
                            {
                                if (items[4] == "") document.StatusInfo += "Шифр системы не обнаружен в коде. ";
                                else document.StatusInfo += "Система " + items[4] + " не найдена в родительском TreeItem " + buildItem.TreeItemCode + ". ";

                            }


                            VMMC_Core.TreeItem levelItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[2], systemItem.TreeItemId.ToString());
                            if (levelItem.TreeItemCode == null) levelItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem("В"/*кирилица*/+ items[2].Replace("B", ""), systemItem.TreeItemId.ToString()); //если в бд уровень на кирилице

                            if (levelItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject levelObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = levelItem.TreeItemId,
                                    ObjectCode = levelItem.TreeItemCode,
                                    ObjectName = levelItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                    LeftObject = levelObj,
                                    RightObject = docObj

                                };
                                relationshipList.Add(relDoc);
                            }
                            else
                            {
                                if (items[2] == "") document.StatusInfo += "Шифр отметки не обнаружен в коде. ";
                                else document.StatusInfo += "Отметка " + items[2] + " не найдена в родительском TreeItem " + systemItem.TreeItemCode + ". ";

                            }

                            if (items[4].IndexOf('-') > 0)
                            {
                                VMMC_Core.TreeItem subsystemItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[4], levelItem.TreeItemId.ToString());

                                if (subsystemItem.TreeItemCode != null)
                                {
                                    VMMC_Core.DbObject subsystemObj = new VMMC_Core.DbObject(sessionInfo)
                                    {
                                        ObjectId = subsystemItem.TreeItemId,
                                        ObjectCode = subsystemItem.TreeItemCode,
                                        ObjectName = subsystemItem.TreeItemName,
                                        CreatedBy = sessionInfo.UserName,
                                        LastModifiedBy = sessionInfo.UserName,

                                    };

                                    VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                    {
                                        RelationshipId = Guid.NewGuid(),
                                        RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                        LeftObject = subsystemObj,
                                        RightObject = docObj

                                    };
                                    relationshipList.Add(relDoc);
                                }
                                else
                                {
                                    if (items[2] == "") document.StatusInfo += "Шифр подсистемы не обнаружен в коде. ";
                                    else document.StatusInfo += "Подсистема " + items[4] + " не найдена в родительском TreeItem " + levelItem.TreeItemCode + ". ";

                                }
                            }
                        }
                        else document.StatusInfo += "Шифр документа отличается от маски. ";
                    }
                }
            }
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.RelationshipsCollection = relationshipList;

            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }


        private void GetDocuments3DDocumentsRdFromExcel()
        {
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
                        ExcelParserViewModel newExcelParserViewModel = GetDocuments3DDocumentsRdFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);
                    }
                }
            }
        }
        public ExcelParserViewModel GetDocuments3DDocumentsRdFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);

            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {

                    VMMC_Core.Document newDoc3D = new VMMC_Core.Document(sessionInfo);
                    newDoc3D.DocumentId = Guid.NewGuid();
                    newDoc3D.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("3D-Модель").ClassId;
                    newDoc3D.Revisions = new ObservableCollection<VMMC_Core.Revision> { new VMMC_Core.Revision(sessionInfo) { RevisionId = Guid.NewGuid(), DocumentId = newDoc3D.DocumentId, Number = 0, IsCurrent = true } };

                    VMMC_Core.Document newDocRD = new VMMC_Core.Document(sessionInfo);
                    newDocRD.DocumentId = Guid.NewGuid();
                    newDocRD.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("Рабочая документация").ClassId;
                    newDocRD.Revisions = new ObservableCollection<VMMC_Core.Revision>();


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "Шифр модели":
                                newDoc3D.DocumentCode = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Описание модели":
                                newDoc3D.DocumentName = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", ""); ;
                                break;

                            case "Файл - шифр":
                                newDocRD.DocumentCode = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Файл - имя":
                                newDocRD.DocumentName = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", ""); ;
                                break;

                            case "Изм":
                                string value = resultDT.Rows[i][j].ToString().Trim().Replace("\r", "\n").Replace("\n", ""); ;

                                int revNumber = 0;
                                int.TryParse(resultDT.Rows[i][j].ToString().Replace("\r", "\n").Replace("\n", "").Replace("Изм.", ""), out revNumber);
                                bool isCurRev = new VMMC_Core.Revision(newDocRD.sessionInfo).IsCurentRevision(newDocRD.DocumentId, revNumber);
                                newDocRD.Revisions = new ObservableCollection<VMMC_Core.Revision> { new VMMC_Core.Revision(sessionInfo) { RevisionId = Guid.NewGuid(), DocumentId = newDocRD.DocumentId, Number = revNumber, IsCurrent = isCurRev } };

                                break;

                            default:
                                break;
                        }
                    }
                    if (newDoc3D.DocumentCode != null && newDocRD.DocumentCode != null)
                    {
                        if (newDoc3D.DocumentCode != "" && newDocRD.DocumentCode != ""
                            && newDoc3D.DocumentCode != "-" && newDocRD.DocumentCode != "-")
                        {
                            docList.Add(newDoc3D);
                            docList.Add(newDocRD);

                            relList.Add(CreateRelDoc3DDocRD(newDoc3D, newDocRD));
                        }
                    }
                }
            }

            List<VMMC_Core.Document> newDocList = new List<VMMC_Core.Document>();
            foreach (VMMC_Core.Document doc in docList)
            {
                string[] docCodes = doc.DocumentCode.Split('\n');

                if (docCodes.Length > 1)
                {
                    doc.StatusInfo = doc.DocumentCode + "\n";

                    for (int i = 0; i < docCodes.Length; i++)
                    {
                        VMMC_Core.Document newDoc = new VMMC_Core.Document(sessionInfo);
                        newDoc.DocumentId = Guid.NewGuid();
                        newDoc.DocumentCode = docCodes[i];
                        newDoc.DocumentClassId = doc.DocumentClassId;
                        newDoc.Revisions = doc.Revisions;
                        newDoc.StatusInfo = "new";
                        doc.StatusInfo += docCodes[i] + "\n";

                        if (i == 0)
                            doc.DocumentCode = docCodes[i];
                        else
                        {
                            if (newDoc.DocumentCode.ToString() != "") newDocList.Add(newDoc);
                            ObservableCollection<VMMC_Core.Relationship> newRelList = RelNewDocList(doc, newDoc, relList);
                            foreach (VMMC_Core.Relationship newRel in newRelList) relList.Add(newRel);
                        }
                    }
                }
            }

            foreach (VMMC_Core.Document doc in docList)
            {
                if (doc.DocumentClassId == new VMMC_Core.Class(sessionInfo).getClass("Рабочая документация").ClassId) { }

                int startDocNumber = 0;
                int endDocNumber = 0;

                string[] docCodes = doc.DocumentCode.Split('-');
                int.TryParse(docCodes[docCodes.Length - 1], out endDocNumber);
                if (endDocNumber == 0) docCodes = doc.DocumentCode.Replace("...", "-").Replace("_", "-").Split('-');
                int.TryParse(docCodes[docCodes.Length - 1], out endDocNumber);

                if (endDocNumber == 0)
                {
                    string[] docNumbers = docCodes[docCodes.Length - 1].Replace(" ", "").Split(',');
                    if (docNumbers.Length > 1)
                    {
                        string patern = doc.DocumentCode.Replace("...", "-").Replace("_", "-").Replace("-" + docCodes[docCodes.Length - 1], "");
                        string strFormant = "";
                        doc.StatusInfo += doc.DocumentCode + "\n";
                        doc.StatusInfo += docCodes[docCodes.Length - 1] + "\n";
                        for (int i = 0; i < docNumbers[0].Length; i++)
                        {
                            strFormant += "0";
                        }

                        for (int i = 0; i < docNumbers.Length; i++)
                        {
                            VMMC_Core.Document newDocRD = new VMMC_Core.Document(sessionInfo);
                            newDocRD.DocumentId = Guid.NewGuid();
                            newDocRD.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("Рабочая документация").ClassId;
                            newDocRD.Revisions = doc.Revisions;
                            newDocRD.StatusInfo = "new";

                            int number = 0;
                            int.TryParse(docNumbers[i], out number);
                            doc.StatusInfo += patern + "-" + number.ToString(strFormant) + "\n";

                            newDocRD.DocumentCode = patern + "-" + number.ToString(strFormant);

                            if (i == 0) doc.DocumentCode = patern + "-" + number.ToString(strFormant);
                            else
                            {
                                if (newDocRD.DocumentCode.ToString() != "") newDocList.Add(newDocRD);

                                ObservableCollection<VMMC_Core.Relationship> newRelList = RelNewDocList(doc, newDocRD, relList);
                                foreach (VMMC_Core.Relationship newRel in newRelList) relList.Add(newRel);
                                {/*add relationships*/}
                            }

                        }
                    }
                    else doc.StatusInfo = "Не удалось распознать номерную часть";
                }
                else
                {
                    int.TryParse(docCodes[docCodes.Length - 2], out startDocNumber);
                    if (startDocNumber == 0) { }
                    else
                    {
                        doc.StatusInfo += doc.DocumentCode + "\n";
                        doc.StatusInfo += docCodes[docCodes.Length - 2] + "-" + docCodes[docCodes.Length - 1] + "\n";
                        string patern = doc.DocumentCode.Replace("...", "-").Replace("_", "-").Replace("-" + docCodes[docCodes.Length - 2] + "-" + docCodes[docCodes.Length - 1], "");
                        string strFormant = "";
                        for (int i = 0; i < docCodes[docCodes.Length - 2].Length; i++)
                        {
                            strFormant += "0";
                        }
                        for (int i = startDocNumber; i <= endDocNumber; i++)
                        {
                            VMMC_Core.Document newDocRD = new VMMC_Core.Document(sessionInfo);
                            newDocRD.DocumentId = Guid.NewGuid();
                            newDocRD.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("Рабочая документация").ClassId;
                            newDocRD.Revisions = doc.Revisions;
                            newDocRD.StatusInfo = "new";
                            newDocRD.DocumentCode = patern + "-" + i.ToString(strFormant);

                            if (i == startDocNumber)
                                doc.DocumentCode = patern + "-" + i.ToString(strFormant);
                            else
                            {
                                if (newDocRD.DocumentCode.ToString() != "") newDocList.Add(newDocRD);

                                ObservableCollection<VMMC_Core.Relationship> newRelList = RelNewDocList(doc, newDocRD, relList);
                                foreach (VMMC_Core.Relationship newRel in newRelList) relList.Add(newRel);
                                {/*add relationships*/}
                            }
                            //doc.StatusInfo += string.Format(strFormant, i);
                            doc.StatusInfo += patern + "-" + i.ToString(strFormant) + "\n";
                        }
                        //doc.DocumentCode = doc.DocumentCode.Replace("-"+ docCodes[docCodes.Length - 1], "");
                        //doc.DocumentCode = patern + "-" + startDocNumber.ToString(strFormant);
                    }
                }
            }
            foreach (VMMC_Core.Document newDoc in newDocList)
            {
                docList.Add(newDoc);

            }
            //------------------------------------------------------------------------------------------------------------------------
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.RelationshipsCollection = relList;

            ExcelParserViewModel result = UnionAllDocumentInListByCode(excelParserViewModel);
            docList = result.DocumentsCollection;
            relList = result.RelationshipsCollection;

            ObservableCollection<VMMC_Core.Document> dbDocumentsCollection = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
            foreach (VMMC_Core.Document doc in docList)
            {
                VMMC_Core.Document dbDoc = dbDocumentsCollection.Where(x => x.DocumentCode == doc.DocumentCode).FirstOrDefault();
                if (dbDoc != null)
                {
                    dbDoc.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(dbDoc.DocumentId);
                    foreach (VMMC_Core.Revision rev in doc.Revisions)
                    {
                        VMMC_Core.Revision newRev = dbDoc.Revisions.Where(x => x.Number == rev.Number).FirstOrDefault();
                        rev.DocumentId = dbDoc.DocumentId;
                        if (newRev == null) dbDoc.Revisions.Add(rev);
                        //else doc.Revisions.Add(newRev);
                    }

                    relList = ReplacedocId(doc, dbDoc, relList);

                    //localFile.Document = dbDoc;
                    doc.DocumentId = dbDoc.DocumentId;
                    doc.DocumentCode = dbDoc.DocumentCode;
                    doc.DocumentName = dbDoc.DocumentName;
                    doc.DocumentClassId = dbDoc.DocumentClassId;
                    doc.Revisions = dbDoc.Revisions;
                    //doc.Status = FileName;
                    doc.StatusInfo = "Документ уже существует в БД";
                    doc.IsExistInDB = true;
                }
            }

            ObservableCollection<VMMC_Core.Relationship> cleanRelList = new ObservableCollection<VMMC_Core.Relationship>();
            foreach (VMMC_Core.Relationship rel in relList)
            {
                VMMC_Core.Relationship existRel = cleanRelList.Where(x => x.RightObjectId == rel.RightObjectId && x.LeftObjectId == rel.LeftObjectId || x.RightObject.ObjectId == rel.RightObject.ObjectId && x.LeftObject.ObjectId == rel.LeftObject.ObjectId).FirstOrDefault();
                if (existRel == null) cleanRelList.Add(rel);
            }


            relList = cleanRelList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.RelationshipsCollection = relList;

            return excelParserViewModel;

        }


        private void GetDocuments3DTagFromExcel()
        {
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
                        ExcelParserViewModel newExcelParserViewModel = GetDocuments3DTagFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);
                    }
                }
            }
        }
        public ExcelParserViewModel GetDocuments3DTagFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    VMMC_Core.Document newDoc = new VMMC_Core.Document(sessionInfo);
                    newDoc.DocumentId = Guid.NewGuid();
                    newDoc.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("3D-Модель").ClassId;
                    newDoc.Status = new FileInfo(FileName).Name;

                    VMMC_Core.Revision newRev = new VMMC_Core.Revision(sessionInfo)
                    {
                        RevisionId = Guid.NewGuid(),
                        DocumentId = newDoc.DocumentId,
                        Number = 0,
                        IsCurrent = true
                    };
                    newDoc.Revisions = new ObservableCollection<VMMC_Core.Revision> { newRev };

                    VMMC_Core.Tag newTag = new VMMC_Core.Tag(sessionInfo);
                    newTag.TagId = Guid.NewGuid();


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "Модель":
                                newDoc.DocumentCode = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Категория":
                                newTag.Position = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "AWName":
                                newTag.TagName = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", "");
                                break;

                            case "Тип элемента":
                                newTag.TagClassId = new VMMC_Core.Class(sessionInfo).getClass(resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", "")).ClassId;
                                if (newTag.TagClassId == Guid.Parse("00000000-0000-0000-0000-000000000000")) newTag.StatusInfo = "Тип элемента '" + resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim().Replace("\n", "") + "' не найден в БД";
                                break;

                            default:
                                break;
                        }
                    }
                    if (newDoc.DocumentCode != null)
                    {
                        if (newDoc.DocumentCode != "") docList.Add(newDoc);
                    }
                    if (newTag.Position != null)
                    {
                        if (newTag.Position != "") tagList.Add(newTag);
                    }

                    if (newDoc.DocumentCode != null && newTag.Position != null)
                    {
                        if (newDoc.DocumentCode != "" && newTag.Position != ""
                            && newDoc.DocumentCode != "-" && newTag.Position != "-")
                        {
                            docList.Add(newDoc);
                            tagList.Add(newTag);

                            relList.Add(CreateRelTagDoc(newTag, newDoc));
                        }
                    }
                }
            }

            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            ExcelParserViewModel result = UnionAllDocumentInListByCode(excelParserViewModel);
            result = UnionAllTagInListByCode(result);
            docList = result.DocumentsCollection;
            tagList = result.TagsCollection;
            relList = result.RelationshipsCollection;

            ObservableCollection<VMMC_Core.Document> dbDocumentsCollection = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
            foreach (VMMC_Core.Document doc in docList)
            {
                VMMC_Core.Document dbDoc = dbDocumentsCollection.Where(x => x.DocumentCode == doc.DocumentCode).FirstOrDefault();
                if (dbDoc != null)
                {
                    dbDoc.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(dbDoc.DocumentId);
                    relList = ReplacedocId(doc, dbDoc, relList);
                    //localFile.Document = dbDoc;
                    doc.DocumentId = dbDoc.DocumentId;
                    doc.DocumentCode = dbDoc.DocumentCode;
                    doc.DocumentName = dbDoc.DocumentName;
                    doc.DocumentClassId = dbDoc.DocumentClassId;
                    doc.Revisions = dbDoc.Revisions;
                    //doc.Status = FileName;
                    doc.StatusInfo = "Документ уже существует в БД";
                    doc.IsExistInDB = true;
                }
            }
            List<VMMC_Core.Tag> dbTagsCollection = new VMMC_Core.Tag(sessionInfo).GetDbTagsList();
            foreach (VMMC_Core.Tag tag in tagList)
            {
                VMMC_Core.Tag dbTag = dbTagsCollection.Where(x => x.Position == tag.Position).FirstOrDefault();
                if (dbTag != null)
                {
                    relList = ReplaceTagId(tag, dbTag, relList);
                    //localFile.Document = dbDoc;
                    tag.TagId = dbTag.TagId;
                    tag.Position = dbTag.Position;
                    tag.TagName = dbTag.TagName;
                    tag.Characteristic = dbTag.Characteristic;
                    tag.TreeItemId = dbTag.TreeItemId;
                    tag.TagClassId = dbTag.TagClassId;
                    //doc.Status = FileName;
                    tag.StatusInfo = "Тег уже существует в БД";
                    tag.IsExistInDB = true;
                }
            }
            //List<VMMC_Core.Relationship> newDocTreeRelList = GetRelFrom3D(docList);
            ObservableCollection<VMMC_Core.Relationship> newDocTreeRelList = new ObservableCollection<VMMC_Core.Relationship>();
            foreach (VMMC_Core.Document document in docList)
            {
                if (document.DocumentCode != null)
                {
                    if (document.DocumentCode != "")
                    {
                        VMMC_Core.DbObject docObj = new VMMC_Core.DbObject(sessionInfo)
                        {
                            ObjectId = document.DocumentId,
                            ObjectCode = document.DocumentCode,
                            ObjectName = document.DocumentName,
                            CreatedBy = sessionInfo.UserName,
                            LastModifiedBy = sessionInfo.UserName,
                        };
                        newDocTreeRelList = getRelationshipFromDoc3DCode(document.DocumentCode, docObj, new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"));
                    }
                }
            }





            List<VMMC_Core.Relationship> newRelList = new List<VMMC_Core.Relationship>();

            foreach (VMMC_Core.Relationship docTreeRel in newDocTreeRelList)
            {

                IEnumerable<VMMC_Core.Relationship> tagDocRelList = relList.Where(x => x.RightObjectId == docTreeRel.RightObjectId || x.RightObject.ObjectId == docTreeRel.RightObject.ObjectId);
                if (tagDocRelList == null)
                {
                    int g = 0;
                }
                foreach (VMMC_Core.Relationship tagDocRel in tagDocRelList)
                {
                    VMMC_Core.Relationship newRel = new VMMC_Core.Relationship(sessionInfo)
                    {
                        RelationshipId = Guid.NewGuid(),
                        RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Тег"),
                        LeftObjectId = docTreeRel.LeftObjectId,
                        LeftObject = docTreeRel.LeftObject,
                        RightObjectId = tagDocRel.LeftObjectId,
                        RightObject = tagDocRel.LeftObject
                    };
                    newRelList.Add(newRel);
                }
            }
            foreach (VMMC_Core.Relationship tagRel in newRelList)
            {
                relList.Add(tagRel);
            }
            newRelList = new List<VMMC_Core.Relationship>();
            foreach (VMMC_Core.Relationship rel in relList)
            {
                VMMC_Core.Relationship existRel = newRelList.Where(x => x.RightObjectId == rel.RightObjectId && x.LeftObjectId == rel.LeftObjectId || x.RightObject.ObjectId == rel.RightObject.ObjectId && x.LeftObject.ObjectId == rel.LeftObject.ObjectId).FirstOrDefault();
                if (existRel == null) newRelList.Add(rel);
            }
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;

            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }


        private void GetComplektsWithRevisionsList()
        {
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
                        ExcelParserViewModel newExcelParserViewModel = GetComplektsWithRevisionsList(fileName);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);
                    }
                }
            }
        }
        public ExcelParserViewModel GetComplektsWithRevisionsList(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Complekt> complectsList = new ObservableCollection<VMMC_Core.Complekt>();

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    VMMC_Core.Complekt newComplekt = new VMMC_Core.Complekt(sessionInfo);
                    newComplekt.ComplektId = Guid.NewGuid();
                    newComplekt.ComplektClassId = new VMMC_Core.Class(sessionInfo).getClass("Комплект").ClassId;

                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "Здание":
                                newComplekt.Info = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Шифр раздела":
                                newComplekt.ComplektCode = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Наименование":
                                newComplekt.ComplektName = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Изм":
                                newComplekt.Status = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                if (!newComplekt.Status.Contains("Изм.") && !newComplekt.Status.Contains("дубликат") && newComplekt.Status!="") newComplekt.Status = "Изм. " + newComplekt.Status;
                                break;

                            case "Файлы":
                                newComplekt.StatusInfo = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }
                    if (newComplekt.ComplektCode != null)
                    {
                        if (newComplekt.ComplektCode != "") complectsList.Add(newComplekt);
                    }                    
                }
            }

            ObservableCollection<VMMC_Core.Complekt> resultComplectsList = new ObservableCollection<VMMC_Core.Complekt>();
            foreach (VMMC_Core.Complekt complekt in complectsList)
            {
                if (complekt.Status.Contains("Изм.")) 
                {
                    int revNo = int.Parse(complekt.Status.Replace("Изм.", "").Trim());

                    for (int i = revNo; i >= 0; i--)
                    {
                        VMMC_Core.Complekt newComplekt = new VMMC_Core.Complekt(sessionInfo);
                        newComplekt.ComplektId = complekt.ComplektId;
                        newComplekt.ComplektCode = complekt.ComplektCode;
                        newComplekt.ComplektName = complekt.ComplektName;
                        newComplekt.StatusInfo = complekt.StatusInfo;
                        newComplekt.Info = complekt.Info;
                        newComplekt.ComplektClassId = new VMMC_Core.Class(sessionInfo).getClass("Комплект").ClassId;
                        newComplekt.Status = "Изм. " + i.ToString();

                        VMMC_Core.Complekt existComplekt = resultComplectsList.Where(x => x.ComplektCode == newComplekt.ComplektCode && x.Status == newComplekt.Status).FirstOrDefault();
                        if (existComplekt == null) resultComplectsList.Add(newComplekt);
                    }

                }
                else resultComplectsList.Add(complekt);
            }

            excelParserViewModel.ComplektsCollection = complectsList;
            excelParserViewModel.ComplektsCollection = resultComplectsList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }


        private void GetDocumentsRdTagsRelationshipsFromExcel()
        {
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
                        ExcelParserViewModel newExcelParserViewModel = GetDocumentsRdTagsRelationshipsFromExcel(fileName);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);
                    }
                }
            }
        }
        public ExcelParserViewModel GetDocumentsRdTagsRelationshipsFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);

            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    VMMC_Core.Document newDocRD = new VMMC_Core.Document(sessionInfo);
                    newDocRD.DocumentId = Guid.NewGuid();
                    newDocRD.DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("Рабочая документация").ClassId;
                    newDocRD.Revisions = new ObservableCollection<VMMC_Core.Revision>();

                    List<VMMC_Core.Tag> tagList = new List<VMMC_Core.Tag>();


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "Документ РД":
                                newDocRD = newDocRD.GetDocument(resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim());
                                break;

                            case "Код отделения ":
                                string value = resultDT.Rows[i][j].ToString().Trim().Replace("\r", "\n").Replace("\n", ""); ;

                                string sql_query = @"SELECT [Id], [Position], [Name], [Characteristic], [TreeItemId], [ClassId]
  FROM [dbo].[Tags]
  WHERE [ClassId] not in (SELECT [Id] FROM [dbo].[Classes] where [ClassName] in ( 'Стена', 'Подвесной потолок' )) AND [Id] in (SELECT [RightObjectId] FROM [dbo].[Relationships] WHERE [RelTypeId] in (SELECT [Id] FROM [dbo].[RelationshipTypes] WHERE [Code] in ('Отметка-Тэг', 'Элемент дерева-Тег')) and [LeftObjectId] in (SELECT [Id] FROM [dbo].[TreeItems] WHERE [TreeItemCode] =  '" + value + "') ) "+
  @"UNION ALL
  SELECT [Id], [Position], [Name], [Characteristic], [TreeItemId], [ClassId]
  FROM [dbo].[Tags]
  WHERE [ClassId] not in (SELECT [Id] FROM [dbo].[Classes] where [ClassName] in ( 'Стена', 'Подвесной потолок' )) AND [Id] in (SELECT [RightObjectId] FROM [dbo].[Relationships] WHERE [RelTypeId] in (SELECT [Id] FROM [dbo].[RelationshipTypes] WHERE [Code] in ('Отметка-Тэг', 'Элемент дерева-Тег')) and [LeftObjectId] in (SELECT [Id] FROM [dbo].[TreeItems] WHERE [ParentId] in (SELECT [Id] FROM [dbo].[TreeItems] WHERE [TreeItemCode] =  '" + value + "')) )";
                                
                                tagList = new VMMC_Core.Tag(sessionInfo).GetDbTagsListFromQuery(sql_query);
                                //docList.Add(newDocRD);
                                foreach (VMMC_Core.Tag tag in tagList)
                                {
                                    tag.StatusInfo = value;
                                }
                                
                               
                                break;

                            default:
                                break;
                        }
                    }

                    
                    if (newDocRD.DocumentCode != null && tagList.Count>0)
                    {
                        if (newDocRD.DocumentCode != "" && newDocRD.DocumentCode != "-")
                        {                            
                            //docList.Add(newDocRD);
                            foreach (VMMC_Core.Tag tag in tagList)
                            {
                                VMMC_Core.Relationship newRel = CreateRelTagDocRD(tag, newDocRD);
                                newRel.StatusInfo = tag.StatusInfo;
                                relList.Add(newRel);
                            }
                            
                            
                        }
                    }
                }
            }          
            
            
            excelParserViewModel.RelationshipsCollection = relList;

            return excelParserViewModel;

        }


        private void GetDocumentsRdFromMDRExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetDocumentsRdFromMDRExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {                            
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetDocumentsRdFromMDRExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    string documentCodeStr = ""; //Шифр КМН (Code)+
                    string documentNameStr = ""; //DocName_ru (Name)+

                    string codeVNPStr = ""; //Шифр ВНП /*[AttributeName] = 'Шифр ВНП'*/+

                    string documentTypeStr1 = ""; //9_DocType +
                    string documentTypeStr2 = ""; //Тип документа +
                    string documentTypeStr = documentTypeStr1 + " - " + documentTypeStr2; // /*[AttributeName] = 'Тип документа'*/+

                    string markCodeStr1 = ""; //8_Mark +
                    string markCodeStr2 = ""; //Mark_RU +
                    string markCodeStr = markCodeStr1 + " - " + markCodeStr2; // /*[AttributeName] = 'Марка документа РД'*/+

                    string buildStr1 = ""; //6_Col6_No +
                    string buildStr2 = ""; //Монтажный блок +
                    string buildStr = buildStr1 + " - " + buildStr2; // /*[AttributeName] = 'Монтажный блок'*/+

                    string complexCodeStr = ""; //Комплекс
                    string systemCodeStr = ""; //Номер системы
                    string systemNameStr = ""; //Система (Tree Item)


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "Шифр КМН":
                                documentCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "DocNo_Заказчика":
                                documentCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "DocName_ru":
                                documentNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "9_DocType":
                                documentTypeStr1 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Тип документа":
                                documentTypeStr2 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "DocType_RU":
                                documentTypeStr2 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Шифр ВНП":
                                codeVNPStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "DocNo_выч":
                                codeVNPStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "6_Col6_No":
                                buildStr1 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Монтажный блок":
                                buildStr2 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Марка":
                                markCodeStr1 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "8_Mark":
                                markCodeStr1 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Mark_RU":
                                markCodeStr2 = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Комплекс":
                                complexCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "ComplexNo":
                                complexCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Номер системы":
                                systemCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "SystemNo":
                                systemCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "Система":
                                systemNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "SystemName":
                                systemNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }

                    VMMC_Core.Document newDoc = new VMMC_Core.Document(sessionInfo).GetDocument(documentCodeStr);
                    if (newDoc == null)
                    {
                        newDoc = new VMMC_Core.Document(sessionInfo)
                        {
                            DocumentId = Guid.NewGuid(),
                            DocumentCode = documentCodeStr,
                            DocumentName = documentNameStr,
                            DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass("Рабочая документация").ClassId
                        };
                    }
                    docList.Add(newDoc);



                    VMMC_Core.AttributeObjectValue codeVNP = new AttributeObjectValue(sessionInfo) { 
                        AttributeObjectValueId = Guid.NewGuid(), 
                        Attribute = new VMMC_Core.Attribute(sessionInfo).SearchAttribute("Шифр ВНП"), 
                        Object = new DbObject(sessionInfo) { ObjectId = newDoc.DocumentId }, 
                        StringValue = codeVNPStr
                    };

                    atrList.Add(codeVNP);



                    documentTypeStr = documentTypeStr1 + " - " + documentTypeStr2;

                    VMMC_Core.AttributeObjectValue documentType = new AttributeObjectValue(sessionInfo) { 
                        AttributeObjectValueId = Guid.NewGuid(), 
                        Attribute = new VMMC_Core.Attribute(sessionInfo).SearchAttribute("Тип документа"), 
                        Object = new DbObject(sessionInfo) { ObjectId = newDoc.DocumentId }, 
                    };
                    VMMC_Core.EnumObjectValue documentTypeEnumObjectValue = new EnumObjectValue(sessionInfo) 
                    {
                        EnumObjectValueId = Guid.NewGuid(),
                        AttributeObjectValue = documentType,
                        EnumAttributeValue = new EnumAttributeValue(sessionInfo).GetEnumAttributeValue(documentType.Attribute.AttributeId, documentTypeStr)
                    };
                    if (documentTypeEnumObjectValue.EnumAttributeValue != null)
                    {
                        documentType.EnumObjectValue = documentTypeEnumObjectValue;
                        documentType.AvailibleEnumAttributeValueList = new EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(documentType.Attribute.AttributeId);
                        documentType.AvailibleValuesList = new EnumObjectValue(sessionInfo).GetAvailibleEnumObjectValuesList(documentType.Attribute.AttributeId);
                        atrList.Add(documentType);
                    }


                    buildStr = buildStr1.Substring(2) + " - " + buildStr2;

                    VMMC_Core.AttributeObjectValue build = new AttributeObjectValue(sessionInfo) { 
                        AttributeObjectValueId = Guid.NewGuid(), 
                        Attribute = new VMMC_Core.Attribute(sessionInfo).SearchAttribute("Монтажный блок"), 
                        Object = new DbObject(sessionInfo) { ObjectId = newDoc.DocumentId },
                    };
                    VMMC_Core.EnumObjectValue buildEnumObjectValue = new EnumObjectValue(sessionInfo)
                    {
                        EnumObjectValueId = Guid.NewGuid(),
                        AttributeObjectValue = build,
                        EnumAttributeValue = new EnumAttributeValue(sessionInfo).GetEnumAttributeValue(build.Attribute.AttributeId, buildStr)
                    };
                    if (buildEnumObjectValue.EnumAttributeValue != null)
                    {
                        build.EnumObjectValue = buildEnumObjectValue;
                        build.AvailibleEnumAttributeValueList = new EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(build.Attribute.AttributeId);
                        build.AvailibleValuesList = new EnumObjectValue(sessionInfo).GetAvailibleEnumObjectValuesList(build.Attribute.AttributeId);
                        atrList.Add(build);
                    }

                    markCodeStr = markCodeStr1 + " - " + markCodeStr2;
                    VMMC_Core.AttributeObjectValue markCode = new AttributeObjectValue(sessionInfo) { 
                        AttributeObjectValueId = Guid.NewGuid(), 
                        Attribute = new VMMC_Core.Attribute(sessionInfo).SearchAttribute("Марка документа РД"), 
                        Object = new DbObject(sessionInfo) { ObjectId = newDoc.DocumentId }, 
                    };
                    VMMC_Core.EnumObjectValue markCodeEnumObjectValue = new EnumObjectValue(sessionInfo)
                    {
                        EnumObjectValueId = Guid.NewGuid(),
                        AttributeObjectValue = markCode,
                        EnumAttributeValue = new EnumAttributeValue(sessionInfo).GetEnumAttributeValue(markCode.Attribute.AttributeId, markCodeStr)
                    };
                    if (markCodeEnumObjectValue.EnumAttributeValue != null)
                    {
                        markCode.EnumObjectValue = markCodeEnumObjectValue;
                        markCode.AvailibleEnumAttributeValueList = new EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(markCode.Attribute.AttributeId);
                        markCode.AvailibleValuesList = new EnumObjectValue(sessionInfo).GetAvailibleEnumObjectValuesList(markCode.Attribute.AttributeId);
                        atrList.Add(markCode);

                    }
                    VMMC_Core.TreeItem doctreeTreeItem = new TreeItem(sessionInfo).getTreeItem("ELEMDOC", null);

                    VMMC_Core.TreeItem rdTreeItem = new TreeItem(sessionInfo).getTreeItem("РД", doctreeTreeItem.TreeItemId.ToString());
                    if (rdTreeItem.TreeItemCode == null)
                    {
                        rdTreeItem.TreeItemId = Guid.NewGuid();
                        rdTreeItem.TreeItemCode = "РД";
                        rdTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("TREEITEM");
                        rdTreeItem.Parent = rdTreeItem;
                        treeItemsList.Add(rdTreeItem);
                    }

                    VMMC_Core.TreeItem buildingTreeItem = new TreeItem(sessionInfo).getTreeItem("10", rdTreeItem.TreeItemId.ToString());
                    if (buildingTreeItem.TreeItemCode == null)
                    {
                        buildingTreeItem.TreeItemId = Guid.NewGuid();
                        buildingTreeItem.TreeItemCode = "10";
                        buildingTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("BUILD");
                        buildingTreeItem.Parent = buildingTreeItem;
                        treeItemsList.Add(buildingTreeItem);
                    }

                    VMMC_Core.TreeItem complexTreeItem = new TreeItem(sessionInfo).getTreeItem(complexCodeStr, buildingTreeItem.TreeItemId.ToString());                    
                    if (complexTreeItem == null)
                    {
                        complexTreeItem = new TreeItem(sessionInfo);
                        complexTreeItem.TreeItemId = Guid.NewGuid();
                        complexTreeItem.TreeItemCode = complexCodeStr;
                        complexTreeItem.TreeItemName = complexCodeStr;
                        complexTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("COMPLEX");
                        complexTreeItem.Parent = buildingTreeItem;
                        treeItemsList.Add(complexTreeItem);
                    }

                    VMMC_Core.TreeItem systemTreeItem = new TreeItem(sessionInfo).getTreeItem(systemCodeStr, complexTreeItem.TreeItemId.ToString());
                    if (systemTreeItem == null)
                    {
                        systemTreeItem = new TreeItem(sessionInfo);
                        systemTreeItem.TreeItemId = Guid.NewGuid();
                        systemTreeItem.TreeItemCode = systemCodeStr;
                        systemTreeItem.TreeItemName = systemNameStr;
                        systemTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("SYSTEM");
                        systemTreeItem.Parent = complexTreeItem;
                        treeItemsList.Add(systemTreeItem);
                    }

                    VMMC_Core.Relationship docSystem = new Relationship(sessionInfo) 
                    { 
                        RelationshipId = Guid.NewGuid(),
                        LeftObject=new DbObject(sessionInfo) { ObjectId = systemTreeItem.TreeItemId},
                        RightObject = new DbObject(sessionInfo) { ObjectId = newDoc.DocumentId},
                        RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ")
                    };
                    relList.Add(docSystem);


                }
            }

            ObservableCollection<VMMC_Core.TreeItem> sortTreeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            foreach (VMMC_Core.TreeItem ti in treeItemsList)
            {
                VMMC_Core.TreeItem existti = sortTreeItemsList.Where(x => x.TreeItemCode == ti.TreeItemCode && x.Parent.TreeItemId == ti.Parent.TreeItemId).FirstOrDefault();
                if (existti == null) sortTreeItemsList.Add(ti);
                else 
                {

                }

            }
            treeItemsList = sortTreeItemsList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;

            ExcelParserViewModel result = UnionAllDocumentInListByCode(excelParserViewModel);
            docList = result.DocumentsCollection;

            ObservableCollection<VMMC_Core.Document> dbDocumentsCollection = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
            foreach (VMMC_Core.Document doc in docList)
            {
                VMMC_Core.Document dbDoc = dbDocumentsCollection.Where(x => x.DocumentCode == doc.DocumentCode).FirstOrDefault();
                if (dbDoc != null)
                {
                    dbDoc.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(dbDoc.DocumentId);
                    relList = ReplacedocId(doc, dbDoc, relList);
                    //localFile.Document = dbDoc;
                    doc.DocumentId = dbDoc.DocumentId;
                    doc.DocumentCode = dbDoc.DocumentCode;
                    doc.DocumentName = dbDoc.DocumentName;
                    doc.DocumentClassId = dbDoc.DocumentClassId;
                    doc.Revisions = dbDoc.Revisions;
                    //doc.Status = FileName;
                    doc.StatusInfo = "Документ уже существует в БД";
                    doc.IsExistInDB = true;
                }
            }

            excelParserViewModel.DocumentsCollection = docList;

            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }


        private void GetBKFILLTagsFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLTagsFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLTagsFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    string tagCodeStr = ""; //
                    string tagNameStr = ""; //

                    string tagClassCodeStr = ""; //

                    string systemCodeStr = ""; //
                    string systemNameStr = ""; //
                    string treeItemCodeStr = ""; //
                    string treeItemNameStr = ""; //




                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "TagPosition":
                                tagCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "TagName":
                                tagNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "TagClassCode":
                                tagClassCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "SystemCode":
                                systemCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "SystemName":
                                systemNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "TreeItemCode":
                                treeItemCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "TreeItemName":
                                treeItemNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }

                   

                    VMMC_Core.Tag newTag = new VMMC_Core.Tag(sessionInfo).GetTag(tagCodeStr);
                    if (newTag == null)
                    {
                        newTag = new VMMC_Core.Tag(sessionInfo)
                        {
                            TagId = Guid.NewGuid(),
                            Position = tagCodeStr,
                            TagName = tagNameStr,
                            TagClassId = new VMMC_Core.Class(sessionInfo).getClass(tagClassCodeStr).ClassId
                            
                        };
                    }
                    tagList.Add(newTag);
                   
                    VMMC_Core.TreeItem tagtreeTreeItem = new TreeItem(sessionInfo).getTreeItem("ELEMTREE", null);

                    //VMMC_Core.TreeItem projectTreeItem = new TreeItem(sessionInfo).getTreeItem("4550.70", tagtreeTreeItem.TreeItemId.ToString());
                    //if (projectTreeItem.TreeItemCode == null)
                    //{
                    //    projectTreeItem.TreeItemId = Guid.NewGuid();
                    //    projectTreeItem.TreeItemCode = complexCodeStr;
                    //    projectTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("TREEITEM");
                    //    projectTreeItem.Parent = tagtreeTreeItem;
                    //    treeItemsList.Add(projectTreeItem);
                    //}

                    VMMC_Core.TreeItem buildingTreeItem = new TreeItem(sessionInfo).getTreeItem("4550.70", tagtreeTreeItem.TreeItemId.ToString());
                    if (buildingTreeItem.TreeItemCode == null)
                    {
                        buildingTreeItem.TreeItemId = Guid.NewGuid();
                        buildingTreeItem.TreeItemCode = "4550.70";
                        buildingTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("BUILD");
                        buildingTreeItem.Parent = tagtreeTreeItem;
                        treeItemsList.Add(buildingTreeItem);
                    }

                    string complexStr = systemCodeStr.Substring(8, 3);
                    string systemStr = systemCodeStr.Substring(12, 4);
                    VMMC_Core.TreeItem complexTreeItem = new TreeItem(sessionInfo).getTreeItem(systemCodeStr.Substring(8, 3), buildingTreeItem.TreeItemId.ToString());
                    if (complexTreeItem == null)
                    {
                        complexTreeItem = new TreeItem(sessionInfo);

                        complexTreeItem.TreeItemId = Guid.NewGuid();
                        complexTreeItem.TreeItemCode = systemCodeStr.Substring(8, 3);
                        complexTreeItem.TreeItemName = systemCodeStr.Substring(8, 3);
                        complexTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("COMPLEX");
                        complexTreeItem.Parent = buildingTreeItem;
                        treeItemsList.Add(complexTreeItem);
                    }

                    VMMC_Core.TreeItem systemTreeItem = new TreeItem(sessionInfo).getTreeItem(systemCodeStr.Substring(12, 4), complexTreeItem.TreeItemId.ToString());
                    if (systemTreeItem == null)
                    {
                        systemTreeItem = new TreeItem(sessionInfo);

                        systemTreeItem.TreeItemId = Guid.NewGuid();
                        systemTreeItem.TreeItemCode = systemCodeStr.Substring(12, 4);
                        systemTreeItem.TreeItemName = systemNameStr;
                        systemTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("SYSTEM");
                        systemTreeItem.Parent = complexTreeItem;
                        treeItemsList.Add(systemTreeItem);
                    }

                    VMMC_Core.Relationship tagSystem = new Relationship(sessionInfo)
                    {
                        RelationshipId = Guid.NewGuid(),
                        LeftObject = new DbObject(sessionInfo) { ObjectId = systemTreeItem.TreeItemId },
                        RightObject = new DbObject(sessionInfo) { ObjectId = newTag.TagId },
                        RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Система-Тэг")
                    };
                    relList.Add(tagSystem);


                }
            }

            ObservableCollection<VMMC_Core.TreeItem> sortTreeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            foreach (VMMC_Core.TreeItem ti in treeItemsList)
            {
                VMMC_Core.TreeItem existti = sortTreeItemsList.Where(x => x.TreeItemCode == ti.TreeItemCode && x.Parent.TreeItemId == ti.Parent.TreeItemId).FirstOrDefault();
                if (existti == null) sortTreeItemsList.Add(ti);
                else
                {

                }

            }
            treeItemsList = sortTreeItemsList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLTagTagRelFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLTagTagRelFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLTagTagRelFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    string leftTagCodeStr = ""; //
                    string rightTagCodeStr = ""; //

                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "LTagCode":
                                leftTagCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "RTagCode":
                                rightTagCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }

                    VMMC_Core.Tag leftTag = new VMMC_Core.Tag(sessionInfo).GetTag(leftTagCodeStr);
                    if (leftTag != null)
                    {
                        VMMC_Core.Tag rightTag = new VMMC_Core.Tag(sessionInfo).GetTag(rightTagCodeStr);
                        if (rightTag != null)
                        {
                            VMMC_Core.Relationship tagTag = new Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                LeftObject = new DbObject(sessionInfo) { ObjectId = leftTag.TagId },
                                RightObject = new DbObject(sessionInfo) { ObjectId = rightTag.TagId },
                                RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Тег-Тэг")
                            };
                            relList.Add(tagTag);
                        }
                    }
                    //tagList.Add(newTag);


                   


                }
            }

            ObservableCollection<VMMC_Core.TreeItem> sortTreeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            foreach (VMMC_Core.TreeItem ti in treeItemsList)
            {
                VMMC_Core.TreeItem existti = sortTreeItemsList.Where(x => x.TreeItemCode == ti.TreeItemCode && x.Parent.TreeItemId == ti.Parent.TreeItemId).FirstOrDefault();
                if (existti == null) sortTreeItemsList.Add(ti);
                else
                {

                }

            }
            treeItemsList = sortTreeItemsList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLTagsAttributesFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLTagsAttributesFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLTagsAttributesFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    string tagCodeStr = ""; //
                    string tagNameStr = ""; //
                    string tagClassCodeStr = ""; //

                    string attributeNameStr = ""; //
                    string attributeTypeStr = ""; //
                    string attributeCharValueStr = ""; //
                    string attributeDateValueStr = ""; //
                    string attributeNumberValueStr = ""; //





                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "code":
                                tagCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "name":
                                tagNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "char_name":
                                attributeNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "char_data_type":
                                attributeTypeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "char_value":
                                attributeCharValueStr = resultDT.Rows[i][j].ToString().Replace("''", "\"").Replace("'", "\"").Replace("\r", "\n").Trim();
                                break;

                            case "date_value":
                                attributeDateValueStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "number_value":
                                attributeNumberValueStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }
                                       

                    VMMC_Core.Tag tag = new VMMC_Core.Tag(sessionInfo).GetTag(tagCodeStr);
                    if (tag != null)
                    {
                        tagList.Add(tag);

                        VMMC_Core.Attribute atr = new VMMC_Core.Attribute(sessionInfo).SearchAttribute(attributeNameStr);

                        if (atr != null)
                        {
                            VMMC_Core.AttributeObjectValue existAOV = new VMMC_Core.AttributeObjectValue(sessionInfo).GetAttributeObjectValue(atr.AttributeId, tag.TagId);

                           
                            if (existAOV == null || atr.AllowMultiValues)
                            {

                                VMMC_Core.AttributeObjectValue newAOV = new VMMC_Core.AttributeObjectValue(sessionInfo)
                                {
                                    AttributeObjectValueId = Guid.NewGuid(),
                                    Attribute = atr,
                                    Object = new VMMC_Core.DbObject(sessionInfo).GetObject(tag.TagId),
                                };

                                switch (attributeTypeStr)
                                {
                                    case "CH":
                                        if (attributeCharValueStr != "" && attributeCharValueStr != "NULL") newAOV.StringValue = attributeCharValueStr;
                                        break;

                                    case "DA":
                                        if (attributeDateValueStr != "" && attributeCharValueStr != "NULL") newAOV.DateTimeValue = DateTime.Parse(attributeDateValueStr);
                                        break;

                                    case "NU":
                                        if (attributeNumberValueStr != "" && attributeCharValueStr != "NULL") newAOV.NumberValue = decimal.Parse(attributeNumberValueStr);
                                        break;

                                    case "PD":
                                        if (attributeCharValueStr != "" && attributeCharValueStr != "NULL")
                                        {
                                            VMMC_Core.EnumAttributeValue eav = new VMMC_Core.EnumAttributeValue(sessionInfo).GetEnumAttributeValue(newAOV.Attribute.AttributeId, attributeCharValueStr);
                                            if (eav == null)
                                            {
                                                eav = new VMMC_Core.EnumAttributeValue(sessionInfo)
                                                {
                                                    EnumAttributeValueId = Guid.NewGuid(),
                                                    AttributeId = newAOV.Attribute.AttributeId,
                                                    EnumValueStr = attributeCharValueStr
                                                };
                                                eav.CreateDBEnumAttributeValue();
                                                eav = new VMMC_Core.EnumAttributeValue(sessionInfo).GetEnumAttributeValue(newAOV.Attribute.AttributeId, attributeCharValueStr);
                                            }

                                            if (eav != null)
                                            {
                                                VMMC_Core.EnumObjectValue eov = new EnumObjectValue(sessionInfo).GetEnumObjectValue(newAOV.AttributeObjectValueId, eav.EnumAttributeValueId);
                                                if (eov == null) eov = new EnumObjectValue(sessionInfo) { EnumObjectValueId = Guid.NewGuid(), AttributeObjectValue = newAOV, EnumAttributeValue = eav };

                                                newAOV.EnumObjectValue = eov;
                                                newAOV.AvailibleEnumAttributeValueList = new EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(newAOV.Attribute.AttributeId);
                                                newAOV.AvailibleValuesList = new EnumObjectValue(sessionInfo).GetAvailibleEnumObjectValuesList(newAOV.Attribute.AttributeId);
                                            }
                                        }
                                        break;

                                    default:
                                        break;
                                }
                                atrList.Add(newAOV);
                            }
                        }
                    }
                }
            }

            ObservableCollection<VMMC_Core.TreeItem> sortTreeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            foreach (VMMC_Core.TreeItem ti in treeItemsList)
            {
                VMMC_Core.TreeItem existti = sortTreeItemsList.Where(x => x.TreeItemCode == ti.TreeItemCode && x.Parent.TreeItemId == ti.Parent.TreeItemId).FirstOrDefault();
                if (existti == null) sortTreeItemsList.Add(ti);
                else
                {

                }

            }
            treeItemsList = sortTreeItemsList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLTagsEnumAttributeValuesFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLTagsEnumAttributeValuesFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLTagsEnumAttributeValuesFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {

                    string attributeNameStr = ""; //
                    string attributeCharValueStr = ""; //


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "char_name":
                                attributeNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "char_value":
                                attributeCharValueStr = resultDT.Rows[i][j].ToString().Replace("''", "\"").Replace("'", "\"").Replace("\r", "\n").Trim();
                                if (attributeCharValueStr == "Дюйм") attributeCharValueStr = "\"";
                                break;

                            default:
                                break;
                        }
                    }

                    VMMC_Core.Attribute atr = new VMMC_Core.Attribute(sessionInfo).SearchAttribute(attributeNameStr);

                    if (atr != null)
                    {
                        if (attributeCharValueStr != "" && attributeCharValueStr != "NULL")
                        {                            
                            VMMC_Core.EnumAttributeValue eav = new VMMC_Core.EnumAttributeValue(sessionInfo).GetEnumAttributeValue(atr.AttributeId, attributeCharValueStr);
                            if (eav == null)
                            {
                                eav = new VMMC_Core.EnumAttributeValue(sessionInfo)
                                {
                                    EnumAttributeValueId = Guid.NewGuid(),
                                    AttributeId = atr.AttributeId,
                                    EnumValueStr = attributeCharValueStr
                                };
                                eav.CreateDBEnumAttributeValue();
                                eav = new VMMC_Core.EnumAttributeValue(sessionInfo).GetEnumAttributeValue(atr.AttributeId, attributeCharValueStr);
                            }                            
                        }
                    }
                }
            }

           


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLAttributeFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLAttributeFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLAttributeFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Attribute> resultAtrList = new ObservableCollection<VMMC_Core.Attribute>();

            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {

                    string attributeNameStr = ""; //
                    string attributeTypeStr = ""; //
                    string attributeIsMultiValueStr = ""; //


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "char_name":
                                attributeNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "char_data_type":
                                attributeTypeStr = resultDT.Rows[i][j].ToString().Replace("''", "\"").Replace("\r", "\n").Trim();
                                break;

                            case "multi_valued":
                                attributeIsMultiValueStr = resultDT.Rows[i][j].ToString().Replace("''", "\"").Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }

                    VMMC_Core.Attribute atr = new VMMC_Core.Attribute(sessionInfo).SearchAttribute(attributeNameStr);

                    if (atr == null)
                    {
                        atr = new VMMC_Core.Attribute(sessionInfo);
                        atr.AttributeId = Guid.NewGuid();
                        atr.AttributeName = attributeNameStr;
                        if (attributeIsMultiValueStr == "Y") atr.AllowMultiValues = true;
                        else atr.AllowMultiValues = false;

                        switch (attributeTypeStr)
                        {
                            case "CH":
                                atr.AtributeDataTypeId = 1;
                                atr.MeasureGroupId = Guid.Parse("794D45ED-88CC-ED11-A60C-00155D03FA01"); //Безразмерная величина
                                break;

                            case "NU":
                                atr.AtributeDataTypeId = 2;
                                atr.MeasureGroupId = Guid.Parse("794D45ED-88CC-ED11-A60C-00155D03FA01"); //Безразмерная величина
                                break;

                            case "DA":
                                atr.AtributeDataTypeId = 3;
                                atr.MeasureGroupId = Guid.Parse("C2A88F36-AA22-ED11-A60A-00155D03FA01"); //Время
                                break;

                            case "PD":
                                atr.AtributeDataTypeId = 3;
                                atr.IsEnum = true;
                                atr.MeasureGroupId = Guid.Parse("794D45ED-88CC-ED11-A60C-00155D03FA01"); //Безразмерная величина

                                break;

                            default:
                                break;
                        }
                        resultAtrList.Add(atr);
                        atr.CreateDBAttribute();
                        
                    }
                }
            }




            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLDocTreeFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLDocTreeFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLDocTreeFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {
                    string treeItemCodeStr = ""; //
                    string treeItemTypeIdStr = ""; //
                    string parentIdStr = ""; //


                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "TreeItemName":
                                treeItemCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "TreeItemId":
                                treeItemTypeIdStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "ParentId":
                                parentIdStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }

                    VMMC_Core.TreeItem parentTreeItem = new TreeItem(sessionInfo).getTreeItem(parentIdStr, null);
                    if (parentTreeItem != null)
                    {
                        VMMC_Core.TreeItem doctreeTreeItem = new TreeItem(sessionInfo).getTreeItem(treeItemCodeStr, parentTreeItem.TreeItemId.ToString());
                        if (doctreeTreeItem == null)
                        {
                            doctreeTreeItem = new TreeItem(sessionInfo);

                            doctreeTreeItem.TreeItemId = Guid.NewGuid();
                            doctreeTreeItem.TreeItemName = "";
                            doctreeTreeItem.TreeItemCode = treeItemCodeStr;
                            doctreeTreeItem.TreeItemDescription = treeItemTypeIdStr;
                            doctreeTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("TREEITEM");
                            doctreeTreeItem.Parent = parentTreeItem;
                            treeItemsList.Add(doctreeTreeItem);
                        }
                    }
                }
            }

            ObservableCollection<VMMC_Core.TreeItem> sortTreeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            foreach (VMMC_Core.TreeItem ti in treeItemsList)
            {
                VMMC_Core.TreeItem existti = sortTreeItemsList.Where(x => x.TreeItemCode == ti.TreeItemCode && x.Parent.TreeItemId == ti.Parent.TreeItemId).FirstOrDefault();
                if (existti == null) sortTreeItemsList.Add(ti);
                else
                {

                }

            }
            treeItemsList = sortTreeItemsList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLDocumentsFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLDocumentsFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLDocumentsFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {

                    string docCodeStr = ""; //
                    string docNameStr = ""; //
                    string docRevisionStr = ""; //
                    string docClassCodeStr = ""; //


                    string tagCodeStr = ""; //
                    string tagNameStr = ""; //

                    string systemCodeStr = ""; //
                    string systemNameStr = ""; //

                    string treeItemCodeStr = ""; //
                    string treeItemNameStr = ""; //




                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "prefix":
                                docCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "title":
                                docNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "revision":
                                docRevisionStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "code":
                                docClassCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }


                    VMMC_Core.Document newDoc = new VMMC_Core.Document(sessionInfo).GetDocument(docCodeStr);
                    if (newDoc == null)
                    {
                        newDoc = new VMMC_Core.Document(sessionInfo)
                        {
                            DocumentId = Guid.NewGuid(),
                            DocumentCode = docCodeStr,
                            DocumentName = docNameStr,
                            DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass(docClassCodeStr).ClassId

                        };
                    }
                    if (docRevisionStr != "" && docRevisionStr != "NULL")
                    {
                        if (newDoc.Revisions == null) newDoc.Revisions = new ObservableCollection<Revision>();
                        newDoc.Revisions.Add(new VMMC_Core.Revision(sessionInfo) { RevisionId = Guid.NewGuid(), DocumentId = newDoc.DocumentId, Number = int.Parse(docRevisionStr) });
                    }
                    docList.Add(newDoc);

                    //VMMC_Core.Tag newTag = new VMMC_Core.Tag(sessionInfo).GetTag(tagCodeStr);
                    //if (newTag == null)
                    //{
                    //    newTag = new VMMC_Core.Tag(sessionInfo)
                    //    {
                    //        TagId = Guid.NewGuid(),
                    //        Position = tagCodeStr,
                    //        TagName = tagNameStr,
                    //        TagClassId = new VMMC_Core.Class(sessionInfo).getClass(tagClassCodeStr).ClassId

                    //    };
                    //}
                    //tagList.Add(newTag);


                    //VMMC_Core.TreeItem tagtreeTreeItem = new TreeItem(sessionInfo).getTreeItem("ELEMTREE", null);

                    ////VMMC_Core.TreeItem projectTreeItem = new TreeItem(sessionInfo).getTreeItem("4550.70", tagtreeTreeItem.TreeItemId.ToString());
                    ////if (projectTreeItem.TreeItemCode == null)
                    ////{
                    ////    projectTreeItem.TreeItemId = Guid.NewGuid();
                    ////    projectTreeItem.TreeItemCode = complexCodeStr;
                    ////    projectTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("TREEITEM");
                    ////    projectTreeItem.Parent = tagtreeTreeItem;
                    ////    treeItemsList.Add(projectTreeItem);
                    ////}

                    //VMMC_Core.TreeItem buildingTreeItem = new TreeItem(sessionInfo).getTreeItem("4550.70", tagtreeTreeItem.TreeItemId.ToString());
                    //if (buildingTreeItem.TreeItemCode == null)
                    //{
                    //    buildingTreeItem.TreeItemId = Guid.NewGuid();
                    //    buildingTreeItem.TreeItemCode = "4550.70";
                    //    buildingTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("BUILD");
                    //    buildingTreeItem.Parent = tagtreeTreeItem;
                    //    treeItemsList.Add(buildingTreeItem);
                    //}

                    //VMMC_Core.TreeItem complexTreeItem = new TreeItem(sessionInfo).getTreeItem("004", buildingTreeItem.TreeItemId.ToString());
                    //if (complexTreeItem.TreeItemCode == null)
                    //{
                    //    complexTreeItem.TreeItemId = Guid.NewGuid();
                    //    complexTreeItem.TreeItemCode = "004";
                    //    complexTreeItem.TreeItemName = "004";
                    //    complexTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("COMPLEX");
                    //    complexTreeItem.Parent = buildingTreeItem;
                    //    treeItemsList.Add(complexTreeItem);
                    //}

                    //VMMC_Core.TreeItem systemTreeItem = new TreeItem(sessionInfo).getTreeItem(systemCodeStr.Replace("4550.70.004.", ""), complexTreeItem.TreeItemId.ToString());
                    //if (systemTreeItem.TreeItemCode == null)
                    //{
                    //    systemTreeItem.TreeItemId = Guid.NewGuid();
                    //    systemTreeItem.TreeItemCode = systemCodeStr.Replace("4550.70.004.", "");
                    //    systemTreeItem.TreeItemName = systemNameStr;
                    //    systemTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("SYSTEM");
                    //    systemTreeItem.Parent = complexTreeItem;
                    //    treeItemsList.Add(systemTreeItem);
                    //}

                    //VMMC_Core.Relationship tagSystem = new Relationship(sessionInfo)
                    //{
                    //    RelationshipId = Guid.NewGuid(),
                    //    LeftObject = new DbObject(sessionInfo) { ObjectId = systemTreeItem.TreeItemId },
                    //    RightObject = new DbObject(sessionInfo) { ObjectId = newTag.TagId },
                    //    RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Система-Тэг")
                    //};
                    //relList.Add(tagSystem);


                }
            }

            ObservableCollection<VMMC_Core.TreeItem> sortTreeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            foreach (VMMC_Core.TreeItem ti in treeItemsList)
            {
                VMMC_Core.TreeItem existti = sortTreeItemsList.Where(x => x.TreeItemCode == ti.TreeItemCode && x.Parent.TreeItemId == ti.Parent.TreeItemId).FirstOrDefault();
                if (existti == null) sortTreeItemsList.Add(ti);
                else
                {

                }

            }
            treeItemsList = sortTreeItemsList;
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLDocumentsRelationshipFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLDocumentsRelationshipFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLDocumentsRelationshipFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {

                    string docCodeStr = ""; //
                    string docNameStr = ""; //
                    string docRevisionStr = ""; //
                    string docClassCodeStr = ""; //


                    string objectCodeStr = ""; //
                    string objectTypeStr = ""; //




                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "prefix":
                                docCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "title":
                                docNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "revision":
                                docRevisionStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "code":
                                docClassCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "ObjectCode":
                                objectCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "ObjectType":
                                objectTypeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }


                    VMMC_Core.Document document = new VMMC_Core.Document(sessionInfo).GetDocument(docCodeStr);
                    if (document != null)
                    {
                        switch (objectTypeStr)
                        {
                            case "TAG":
                                VMMC_Core.Tag tag = new VMMC_Core.Tag(sessionInfo).GetTag(objectCodeStr);
                                VMMC_Core.Relationship tagDocument = new Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    LeftObject = new DbObject(sessionInfo) { ObjectId = tag.TagId },
                                    RightObject = new DbObject(sessionInfo) { ObjectId = document.DocumentId },
                                    RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Тег-Документ")
                                };
                                relList.Add(tagDocument);
                                break;

                            case "SYSTEM":
                                VMMC_Core.TreeItem system = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(objectCodeStr.Replace("4550.70.004.", ""), new VMMC_Core.Class(sessionInfo).getClass("SYSTEM").ClassId.ToString());
                                if (system != null)
                                {
                                    VMMC_Core.Relationship systemDocument = new Relationship(sessionInfo)
                                    {
                                        RelationshipId = Guid.NewGuid(),
                                        LeftObject = new DbObject(sessionInfo) { ObjectId = system.TreeItemId },
                                        RightObject = new DbObject(sessionInfo) { ObjectId = document.DocumentId },
                                        RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Система-Документ")
                                    };
                                    relList.Add(systemDocument);
                                }
                                break;

                            case "FOLDER":
                                VMMC_Core.TreeItem folder = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(objectCodeStr, new VMMC_Core.Class(sessionInfo).getClass("TREEITEM").ClassId.ToString());
                                if (folder != null)
                                {
                                    VMMC_Core.Relationship folderDocument = new Relationship(sessionInfo)
                                    {
                                        RelationshipId = Guid.NewGuid(),
                                        LeftObject = new DbObject(sessionInfo) { ObjectId = folder.TreeItemId },
                                        RightObject = new DbObject(sessionInfo) { ObjectId = document.DocumentId },
                                        RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ")
                                    };
                                    relList.Add(folderDocument);
                                }
                                break;

                            default:
                                break;
                        }

                    }
                    

                    //VMMC_Core.Tag newTag = new VMMC_Core.Tag(sessionInfo).GetTag(tagCodeStr);
                    //if (newTag == null)
                    //{
                    //    newTag = new VMMC_Core.Tag(sessionInfo)
                    //    {
                    //        TagId = Guid.NewGuid(),
                    //        Position = tagCodeStr,
                    //        TagName = tagNameStr,
                    //        TagClassId = new VMMC_Core.Class(sessionInfo).getClass(tagClassCodeStr).ClassId

                    //    };
                    //}
                    //tagList.Add(newTag);


                    //VMMC_Core.TreeItem tagtreeTreeItem = new TreeItem(sessionInfo).getTreeItem("ELEMTREE", null);

                    ////VMMC_Core.TreeItem projectTreeItem = new TreeItem(sessionInfo).getTreeItem("4550.70", tagtreeTreeItem.TreeItemId.ToString());
                    ////if (projectTreeItem.TreeItemCode == null)
                    ////{
                    ////    projectTreeItem.TreeItemId = Guid.NewGuid();
                    ////    projectTreeItem.TreeItemCode = complexCodeStr;
                    ////    projectTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("TREEITEM");
                    ////    projectTreeItem.Parent = tagtreeTreeItem;
                    ////    treeItemsList.Add(projectTreeItem);
                    ////}

                    //VMMC_Core.TreeItem buildingTreeItem = new TreeItem(sessionInfo).getTreeItem("4550.70", tagtreeTreeItem.TreeItemId.ToString());
                    //if (buildingTreeItem.TreeItemCode == null)
                    //{
                    //    buildingTreeItem.TreeItemId = Guid.NewGuid();
                    //    buildingTreeItem.TreeItemCode = "4550.70";
                    //    buildingTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("BUILD");
                    //    buildingTreeItem.Parent = tagtreeTreeItem;
                    //    treeItemsList.Add(buildingTreeItem);
                    //}

                    //VMMC_Core.TreeItem complexTreeItem = new TreeItem(sessionInfo).getTreeItem("004", buildingTreeItem.TreeItemId.ToString());
                    //if (complexTreeItem.TreeItemCode == null)
                    //{
                    //    complexTreeItem.TreeItemId = Guid.NewGuid();
                    //    complexTreeItem.TreeItemCode = "004";
                    //    complexTreeItem.TreeItemName = "004";
                    //    complexTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("COMPLEX");
                    //    complexTreeItem.Parent = buildingTreeItem;
                    //    treeItemsList.Add(complexTreeItem);
                    //}

                    //VMMC_Core.TreeItem systemTreeItem = new TreeItem(sessionInfo).getTreeItem(systemCodeStr.Replace("4550.70.004.", ""), complexTreeItem.TreeItemId.ToString());
                    //if (systemTreeItem.TreeItemCode == null)
                    //{
                    //    systemTreeItem.TreeItemId = Guid.NewGuid();
                    //    systemTreeItem.TreeItemCode = systemCodeStr.Replace("4550.70.004.", "");
                    //    systemTreeItem.TreeItemName = systemNameStr;
                    //    systemTreeItem.Class = new VMMC_Core.Class(sessionInfo).getClass("SYSTEM");
                    //    systemTreeItem.Parent = complexTreeItem;
                    //    treeItemsList.Add(systemTreeItem);
                    //}

                    //VMMC_Core.Relationship tagSystem = new Relationship(sessionInfo)
                    //{
                    //    RelationshipId = Guid.NewGuid(),
                    //    LeftObject = new DbObject(sessionInfo) { ObjectId = systemTreeItem.TreeItemId },
                    //    RightObject = new DbObject(sessionInfo) { ObjectId = newTag.TagId },
                    //    RelTypeId = new Relationship(sessionInfo).GetRelationshipTypeId("Система-Тэг")
                    //};
                    //relList.Add(tagSystem);


                }
            }

           
            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }
        private void GetBKFILLDocumentsFilesFromExcel()
        {
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
                        AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                        ExcelParserViewModel newExcelParserViewModel = GetBKFILLDocumentsFilesFromExcel(fileName);

                        if (DocumentsCollection == null) DocumentsCollection = newExcelParserViewModel.DocumentsCollection;
                        else foreach (VMMC_Core.Document document in newExcelParserViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                        if (TagsCollection == null) TagsCollection = newExcelParserViewModel.TagsCollection;
                        else foreach (VMMC_Core.Tag tag in newExcelParserViewModel.TagsCollection) TagsCollection.Add(tag);

                        if (ComplektsCollection == null) ComplektsCollection = newExcelParserViewModel.ComplektsCollection;
                        else foreach (VMMC_Core.Complekt complekt in newExcelParserViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                        if (RelationshipsCollection == null) RelationshipsCollection = newExcelParserViewModel.RelationshipsCollection;
                        else foreach (VMMC_Core.Relationship relationship in newExcelParserViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                        if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newExcelParserViewModel.AttributeObjectValuesCollection;
                        else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newExcelParserViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                        if (AttributeObjectValuesCollection != null)
                        {
                            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                            {
                                //AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel(aov) { AttributeObjectValue = aov });
                            }
                        }
                        if (TreeItemsCollection == null) TreeItemsCollection = newExcelParserViewModel.TreeItemsCollection;
                        else foreach (VMMC_Core.TreeItem treeItem in newExcelParserViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

                    }
                }
            }
        }
        public ExcelParserViewModel GetBKFILLDocumentsFilesFromExcel(string FileName)
        {
            ExcelParserViewModel excelParserViewModel = new ExcelParserViewModel(sessionInfo);

            ObservableCollection<DataTable> resultDtList = new VMMC_ExcelParcer.ImportFromExcel().ExcelToDataTable(FileName, true);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Tag> tagList = new ObservableCollection<VMMC_Core.Tag>();
            ObservableCollection<VMMC_Core.Relationship> relList = new ObservableCollection<VMMC_Core.Relationship>();
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new ObservableCollection<VMMC_Core.TreeItem>();
            ObservableCollection<VMMC_Core.AttributeObjectValue> atrList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();


            foreach (DataTable resultDT in resultDtList)
            {
                for (int i = 0; i < resultDT.Rows.Count; i++)
                {

                    string docCodeStr = ""; //
                    string docRevisionStr = ""; //
                    int docRevisionInt = 0;


                    string PhysicalPathStr = ""; //
                    string FileNameStr = ""; //
                    string FileTypeStr = ""; //




                    for (int j = 0; j < resultDT.Columns.Count; j++)
                    {
                        string columnName = resultDT.Columns[j].ColumnName.Replace("\n", "");

                        switch (columnName)
                        {
                            case "prefix":
                                docCodeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "revision":
                                docRevisionStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                int.TryParse(docRevisionStr, out docRevisionInt);
                                break;

                            case "PhysicalPath":
                                PhysicalPathStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "FileName":
                                FileNameStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            case "FileType":
                                FileTypeStr = resultDT.Rows[i][j].ToString().Replace("\r", "\n").Trim();
                                break;

                            default:
                                break;
                        }
                    }


                    VMMC_Core.Document document = new VMMC_Core.Document(sessionInfo).GetDocument(docCodeStr);
                    if (document != null)
                    {
                        if (document.Revisions == null) document.Revisions = new ObservableCollection<Revision>();
                        VMMC_Core.Revision revision = document.Revisions.Where(x => x.Number == docRevisionInt).FirstOrDefault();
                        if (revision == null) document.Revisions.Add(new VMMC_Core.Revision(sessionInfo) { RevisionId= Guid.NewGuid(), DocumentId = document.DocumentId, Number = docRevisionInt } );
                        revision = document.Revisions.Where(x => x.Number == docRevisionInt).FirstOrDefault();

                        if (revision != null) 
                        {
                            if (revision.Files == null) revision.Files = new ObservableCollection<Files>();
                            VMMC_Core.Files file = revision.Files.Where(x => x.FileName == FileNameStr).FirstOrDefault();
                            if (file == null)
                            {
                                file = new VMMC_Core.Files(sessionInfo){ FileName = FileNameStr, LocalPath = PhysicalPathStr, FileType = FileTypeStr, RevisionId = revision.RevisionId };
                                file.Checksum = file.ComputeMD5Checksum(file.LocalPath);
                                revision.Files.Add(file);
                            }
                            file = revision.Files.Where(x => x.FileName == FileNameStr).FirstOrDefault();
                        }
                        docList.Add(document);
                    }
                }
            }

            excelParserViewModel.DocumentsCollection = docList;
            excelParserViewModel.TagsCollection = tagList;
            excelParserViewModel.RelationshipsCollection = relList;
            excelParserViewModel.TreeItemsCollection = treeItemsList;
            excelParserViewModel.AttributeObjectValuesCollection = atrList;


            return excelParserViewModel;
            //DataContext = excelParserViewModel;
            //Documents_DataGrid.ItemsSource = excelParserViewModel.DocumentsCollection;
        }

        private ObservableCollection<VMMC_Core.Relationship> RelNewDocList(VMMC_Core.Document oldDoc, VMMC_Core.Document newDoc, ObservableCollection<VMMC_Core.Relationship> targetRelList)
        {
            ObservableCollection<VMMC_Core.Relationship> result = new ObservableCollection<VMMC_Core.Relationship>();

            IEnumerable<VMMC_Core.Relationship> existRels = targetRelList.Where(x => x.LeftObjectId == oldDoc.DocumentId || x.RightObjectId == oldDoc.DocumentId || x.LeftObject.ObjectId == oldDoc.DocumentId || x.RightObject.ObjectId == oldDoc.DocumentId);

            if (existRels.Count() != 0)
            {
                foreach (VMMC_Core.Relationship existRel in existRels)
                {
                    VMMC_Core.DbObject docObj = new VMMC_Core.DbObject(sessionInfo)
                    {
                        ObjectId = newDoc.DocumentId,
                        ObjectCode = newDoc.DocumentCode,
                        ObjectName = newDoc.DocumentName,
                        CreatedBy = sessionInfo.UserName,
                        LastModifiedBy = sessionInfo.UserName,

                    };

                    if (existRel.LeftObjectId == oldDoc.DocumentId || existRel.LeftObject.ObjectId == oldDoc.DocumentId)
                    {
                        VMMC_Core.Relationship newRel = new VMMC_Core.Relationship(sessionInfo)
                        {

                            RelationshipId = Guid.NewGuid(),
                            RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("3D_Модель-Документ"),
                            LeftObject = docObj,
                            RightObject = existRel.RightObject

                        };
                    }
                    else if (existRel.RightObjectId == oldDoc.DocumentId || existRel.RightObject.ObjectId == oldDoc.DocumentId)
                    {
                        VMMC_Core.Relationship newRel = new VMMC_Core.Relationship(sessionInfo)
                        {

                            RelationshipId = Guid.NewGuid(),
                            RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("3D_Модель-Документ"),
                            LeftObject = existRel.LeftObject,
                            RightObject = docObj

                        };

                        result.Add(newRel);
                    }
                }
            }
            return result;
        }
        private ObservableCollection<VMMC_Core.Relationship> ReplacedocId(VMMC_Core.Document oldDoc, VMMC_Core.Document newDoc, ObservableCollection<VMMC_Core.Relationship> targetRelList)
        {
            IEnumerable<VMMC_Core.Relationship> targetRels = targetRelList.Where(x => x.LeftObjectId == oldDoc.DocumentId || x.LeftObject.ObjectId == oldDoc.DocumentId || x.RightObjectId == oldDoc.DocumentId || x.RightObject.ObjectId == oldDoc.DocumentId);
            foreach (VMMC_Core.Relationship targetRel in targetRels)
            {
                if (targetRel.LeftObjectId == oldDoc.DocumentId || targetRel.LeftObject.ObjectId == oldDoc.DocumentId)
                {
                    targetRel.LeftObjectId = newDoc.DocumentId;
                    targetRel.LeftObject.ObjectId = newDoc.DocumentId;
                    targetRel.LeftObject.ObjectCode = newDoc.DocumentCode;
                    targetRel.LeftObject.ObjectName = newDoc.DocumentName;

                }
                else if (targetRel.RightObjectId == oldDoc.DocumentId || targetRel.RightObject.ObjectId == oldDoc.DocumentId)
                {
                    targetRel.RightObjectId = newDoc.DocumentId;
                    targetRel.RightObject.ObjectId = newDoc.DocumentId;
                    targetRel.RightObject.ObjectCode = newDoc.DocumentCode;
                    targetRel.RightObject.ObjectName = newDoc.DocumentName;
                }
            }
            return targetRelList;
        }
        private ExcelParserViewModel UnionAllDocumentInListByCode(ExcelParserViewModel excelParserViewModel)
        {
            ObservableCollection<VMMC_Core.Document> targetDocumentList = excelParserViewModel.DocumentsCollection;
            ObservableCollection<VMMC_Core.Relationship> targetRelationshipList = excelParserViewModel.RelationshipsCollection;

            ObservableCollection<VMMC_Core.Document> result = new ObservableCollection<VMMC_Core.Document>();
            foreach (VMMC_Core.Document doc in targetDocumentList)
            {
                VMMC_Core.Document existDoc = result.Where(x => x.DocumentCode == doc.DocumentCode).FirstOrDefault();
                if (existDoc == null) result.Add(doc);
                else
                {
                    ////existDoc.StatusInfo += "Updated";
                    //foreach (VMMC_Core.Revision rev in doc.Revisions)
                    //{
                    //    VMMC_Core.Revision existRev = existDoc.Revisions.Where(x => x.Number == rev.Number).FirstOrDefault();
                    //    if (existRev == null) existDoc.Revisions.Add(rev);
                    //}
                    if(targetRelationshipList!=null) targetRelationshipList = ReplacedocId(doc, existDoc, targetRelationshipList);
                    {/*replace docId in relationships*/}
                }
            }

            excelParserViewModel.DocumentsCollection = result;
            excelParserViewModel.RelationshipsCollection = targetRelationshipList;

            return excelParserViewModel;
        }
        private VMMC_Core.Relationship CreateRelDoc3DDocRD(VMMC_Core.Document newDoc3D, VMMC_Core.Document newDocRD)
        {
            VMMC_Core.DbObject doc3DObj = new VMMC_Core.DbObject(sessionInfo)
            {
                ObjectId = newDoc3D.DocumentId,
                ObjectCode = newDoc3D.DocumentCode,
                ObjectName = newDoc3D.DocumentName,
                CreatedBy = sessionInfo.UserName,
                LastModifiedBy = sessionInfo.UserName,

            };
            VMMC_Core.DbObject docRDObj = new VMMC_Core.DbObject(sessionInfo)
            {
                ObjectId = newDocRD.DocumentId,
                ObjectCode = newDocRD.DocumentCode,
                ObjectName = newDocRD.DocumentName,
                CreatedBy = sessionInfo.UserName,
                LastModifiedBy = sessionInfo.UserName,

            };

            VMMC_Core.Relationship newRel = new VMMC_Core.Relationship(sessionInfo)
            {

                RelationshipId = Guid.NewGuid(),
                RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("3D_Модель-Документ"),
                LeftObject = doc3DObj,
                RightObject = docRDObj

            };

            return newRel;
        }
        private VMMC_Core.Relationship CreateRelTagDocRD(VMMC_Core.Tag newTag, VMMC_Core.Document newDocRD)
        {
            VMMC_Core.DbObject tagObj = new VMMC_Core.DbObject(sessionInfo)
            {
                ObjectId = newTag.TagId,
                ObjectCode = newTag.Position,
                ObjectName = newTag.TagName,
                CreatedBy = sessionInfo.UserName,
                LastModifiedBy = sessionInfo.UserName,

            };
            VMMC_Core.DbObject docRDObj = new VMMC_Core.DbObject(sessionInfo)
            {
                ObjectId = newDocRD.DocumentId,
                ObjectCode = newDocRD.DocumentCode,
                ObjectName = newDocRD.DocumentName,
                CreatedBy = sessionInfo.UserName,
                LastModifiedBy = sessionInfo.UserName,

            };

            VMMC_Core.Relationship newRel = new VMMC_Core.Relationship(sessionInfo)
            {

                RelationshipId = Guid.NewGuid(),
                RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Тэг-Рабочая документация"),
                LeftObject = tagObj,
                RightObject = docRDObj

            };

            return newRel;
        }
        private ExcelParserViewModel UnionAllTagInListByCode(ExcelParserViewModel excelParserViewModel)
        {
            ObservableCollection<VMMC_Core.Tag> targetTagList = excelParserViewModel.TagsCollection;
            ObservableCollection<VMMC_Core.Relationship> targetRelationshipList = excelParserViewModel.RelationshipsCollection;

            ObservableCollection<VMMC_Core.Tag> result = new ObservableCollection<VMMC_Core.Tag>();
            foreach (VMMC_Core.Tag tag in targetTagList)
            {
                VMMC_Core.Tag existTag = result.Where(x => x.Position == tag.Position).FirstOrDefault();
                if (existTag == null) result.Add(tag);
                else
                {
                    targetRelationshipList = ReplaceTagId(tag, existTag, targetRelationshipList);
                    {/*replace docId in relationships*/}
                }
            }

            excelParserViewModel.TagsCollection = result;
            excelParserViewModel.RelationshipsCollection = targetRelationshipList;

            return excelParserViewModel;
        }
        private VMMC_Core.Relationship CreateRelTagDoc(VMMC_Core.Tag newTag, VMMC_Core.Document newDoc)
        {
            VMMC_Core.DbObject tagObj = new VMMC_Core.DbObject(sessionInfo)
            {
                ObjectId = newTag.TagId,
                ObjectCode = newTag.Position,
                ObjectName = newTag.TagName,
                CreatedBy = sessionInfo.UserName,
                LastModifiedBy = sessionInfo.UserName,

            };
            VMMC_Core.DbObject docObj = new VMMC_Core.DbObject(sessionInfo)
            {
                ObjectId = newDoc.DocumentId,
                ObjectCode = newDoc.DocumentCode,
                ObjectName = newDoc.DocumentName,
                CreatedBy = sessionInfo.UserName,
                LastModifiedBy = sessionInfo.UserName,

            };

            VMMC_Core.Relationship newRel = new VMMC_Core.Relationship(sessionInfo)
            {

                RelationshipId = Guid.NewGuid(),
                RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Тег-3D Модель"),
                LeftObject = tagObj,
                RightObject = docObj

            };

            return newRel;
        }
        private ObservableCollection<VMMC_Core.Relationship> ReplaceTagId(VMMC_Core.Tag oldTag, VMMC_Core.Tag newTag, ObservableCollection<VMMC_Core.Relationship> targetRelList)
        {
            IEnumerable<VMMC_Core.Relationship> targetRels = targetRelList.Where(x => x.LeftObjectId == oldTag.TagId || x.LeftObject.ObjectId == oldTag.TagId || x.RightObjectId == oldTag.TagId || x.RightObject.ObjectId == oldTag.TagId);
            foreach (VMMC_Core.Relationship targetRel in targetRels)
            {
                if (targetRel.LeftObjectId == oldTag.TagId || targetRel.LeftObject.ObjectId == oldTag.TagId)
                {
                    targetRel.LeftObjectId = newTag.TagId;
                    targetRel.LeftObject.ObjectId = newTag.TagId;
                    targetRel.LeftObject.ObjectCode = newTag.Position;
                    targetRel.LeftObject.ObjectName = newTag.TagName;

                }
                else if (targetRel.RightObjectId == oldTag.TagId || targetRel.RightObject.ObjectId == oldTag.TagId)
                {
                    targetRel.RightObjectId = newTag.TagId;
                    targetRel.RightObject.ObjectId = newTag.TagId;
                    targetRel.RightObject.ObjectCode = newTag.Position;
                    targetRel.RightObject.ObjectName = newTag.TagName;
                }
            }
            return targetRelList;
        }
        private ObservableCollection<VMMC_Core.Relationship> getRelationshipFromDoc3DCode(string doc3DCode, VMMC_Core.DbObject rightObj, Guid relTypeId)
        {
            ObservableCollection<VMMC_Core.Relationship> result = new ObservableCollection<VMMC_Core.Relationship>();

            if (doc3DCode != null)
            {
                if (doc3DCode != "")
                {
                    string[] items = doc3DCode.Replace("--", "-").Split('-');
                    if (items.Length > 7)
                    {
                        string[] newItem = new string[]
                        {
                                items[0],
                                items[1],
                                items[2],
                                items[3],
                                items[4] +"-"+items[5],
                                items[6],
                                items[7]
                        };

                        items = newItem;
                    }


                    if (items.Length == 7)
                    {
                        VMMC_Core.TreeItem treeItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem("ELEMTREE", null);
                        if (items[1].Length == 2) items[1] = "00" + items[1];
                        VMMC_Core.TreeItem buildItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[1], treeItem.TreeItemId.ToString());
                        if (buildItem.TreeItemCode != null)
                        {
                            VMMC_Core.DbObject buildObj = new VMMC_Core.DbObject(sessionInfo)
                            {
                                ObjectId = buildItem.TreeItemId,
                                ObjectCode = buildItem.TreeItemCode,
                                ObjectName = buildItem.TreeItemName,
                                CreatedBy = sessionInfo.UserName,
                                LastModifiedBy = sessionInfo.UserName,

                            };

                            VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                RelTypeId = relTypeId,
                                LeftObject = buildObj,
                                RightObject = rightObj

                            };
                            result.Add(relDoc);
                        }
                        else
                        {
                            VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                RightObject = rightObj
                            };
                            if (items[1] == "") relDoc.StatusInfo += "Шифр строения не обнаружен в коде. ";
                            else relDoc.StatusInfo += "Строение " + items[1] + " не найдено. ";
                            result.Add(relDoc);

                        }

                        string systemItemCode = items[4];
                        if (systemItemCode.IndexOf('-') > 0 & items[3] == "HV") systemItemCode = "SOV";
                        else if (systemItemCode.Length != 3 & items[3] == "HV") systemItemCode = "SOV";
                        //else { }

                        VMMC_Core.TreeItem systemItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(systemItemCode, buildItem.TreeItemId.ToString());
                        if (systemItem.TreeItemCode != null)
                        {
                            VMMC_Core.DbObject systemObj = new VMMC_Core.DbObject(sessionInfo)
                            {
                                ObjectId = systemItem.TreeItemId,
                                ObjectCode = systemItem.TreeItemCode,
                                ObjectName = systemItem.TreeItemName,
                                CreatedBy = sessionInfo.UserName,
                                LastModifiedBy = sessionInfo.UserName,

                            };

                            VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                RelTypeId = relTypeId,
                                LeftObject = systemObj,
                                RightObject = rightObj

                            };
                            result.Add(relDoc);
                        }
                        else
                        {
                            VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                RightObject = rightObj
                            };
                            if (items[4] == "") relDoc.StatusInfo += "Шифр системы не обнаружен в коде. ";
                            else relDoc.StatusInfo += "Система " + items[4] + " не найдена в родительском TreeItem " + buildItem.TreeItemCode + ". ";
                            result.Add(relDoc);

                        }


                        VMMC_Core.TreeItem levelItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[2], systemItem.TreeItemId.ToString());
                        if (levelItem.TreeItemCode == null) levelItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem("В"/*кирилица*/+ items[2].Replace("B", ""), systemItem.TreeItemId.ToString()); //если в бд уровень на кирилице

                        if (levelItem.TreeItemCode != null)
                        {
                            VMMC_Core.DbObject levelObj = new VMMC_Core.DbObject(sessionInfo)
                            {
                                ObjectId = levelItem.TreeItemId,
                                ObjectCode = levelItem.TreeItemCode,
                                ObjectName = levelItem.TreeItemName,
                                CreatedBy = sessionInfo.UserName,
                                LastModifiedBy = sessionInfo.UserName,

                            };

                            VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                RelTypeId = relTypeId,
                                LeftObject = levelObj,
                                RightObject = rightObj

                            };
                            result.Add(relDoc);
                        }
                        else
                        {
                            VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                            {
                                RelationshipId = Guid.NewGuid(),
                                RightObject = rightObj
                            };
                            if (items[2] == "") relDoc.StatusInfo += "Шифр отметки не обнаружен в коде. ";
                            else relDoc.StatusInfo += "Отметка " + items[2] + " не найдена в родительском TreeItem " + systemItem.TreeItemCode + ". ";
                            result.Add(relDoc);

                        }

                        if (items[4].IndexOf('-') > 0)
                        {
                            VMMC_Core.TreeItem subsystemItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[4], levelItem.TreeItemId.ToString());

                            if (subsystemItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject subsystemObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = subsystemItem.TreeItemId,
                                    ObjectCode = subsystemItem.TreeItemCode,
                                    ObjectName = subsystemItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = relTypeId,
                                    LeftObject = subsystemObj,
                                    RightObject = rightObj

                                };
                                result.Add(relDoc);
                            }
                            else
                            {
                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RightObject = rightObj
                                };
                                if (items[2] == "") relDoc.StatusInfo += "Шифр подсистемы не обнаружен в коде. ";
                                else relDoc.StatusInfo += "Подсистема " + items[4] + " не найдена в родительском TreeItem " + levelItem.TreeItemCode + ". ";
                                result.Add(relDoc);

                            }
                        }
                    }
                    else
                    {
                        VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                        {
                            RelationshipId = Guid.NewGuid(),
                            RightObject = rightObj
                        };
                        relDoc.StatusInfo += "Шифр документа отличается от маски. ";
                        result.Add(relDoc);

                    }
                }
            }

            return result;
        }
        private ObservableCollection<VMMC_Core.Relationship> GetRelFrom3D(ObservableCollection<VMMC_Core.Document> doc3Dlist)
        {
            ObservableCollection<VMMC_Core.Relationship> result = new ObservableCollection<VMMC_Core.Relationship>();

            foreach (VMMC_Core.Document document in doc3Dlist)
            {
                if (document.DocumentCode != null)
                {
                    if (document.DocumentCode != "")
                    {
                        VMMC_Core.DbObject docObj = new VMMC_Core.DbObject(sessionInfo)
                        {
                            ObjectId = document.DocumentId,
                            ObjectCode = document.DocumentCode,
                            ObjectName = document.DocumentName,
                            CreatedBy = sessionInfo.UserName,
                            LastModifiedBy = sessionInfo.UserName,

                        };

                        string[] items = document.DocumentCode.Replace("--", "-").Split('-');
                        if (items.Length > 7)
                        {
                            string[] newItem = new string[]
                            {
                                items[0],
                                items[1],
                                items[2],
                                items[3],
                                items[4] +"-"+items[5],
                                items[6],
                                items[7]
                            };
                            //if (newItem[4].IndexOf('(') > 0) newItem[4] = newItem[4].Remove(newItem[4].IndexOf('('));
                            //newItem[4] = newItem[4].Remove(newItem[4].IndexOf('('), newItem[4].Length - newItem[4].IndexOf('('));

                            items = newItem;
                        }


                        if (items.Length == 7)
                        {
                            VMMC_Core.TreeItem treeItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem("ELEMTREE", null);
                            if (items[1].Length == 2) items[1] = "00" + items[1];
                            VMMC_Core.TreeItem buildItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[1], treeItem.TreeItemId.ToString());
                            if (buildItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject buildObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = buildItem.TreeItemId,
                                    ObjectCode = buildItem.TreeItemCode,
                                    ObjectName = buildItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                    LeftObject = buildObj,
                                    RightObject = docObj
                                };
                                result.Add(relDoc);
                            }
                            else
                            {
                                if (items[1] == "") document.StatusInfo += "Шифр строения не обнаружен в коде. ";
                                else document.StatusInfo += "Строение " + items[1] + " не найдено. ";

                            }

                            string systemItemCode = items[4];
                            if (systemItemCode.IndexOf('-') > 0 & items[3] == "HV") systemItemCode = "SOV";
                            else if (systemItemCode.Length != 3 & items[3] == "HV") systemItemCode = "SOV";
                            //else { }

                            VMMC_Core.TreeItem systemItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(systemItemCode, buildItem.TreeItemId.ToString());
                            if (systemItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject systemObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = systemItem.TreeItemId,
                                    ObjectCode = systemItem.TreeItemCode,
                                    ObjectName = systemItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                    LeftObject = systemObj,
                                    RightObject = docObj

                                };
                                result.Add(relDoc);
                            }
                            else
                            {
                                if (items[4] == "") document.StatusInfo += "Шифр системы не обнаружен в коде. ";
                                else document.StatusInfo += "Система " + items[4] + " не найдена в родительском TreeItem " + buildItem.TreeItemCode + ". ";

                            }


                            VMMC_Core.TreeItem levelItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[2], systemItem.TreeItemId.ToString());
                            if (levelItem.TreeItemCode == null) levelItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem("В"/*кирилица*/+ items[2].Replace("B", ""), systemItem.TreeItemId.ToString()); //если в бд уровень на кирилице

                            if (levelItem.TreeItemCode != null)
                            {
                                VMMC_Core.DbObject levelObj = new VMMC_Core.DbObject(sessionInfo)
                                {
                                    ObjectId = levelItem.TreeItemId,
                                    ObjectCode = levelItem.TreeItemCode,
                                    ObjectName = levelItem.TreeItemName,
                                    CreatedBy = sessionInfo.UserName,
                                    LastModifiedBy = sessionInfo.UserName,

                                };

                                VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                {
                                    RelationshipId = Guid.NewGuid(),
                                    RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                    LeftObject = levelObj,
                                    RightObject = docObj

                                };
                                result.Add(relDoc);
                            }
                            else
                            {
                                if (items[2] == "") document.StatusInfo += "Шифр отметки не обнаружен в коде. ";
                                else document.StatusInfo += "Отметка " + items[2] + " не найдена в родительском TreeItem " + systemItem.TreeItemCode + ". ";

                            }

                            if (items[4].IndexOf('-') > 0)
                            {
                                VMMC_Core.TreeItem subsystemItem = new VMMC_Core.TreeItem(sessionInfo).getTreeItem(items[4], levelItem.TreeItemId.ToString());

                                if (subsystemItem.TreeItemCode != null)
                                {
                                    VMMC_Core.DbObject subsystemObj = new VMMC_Core.DbObject(sessionInfo)
                                    {
                                        ObjectId = subsystemItem.TreeItemId,
                                        ObjectCode = subsystemItem.TreeItemCode,
                                        ObjectName = subsystemItem.TreeItemName,
                                        CreatedBy = sessionInfo.UserName,
                                        LastModifiedBy = sessionInfo.UserName,

                                    };

                                    VMMC_Core.Relationship relDoc = new VMMC_Core.Relationship(sessionInfo)
                                    {
                                        RelationshipId = Guid.NewGuid(),
                                        RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Элемент дерева-Документ"),
                                        LeftObject = subsystemObj,
                                        RightObject = docObj

                                    };
                                    result.Add(relDoc);
                                }
                                else
                                {
                                    if (items[2] == "") document.StatusInfo += "Шифр подсистемы не обнаружен в коде. ";
                                    else document.StatusInfo += "Подсистема " + items[4] + " не найдена в родительском TreeItem " + levelItem.TreeItemCode + ". ";

                                }
                            }
                        }
                        else document.StatusInfo += "Шифр документа отличается от маски. ";
                    }
                }
            }
            return result;
        }


        private static string Lats_Char_Found(string position)
        {
            string result = "";
            for (int c = 0; c < position.Length; c++)
            {
                if ((position[c] >= 'А' && position[c] <= 'Я') || (position[c] >= 'а' && position[c] <= 'я') || position[c] == 'Ё' || position[c] == 'ё') //нахождение русских символов А-Я, а-я и отдельно для Ё-ё.
                {
                    result = result + position[c] + ","; // вывод русских символов через запятую
                }
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1); // убрать последний символ (в данном случае запятую). Пример А,Я
            }
            return result;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnExcelParserPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
