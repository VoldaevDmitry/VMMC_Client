using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;


using System.Collections.ObjectModel;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace VMMC_FileParser
{

    public class FileParserViewModel : INotifyPropertyChanged
    {
        VMMC_Core.SessionInfo sessionInfo;      
        public ObservableCollection<VMMC_Core.Document> dbDocumentsCollection;


        public string folderPath;  
        public string FolderPath 
        { 
            get { return folderPath; }
            //set 
            //{
            //    folderPath = value;
            //    OnFileParserPropertyChanged("FolderPath");
            //    OnFileParserPropertyChanged("FilesCollection");
            //    OnFileParserPropertyChanged("ComplektsCollection");
            //    OnFileParserPropertyChanged("DocumentsCollection");
            //    OnFileParserPropertyChanged("RevisionsCollection");
            //}
        }


        public ObservableCollection<VMMC_Core.LocalFile> localFilesCollection;
        public ObservableCollection<VMMC_Core.LocalFile> LocalFilesCollection
        {
            get { return localFilesCollection; }            
        }


        public ObservableCollection<VMMC_Core.Files> filesCollection;
        public ObservableCollection<VMMC_Core.Files> FilesCollection
        {
            get { return filesCollection; }            
        }


        public ObservableCollection<VMMC_Core.Complekt> complektsCollection;
        public ObservableCollection<VMMC_Core.Complekt> ComplektsCollection
        {
            get { return complektsCollection; }
        }


        public ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get
            {
                return documentsCollection;
            }
        }


        public ObservableCollection<VMMC_Core.Relationship> relationshipsCollection;
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsCollection
        {
            get
            {
                return relationshipsCollection;
            }
        }


        private bool silentMode;
        public bool SilentMode
        {
            get { return silentMode; }
            set
            {
                silentMode = value;
                OnFileParserPropertyChanged("SilentMode");
            }
        }

        private bool hasParentWindow;
        public bool HasParentWindow
        {
            get { return hasParentWindow; }
            set
            {
                hasParentWindow = value;
                OnFileParserPropertyChanged("HasParentWindow");
            }
        }


        public FileParserViewModel(VMMC_Core.SessionInfo session, string path) 
        {
            folderPath = path;
            if (path != "")
            {
                sessionInfo = session;
                dbDocumentsCollection = new VMMC_Core.Document(session).GetDbDocumentsList();
                localFilesCollection = GetFilesFromFolder(folderPath);
                complektsCollection = GetComplektsCollection(localFilesCollection);
                documentsCollection = GetDocumentsCollection(localFilesCollection);
                relationshipsCollection = GetRelationshipsCollection(localFilesCollection);
                //localFilesCollection = SortLocalFiles();
                //foreach (LocalFile file in filesCollection) 
                //{
                //    file.Checksum = file.ComputeMD5ChecksumAsync(file.LocalFilePath).Result;
                //}

            }
        }


        public ObservableCollection<VMMC_Core.LocalFile> GetFilesFromFolder(string path) 
        {
            ObservableCollection<VMMC_Core.LocalFile> filesList = new ObservableCollection<VMMC_Core.LocalFile>();
            if (path != null)
            {
                string[] allfiles = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                foreach (string filepath in allfiles)
                {
                    FileInfo fi = new FileInfo(filepath);
                    int rev = 0;
                    string revStr = FindCode(fi.Name)[2];
                    if (revStr != "")
                    {
                        revStr = revStr.Substring(3);
                        revStr = revStr.Trim(new char[] { '-', '_', '.', ' ' });
                        //bool success = Int32.TryParse(revStr, out rev); //  TryParse(revision_str);
                        rev = int.Parse(revStr);
                    }

                    //LocalFile newLocalFile = new LocalFile(filepath)
                    //{
                    //    //LocalFileName = fi.Name,
                    //    //LocalFileType = fi.Extension,
                    //    LocalFilePath = filepath,
                    //    DocumentCode = FindCode(fi.Name)[0],
                    //    DocumentName = FindCode(fi.Name)[1],
                    //    DocumentRevision = rev, //int.Parse(FindCode(fi.Name)[2]),
                    //    ComplektCode = FindCode(fi.Name)[3],
                    //    ComplektName = "Комплект документации "
                    //};

                    VMMC_Core.LocalFile newLocalFile = new VMMC_Core.LocalFile(filepath);

                    VMMC_Core.Revision newRevision = new VMMC_Core.Revision(sessionInfo);
                    VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo);
                    VMMC_Core.Complekt newComplekt = new VMMC_Core.Complekt(sessionInfo);

                    string[] findCodeResult = newLocalFile.FindCode(fi.Name);

                    newDocument.DocumentId = Guid.NewGuid();
                    newDocument.DocumentCode = findCodeResult[0];
                    if (newDocument.DocumentCode == "") newDocument.DocumentCode = fi.Name;
                    newDocument.DocumentName = findCodeResult[1];
                    newDocument.Status = "NEW";

                    newRevision.DocumentId = newDocument.DocumentId;
                    newRevision.RevisionId = Guid.NewGuid();
                    newRevision.Number = newLocalFile.FindRevision(fi.Name);
                    newRevision.Status = "NEW";

                    newDocument.Revisions = new ObservableCollection<VMMC_Core.Revision>();
                    newDocument.Revisions.Add(newRevision);

                    newComplekt.ComplektId = Guid.NewGuid();
                    newComplekt.ComplektCode = findCodeResult[3];
                    newComplekt.ComplektName = "Комплект документации " + newComplekt.ComplektCode;
                    newComplekt.Status = "NEW";

                    VMMC_Core.DbObject ComplektObj = new VMMC_Core.DbObject(sessionInfo)
                    {
                        ObjectId = newComplekt.ComplektId,
                        ObjectCode = newComplekt.ComplektCode,
                        ObjectName = newComplekt.ComplektName,
                        CreatedBy = sessionInfo.UserName,
                        LastModifiedBy = sessionInfo.UserName,

                    };
                    VMMC_Core.DbObject docObj = new VMMC_Core.DbObject(sessionInfo)
                    {
                        ObjectId = newDocument.DocumentId,
                        ObjectCode = newDocument.DocumentCode,
                        ObjectName = newDocument.DocumentName,
                        CreatedBy = sessionInfo.UserName,
                        LastModifiedBy = sessionInfo.UserName,
                    };

                    VMMC_Core.Relationship newRelationship = new VMMC_Core.Relationship(sessionInfo)
                    {
                        RelationshipId = Guid.NewGuid(),
                        RelTypeId = new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Комплект-Документ"),
                        LeftObject = ComplektObj,
                        RightObject = docObj

                    };

                    newLocalFile.Document = newDocument;
                    newLocalFile.Complekt = newComplekt;
                    newLocalFile.Revision = newRevision;
                    newLocalFile.Relationship = newRelationship;
                    //newLocalFile.DocumentCode = findCodeResult[0];
                    //newLocalFile.DocumentName = findCodeResult[1];
                    //newLocalFile.Revision.DocumentRevision = newLocalFile.FindRevision(fi.Name); 
                    //newLocalFile.Complekt.ComplektCode = findCodeResult[3];
                    //newLocalFile.Complekt.ComplektName = "Комплект документации ";


                    filesList.Add(newLocalFile);
                }
            }

            //OnFileParserPropertyChanged("FilesCollection");
            return filesList;
        }
        public ObservableCollection<VMMC_Core.Complekt> GetComplektsCollection(ObservableCollection<VMMC_Core.LocalFile> filesList)
        {
            ObservableCollection<VMMC_Core.Complekt> complektsList = new ObservableCollection<VMMC_Core.Complekt>();

            Guid rdClassId = Guid.NewGuid();

            foreach (VMMC_Core.LocalFile file in filesList)
            {
                Guid complektId = Guid.NewGuid();
                bool isExist = false;
                foreach (VMMC_Core.Complekt complekt in complektsList)
                {
                    if (complekt.ComplektCode == file.Complekt.ComplektCode)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist) complektsList.Add(new VMMC_Core.Complekt(sessionInfo) { ComplektId = file.Complekt.ComplektId, ComplektCode = file.Complekt.ComplektCode, ComplektName = "Комплект документации " + file.Complekt.ComplektCode, ComplektClassId = rdClassId });
            }
            //OnFileParserPropertyChanged("ComplektsCollection");
            return complektsList;
        }
        public ObservableCollection<VMMC_Core.Relationship> GetRelationshipsCollection(ObservableCollection<VMMC_Core.LocalFile> filesList)
        {
            ObservableCollection<VMMC_Core.Relationship> relationshipsList = new ObservableCollection<VMMC_Core.Relationship>();

            Guid rdClassId = Guid.NewGuid();

            foreach (VMMC_Core.LocalFile file in filesList)
            {
                Guid complektId = Guid.NewGuid();
                bool isExist = false;
                foreach (VMMC_Core.Relationship relationship in relationshipsList)
                {
                    if (relationship.LeftObject.ObjectCode == file.Relationship.LeftObject.ObjectCode & relationship.RightObject.ObjectCode == file.Relationship.RightObject.ObjectCode)
                    {
                        isExist = true;
                        break;
                    }
                }
                if (!isExist) relationshipsList.Add(new VMMC_Core.Relationship(sessionInfo) { RelationshipId = file.Relationship.RelationshipId, RelTypeId = file.Relationship.RelTypeId, LeftObject = file.Relationship.LeftObject, RightObject = file.Relationship.RightObject });
            }
            //OnFileParserPropertyChanged("ComplektsCollection");
            return relationshipsList;
        }
        public ObservableCollection<VMMC_Core.Document> GetDocumentsCollection(ObservableCollection<VMMC_Core.LocalFile> localFilesList)
        {
            ObservableCollection<VMMC_Core.Document> documentsList = new ObservableCollection<VMMC_Core.Document>();
            ObservableCollection<VMMC_Core.Files> filesList = new ObservableCollection<VMMC_Core.Files>();

            Guid rdClassId = Guid.NewGuid();


            foreach (VMMC_Core.LocalFile localFile in localFilesList)
            {
                Guid documentId = localFile.Document.DocumentId;
                Guid revisionId = Guid.NewGuid();
                Guid fileId = Guid.NewGuid();
                bool isDocumentExist = false;
                bool isRevisionExist = false;
                bool isFileExist = false;
                foreach (VMMC_Core.Document document in documentsList)
                {
                    if (document.DocumentCode == localFile.Document.DocumentCode)
                    {
                        isDocumentExist = true;
                        documentId = document.DocumentId;
                        foreach (VMMC_Core.Revision revision in document.Revisions)
                        {
                            if (revision.Number == localFile.Revision.Number)
                            {
                                isRevisionExist = true;
                                //revisionId = revision.RevisionId;
                                //foreach (VMMC_Core.Files files in revision.Files)
                                //{
                                //    if (files.LocalPath == localFile.LocalFilePath)
                                //    {
                                //        isFileExist = true;
                                //        break;
                                //    }
                                //}
                                break;
                            }
                        }
                        break;
                    }
                }
                if (!isDocumentExist)
                {
                    //VMMC_Core.Files newFile = new VMMC_Core.Files {  FileId= fileId, RevisionId = revisionId };
                    VMMC_Core.Revision newRev = new VMMC_Core.Revision(sessionInfo) { RevisionId = revisionId, DocumentId = documentId, Number = localFile.Revision.Number, IsCurrent = true };
                    ObservableCollection<VMMC_Core.Revision> newRevList = new ObservableCollection<VMMC_Core.Revision>();
                    newRevList.Add(newRev);
                    documentsList.Add(new VMMC_Core.Document(sessionInfo) { DocumentId = localFile.Document.DocumentId, DocumentCode = localFile.Document.DocumentCode, DocumentName = localFile.Document.DocumentName, DocumentClassId = rdClassId, Revisions = newRevList });
                }
                else if (isDocumentExist & !isRevisionExist)
                {
                    VMMC_Core.Document existDocument = documentsList.Where(x => x.DocumentId == documentId).FirstOrDefault();
                    ObservableCollection<VMMC_Core.Revision> RevList = existDocument.Revisions;
                    int maxRev = 0;
                    bool isCurent = false;
                    foreach (VMMC_Core.Revision rev in RevList)
                    {
                        if (rev.Number > maxRev) maxRev = rev.Number;
                    }
                    if (maxRev < localFile.Revision.Number) isCurent = true;
                    VMMC_Core.Revision newRev = new VMMC_Core.Revision(sessionInfo) { RevisionId = revisionId, DocumentId = documentId, Number = localFile.Revision.Number, IsCurrent = isCurent };
                    RevList.Add(newRev);
                }


                isFileExist = false;
                foreach (VMMC_Core.Files file in filesList)
                {
                    if (file.LocalPath == localFile.LocalFilePath)
                    {
                        isFileExist = true;
                        break;
                    }
                }
                if (!isFileExist) filesList.Add(new VMMC_Core.Files(sessionInfo) { FileGuid = fileId , FileName= localFile.LocalFileName, FileType = localFile.LocalFileType, LocalPath= localFile.LocalFilePath, Checksum= localFile.Checksum, RevisionId = revisionId });
                


            }
            documentsCollection = documentsList;
            filesCollection = filesList;
            //OnFileParserPropertyChanged("DocumentsCollection");
            return documentsList;
        }
        public void SortLocalFiles()
        {            
            foreach (VMMC_Core.LocalFile  localFile in localFilesCollection) 
            {
                localFile.Document = documentsCollection.Where(x => x.DocumentCode == localFile.Document.DocumentCode ).FirstOrDefault();
                localFile.Complekt = complektsCollection.Where(x => x.ComplektCode == localFile.Complekt.ComplektCode ).FirstOrDefault();
            }

            foreach (VMMC_Core.LocalFile localFile in localFilesCollection)
            {
                VMMC_Core.Document doc = dbDocumentsCollection.Where(x => x.DocumentCode == localFile.Document.DocumentCode).FirstOrDefault();
                if (doc != null)
                {
                    doc.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(doc.DocumentId);
                    foreach (VMMC_Core.Revision rev in localFile.Document.Revisions)
                    {
                        VMMC_Core.Revision newRev = doc.Revisions.Where(x => x.Number == rev.Number).FirstOrDefault();
                        if (newRev == null) doc.Revisions.Add(rev);
                        else localFile.Revision = newRev;
                    }
                    localFile.Document = doc;
                }
            }

            //OnFileParserPropertyChanged("FilesCollection");
            //return localFilesList;
        }        
        private string[] FindCode(string nameFile)
        {
            //создаю правила поиска шифра:
            /*
            string pattern1 = @"^\w{4}-\w{2}-\d{2}-.*-\w{1}-\d{4}-\d{4}";           //ВММК-РД-04-ВК.2-Ч-0006-0009-План заглубленного трубопровода систем канализации и водостока. Части 1-4.dwg
            string pattern2 = @"^\w{4}-\w{2}-\d{2}-.*-\w{1}-\d{4}";                 //ВММК-РД-04-ЭОМ-Ч-0003.dwg
            string pattern3 = @"^\w{4}-\w{2}-\d{2}-.*-\w{1}-\d{3}-\d{3}";           //ВММК-РД-08-КЖ-02.3-Ч-001-002-Изм.3_Общие данные.dwg
            string pattern4 = @"^\w{4}-\w{2}-\d{2}-.*-\w{1}-\d{3}";                 //ВММК-РД-08-КЖ-02.1-Ч-002-Изм.2-Плита перекрытия на отм. -1,500 (сектор 16). Опалубка.pdf
            string pattern5 = @"^\w{4}-\w{2}-\d{2}-\w{2}-\d{2}\.(\d{1}|\d{2})";     //ВММК-РД-08-КЖ-02.10_Титульный лист.docx
            string pattern6 = @"^\w{4}-\w{2}-\d{2}-\w{2}\.\d*\.\w{2}";              //ВММК-РД-08-ВК.2.СО-Спецификация оборудования, изделий и материалов.dwg
            string pattern7 = @"^\w{4}-\w{2}-\d{2}-\w{3}-\w{2}";                    //ВММК-РД-05-ЭОМ-СО.pdf
            */
            //string pattern1 = @"^\w{4}-\w{2}-\d{2}-.*-.*-(\w{1}|\w{2}|\w{1}-d{1})-(\d{4}-\d{4}|\d{4}|\d{3}-\d{3}|\d{3})(_|-)Изм.\d{1}";

            string pattern1 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)((_\s)|_|-|\s|\.)((И|и)зм(\.|-|_\s|_|\s)\d+)?";

            string pattern2 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)";

            string pattern3 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})))";

            string pattern4 = @"((И|и)зм(\.|-|_\s|_|\s)\d+)";


            string match1 = Regex.Match(nameFile, pattern1).ToString();
            match1 = match1.Replace("ВК.2.", "ВК-2-");
            match1 = match1.Replace("ВК.2", "ВК-2");

            string DocCode = Regex.Match(match1, pattern2).ToString();
            DocCode = DocCode.Trim(new char[] { '-', '_', '.', ' ' });

            string DocName = Regex.Replace(nameFile, pattern1, "");
            DocName = DocName.Substring(0, DocName.LastIndexOf(".") + 1);
            DocName = DocName.Trim(new char[] { '-', '_', '.', ' ' });

            string DocRevision = Regex.Match(match1, pattern4).ToString();
            string DocSetCode = Regex.Match(DocCode, pattern3).ToString();

            string[] result = new string[] { DocCode, DocName, DocRevision, DocSetCode };

            return result;
        }
        public void FillLocalFilesChecksum()
        {
            foreach (VMMC_Core.LocalFile file in LocalFilesCollection)
            {
                if (file.Checksum == null) file.Checksum = file.ComputeMD5Checksum(file.LocalFilePath);
                else if (file.Checksum.Length != 32) file.Checksum = file.ComputeMD5Checksum(file.LocalFilePath);

            }
        }
        public void CheckLocalFilesList()
        {
            ObservableCollection<VMMC_Core.Files> dbFilesCollection = new VMMC_Core.Files(sessionInfo).GetDbFiles();

            foreach (VMMC_Core.LocalFile file in LocalFilesCollection)
            {
                string statusInfo = "";
                string status = "OK";

                //statusInfo += CheckChecksum(file);
                string checkChecksumResult = file.CheckChecksum();
                statusInfo += checkChecksumResult;
                if (checkChecksumResult != "") status = "Error";

                //statusInfo +=CheckForDublicatesInList(file, filesCollection);
                string checkForDublicatesInListResult = file.CheckForDublicatesInList(LocalFilesCollection);
                statusInfo += checkForDublicatesInListResult;
                if (checkForDublicatesInListResult != "" & status != "Error") status = "Warning";

                //statusInfo += CheckForDublicatesInDataBase(file, dbFilesCollection);
                string checkForDublicatesInDataBaseResult = file.CheckForDublicatesInDataBase(dbFilesCollection);
                statusInfo += checkForDublicatesInDataBaseResult;
                if (checkForDublicatesInDataBaseResult != "" & status != "Error") status = "Warning";

                //statusInfo += CheckDocument(file);
                string checkDocumentResult = file.CheckDocumentInfo();
                statusInfo += checkDocumentResult;
                if (checkDocumentResult != "") status = "Error";

                //statusInfo += CheckComplekt(file);
                string checkComplektResult = file.CheckComplektinfo();
                statusInfo += checkComplektResult;
                if (checkComplektResult != "") status = "Error";

                if (file.Checksum == null) file.Checksum = file.ComputeMD5Checksum(file.LocalFilePath);
                else if (file.Checksum.Length != 32) file.Checksum = file.ComputeMD5Checksum(file.LocalFilePath);

                file.status = status;
                file.statusInfo = statusInfo;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnFileParserPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
