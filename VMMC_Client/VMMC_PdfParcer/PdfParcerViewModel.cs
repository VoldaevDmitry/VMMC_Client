using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.Crmf;
using Tesseract;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Windows.Media.Imaging;
using iTextSharp.text.pdf.codec;

using PdfiumViewer; //apache 2.0
using VMMC_Core;
using System.Reflection.Emit;

namespace VMMC_PdfParcer
{
    public class PdfParcerViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.SessionInfo sessionInfo;

        public string sourcePdfPath;
        public string folderPath;
        public List<VMMC_Core.LocalFile> localFilesCollection;
        //public List<VMMC_Core.Revision> revisionsCollection;


        DataSet ParsingPdfDS;
        DataSet ExportListDS;
        Bitmap Original_Bitmap;
        Bitmap Display_Bitmap;
        //string saveFolderPath = "";
        public string inputPdf = "";

        int DPIvalue = 0;

        string DocumentSetCode = "ВММК-ИД-XX-AAA-YY";
        string DocumentSetCodeRelationship = "ВММК-РД-XX-AAA-YY";

        
        public SelectSourcePdfViewModel SelectSourcePdfViewModel
        {
            get { return selectSourcePdfViewModel; }
            set
            {
                selectSourcePdfViewModel = value;
                OnPdfParcerPropertyChanged("SelectSourcePdfViewModel");
            }
        }
        private SelectSourcePdfViewModel selectSourcePdfViewModel;

        public AnalizePdfPagesViewModel AnalizePdfPagesViewModel
        {
            get { return analizePdfPagesViewModel; }
            set
            {
                analizePdfPagesViewModel = value;
                OnPdfParcerPropertyChanged("AnalizePdfPagesViewModel");
            }
        }
        private AnalizePdfPagesViewModel analizePdfPagesViewModel;
        public EditDocumentAttributesViewModel EditDocumentAttributesViewModel
        {
            get { return editDocumentAttributesViewModel; }
            set
            {
                editDocumentAttributesViewModel = value;
                OnPdfParcerPropertyChanged("EditDocumentAttributesViewModel");
            }
        }
        private EditDocumentAttributesViewModel editDocumentAttributesViewModel;




        public ObservableCollection<VMMC_Core.Complekt> ComplektsCollection
        {
            get { return complektsCollection; }
            set
            {
                complektsCollection = value;
                OnPdfParcerPropertyChanged("ComplektsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Complekt> complektsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnPdfParcerPropertyChanged("DocumentsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValuesCollection
        {
            get { return attributeObjectValuesCollection; }
            set
            {
                attributeObjectValuesCollection = value;
                OnPdfParcerPropertyChanged("AttributeObjectValuesCollection");
            }
        }
        private ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesCollection;
        public ObservableCollection<VMMC_Core.Tag> TagsCollection
        {
            get { return tagsCollection; }
            set
            {
                tagsCollection = value;
                OnPdfParcerPropertyChanged("TagsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Tag> tagsCollection;
        public ObservableCollection<VMMC_Core.TreeItem> TreeItemsCollection
        {
            get { return treeItemsCollection; }
            set
            {
                treeItemsCollection = value;
                OnPdfParcerPropertyChanged("TreeItemsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.TreeItem> treeItemsCollection;
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsCollection
        {
            get { return relationshipsCollection; }
            set
            {
                relationshipsCollection = value;
                OnPdfParcerPropertyChanged("RelationshipsCollection");
            }
        }
        private ObservableCollection<VMMC_Core.Relationship> relationshipsCollection;
        public ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> AttributeObjectValueViewModelCollection
        {
            get { return attributeObjectValueViewModelCollection; }
            set
            {
                attributeObjectValueViewModelCollection = value;
                OnPdfParcerPropertyChanged("AttributeObjectValueViewModelCollection");
            }
        }
        private ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> attributeObjectValueViewModelCollection;


        public VMMC_Core.RelayCommand ParcingPdfCommand
        {
            get
            {
                return parcingPdfCommand ??
                  (parcingPdfCommand = new RelayCommand(obj => { Parcing(); }));
            }
        }
        private VMMC_Core.RelayCommand parcingPdfCommand;
        public VMMC_Core.RelayCommand ShowSelectSourcePdfViewCommand
        {
            get
            {
                return showSelectSourcePdfViewCommand ??
                  (showSelectSourcePdfViewCommand = new RelayCommand(obj => {
                      SelectSourcePdfViewModel.SelectSourcePdfViewVisibility = Visibility.Visible;
                      AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility = Visibility.Hidden;
                      EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility = Visibility.Hidden;
                  }));
            }
        }
        private VMMC_Core.RelayCommand showSelectSourcePdfViewCommand;
        public VMMC_Core.RelayCommand ShowAnalizePdfPagesViewCommand
        {
            get
            {
                return showAnalizePdfPagesViewCommand ??
                  (showAnalizePdfPagesViewCommand = new RelayCommand(obj => {
                      SelectSourcePdfViewModel.SelectSourcePdfViewVisibility = Visibility.Hidden;
                      AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility = Visibility.Visible;
                      EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility = Visibility.Hidden;
                  }));
            }
        }
        private VMMC_Core.RelayCommand showAnalizePdfPagesViewCommand;
        public VMMC_Core.RelayCommand ShowEditDocumentAttributesViewCommand
        {
            get
            {
                return showEditDocumentAttributesViewCommand ??
                  (showEditDocumentAttributesViewCommand = new RelayCommand(obj => {
                      SelectSourcePdfViewModel.SelectSourcePdfViewVisibility = Visibility.Hidden;
                      AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility = Visibility.Hidden;
                      EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility = Visibility.Visible;
                  }));
            }
        }
        private VMMC_Core.RelayCommand showEditDocumentAttributesViewCommand;

        public bool SilentMode
        {
            get { return silentMode; }
            set
            {
                silentMode = value;
                OnPdfParcerPropertyChanged("SilentMode");
            }
        }
        private bool silentMode;
        public bool HasParentWindow
        {
            get { return hasParentWindow; }
            set
            {
                hasParentWindow = value;
                OnPdfParcerPropertyChanged("HasParentWindow");
            }
        }
        private bool hasParentWindow;



        public bool IsPDFselected
        {
            get
            {
                if (SelectSourcePdfViewModel.SourcePdfPath != null && SelectSourcePdfViewModel.SourcePdfPath != String.Empty) return true;
                else return false;
            }
        }
        public bool IsPDFparsed
        {
            get { return isPDFparsed; }
            set
            {
                isPDFparsed = value;
                OnPdfParcerPropertyChanged("IsPDFparsed");
            }
        }
        private bool isPDFparsed;
        public bool IsPDFsplited
        {
            get { return isPDFsplited; }
            set
            {
                isPDFsplited = value;
                OnPdfParcerPropertyChanged("IsPDFsplited");
            }
        }
        private bool isPDFsplited;


        public bool IsSelectSourcePdfViewAvailable
        {
            get { return true; }
        }
        public bool IsAnalizePdfPagesViewAvailable
        {
            get
            {
                if (IsPDFselected && IsPDFparsed) return true;
                else return false;
            }
        }
        public bool IsEditDocumentAttributesViewAvailable
        {
            get
            {
                if (IsPDFselected && IsPDFparsed && IsPDFsplited) return true;
                else return false;
            }
        }
        public string NextStepButtonContent
        {
            get
            {
                if (EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility == Visibility.Visible) return "Готово";
                else return "Далее";
            }
        }


        public PdfParcerViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

            IsPDFparsed = false;
            IsPDFsplited = false;

            VMMC_Core.Complekt complektID = new Complekt(sessionInfo);
            complektID.ComplektCode = "ВММК-ИД-XX-AAA-YY";
            DocumentSetCode = complektID.ComplektCode;

            VMMC_Core.Complekt complektRD = new Complekt(sessionInfo);
            complektRD.ComplektCode = "ВММК-РД-XX-AAA-YY";
            DocumentSetCodeRelationship = complektRD.ComplektCode;

            SelectSourcePdfViewModel = new SelectSourcePdfViewModel(sessionInfo);
            //selectSourcePdfViewModel.SourcePdfPath = @"C:\temp\test.pdf";
            SelectSourcePdfViewModel.ComplektID = complektID;
            SelectSourcePdfViewModel.ComplektRD = complektRD;
            SelectSourcePdfViewModel.ComplektsRDCollection = new ObservableCollection<Complekt>();
            SelectSourcePdfViewModel.SelectSourcePdfViewVisibility = Visibility.Visible;


            AnalizePdfPagesViewModel = new AnalizePdfPagesViewModel(sessionInfo);
            AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility = Visibility.Hidden;

            EditDocumentAttributesViewModel = new EditDocumentAttributesViewModel(sessionInfo);
            EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility = Visibility.Hidden;

        }

        public void NextCommand()
        {
            if (SelectSourcePdfViewModel.SelectSourcePdfViewVisibility == Visibility.Visible)
            {
                if (IsPDFselected && !IsPDFparsed && !IsPDFsplited)
                {
                    Parcing();

                    SelectSourcePdfViewModel.SelectSourcePdfViewVisibility = Visibility.Hidden;
                    EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility = Visibility.Hidden;

                    AnalizePdfPagesViewModel.SourcePdfPath = SelectSourcePdfViewModel.SourcePdfPath;
                    AnalizePdfPagesViewModel.DocumentsCollection = DocumentsCollection;
                    AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility = Visibility.Visible;

                }
                else if (IsPDFselected && IsPDFparsed && !IsPDFsplited)
                {

                }

            }
            else if (AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility == Visibility.Visible)
            {
                if (IsPDFselected && IsPDFparsed && !IsPDFsplited)
                {
                    Spliting();

                    SelectSourcePdfViewModel.SelectSourcePdfViewVisibility = Visibility.Hidden;
                    AnalizePdfPagesViewModel.AnalizePdfPagesViewVisibility = Visibility.Hidden;

                    EditDocumentAttributesViewModel.DocumentsCollection = DocumentsCollection;
                    EditDocumentAttributesViewModel.EditDocumentAttributesViewVisibility = Visibility.Visible;

                }
                else if (IsPDFselected && IsPDFparsed && IsPDFsplited)
                {

                }
            }
        }

        private void Parcing()
        {
            if (IsPDFselected) 
            {
                AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

                DocumentsCollection = null;
                ComplektsCollection = null;
                TagsCollection = null;
                TreeItemsCollection = null;
                RelationshipsCollection = null;

                PdfParcerViewModel newPdfParcerViewModel = Parcing(SelectSourcePdfViewModel.SourcePdfPath);

                if (DocumentsCollection == null) DocumentsCollection = newPdfParcerViewModel.DocumentsCollection;
                else foreach (VMMC_Core.Document document in newPdfParcerViewModel.DocumentsCollection) DocumentsCollection.Add(document);

                if (TagsCollection == null) TagsCollection = newPdfParcerViewModel.TagsCollection;
                else foreach (VMMC_Core.Tag tag in newPdfParcerViewModel.TagsCollection) TagsCollection.Add(tag);

                if (ComplektsCollection == null) ComplektsCollection = newPdfParcerViewModel.ComplektsCollection;
                else foreach (VMMC_Core.Complekt complekt in newPdfParcerViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

                if (RelationshipsCollection == null) RelationshipsCollection = newPdfParcerViewModel.RelationshipsCollection;
                else foreach (VMMC_Core.Relationship relationship in newPdfParcerViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

                if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newPdfParcerViewModel.AttributeObjectValuesCollection;
                else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newPdfParcerViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
                if (AttributeObjectValuesCollection != null)
                {
                    foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
                    {
                        AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel() { AttributeObjectValue = aov });
                    }
                }
                if (TreeItemsCollection == null) TreeItemsCollection = newPdfParcerViewModel.TreeItemsCollection;
                else foreach (VMMC_Core.TreeItem treeItem in newPdfParcerViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);
            }
            IsPDFparsed = true;
            
            //using (var dialog = new System.Windows.Forms.OpenFileDialog())
            //{
            //    dialog.Multiselect = true;

            //    DocumentsCollection = null;
            //    ComplektsCollection = null;
            //    TagsCollection = null;
            //    TreeItemsCollection = null;
            //    RelationshipsCollection = null;

            //    dialog.ShowDialog();


            //    if (dialog.FileNames != null)
            //    {
            //        foreach (string fileName in dialog.FileNames)
            //        {
            //            AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();

            //            PdfParcerViewModel newPdfParcerViewModel = Parcing(fileName);

            //            if (DocumentsCollection == null) DocumentsCollection = newPdfParcerViewModel.DocumentsCollection;
            //            else foreach (VMMC_Core.Document document in newPdfParcerViewModel.DocumentsCollection) DocumentsCollection.Add(document);

            //            if (TagsCollection == null) TagsCollection = newPdfParcerViewModel.TagsCollection;
            //            else foreach (VMMC_Core.Tag tag in newPdfParcerViewModel.TagsCollection) TagsCollection.Add(tag);

            //            if (ComplektsCollection == null) ComplektsCollection = newPdfParcerViewModel.ComplektsCollection;
            //            else foreach (VMMC_Core.Complekt complekt in newPdfParcerViewModel.ComplektsCollection) ComplektsCollection.Add(complekt);

            //            if (RelationshipsCollection == null) RelationshipsCollection = newPdfParcerViewModel.RelationshipsCollection;
            //            else foreach (VMMC_Core.Relationship relationship in newPdfParcerViewModel.RelationshipsCollection) RelationshipsCollection.Add(relationship);

            //            if (AttributeObjectValuesCollection == null) AttributeObjectValuesCollection = newPdfParcerViewModel.AttributeObjectValuesCollection;
            //            else foreach (VMMC_Core.AttributeObjectValue attributeObjectValue in newPdfParcerViewModel.AttributeObjectValuesCollection) AttributeObjectValuesCollection.Add(attributeObjectValue);
            //            if (AttributeObjectValuesCollection != null)
            //            {
            //                foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
            //                {
            //                    AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel() { AttributeObjectValue = aov });
            //                }
            //            }
            //            if (TreeItemsCollection == null) TreeItemsCollection = newPdfParcerViewModel.TreeItemsCollection;
            //            else foreach (VMMC_Core.TreeItem treeItem in newPdfParcerViewModel.TreeItemsCollection) TreeItemsCollection.Add(treeItem);

            //        }
            //    }
            //}
        }
        private PdfParcerViewModel Parcing(string inputPdfFilePath) // This event handler is where the time-consuming work is done.
        {
            PdfParcerViewModel pdfParserViewModel = new PdfParcerViewModel(sessionInfo);
            ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();

            string outputSplit = string.Empty;
            int DPI = (int)(DPIvalue);

            //string[] prevDocInfo;
            FileInfo pdfFileInfo = new FileInfo(inputPdfFilePath);
            int currentDoc = 0;
            //string[] DocInfo;
            //string[] prevDocInfo;

            int firstSubdocumentPage = 0;
            int lastSubdocumentPage = 0;

            string levReslt = "";
            int pageCounnt = PDF_PageCount(inputPdfFilePath);
            string FileName = "";
            //pageCounnt = 10;

            

            //DataRow row = FilesDataTable.NewRow();



            for (int currentPage = 1; currentPage <= pageCounnt; currentPage++)
            //for (int currentPage = 70; currentPage < 80; currentPage++)
            {


                int countRows = pageCounnt;
                int CurrentRow = currentPage;

                string altFileNeme = "";
                string FileType = ".pdf";
                string DocCode = "";
                string DocName = "";
                int DocRevision = 0;//все ИД помечаются 0 ревизией
                string DocumentClass = "";
                string DocumentClassCode = "";

                if (currentPage == 1 || currentPage > lastSubdocumentPage)//int.Parse(prevDocInfo[6]))
                {

                    if (LevenshteinDistance(recognize(inputPdfFilePath, currentPage, 1, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }), "АКТосвидетельствованияскрытыхработ") < 10)
                    //if (recognize(inputPdf, currentPage, 1, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }) == "АКТосвидетельствованияскрытыхработ")

                    {
                        currentDoc++;

                        DocumentClass = "Акт освидетельствования скрытых работ";
                        DocumentClassCode = "АОСР";
                        firstSubdocumentPage = currentPage;
                        lastSubdocumentPage = currentPage;
                        DocName = recognize(inputPdfFilePath, currentPage, 3, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "'").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });


                        for (int lastPage = currentPage + 1; lastPage < pageCounnt; lastPage++)
                        {


                            //DocName = recognize(inputPdf, lastPage, 5, 2, 100);
                            string str = recognize(inputPdfFilePath, lastPage, 3, 2, 190).Replace("\n", " ").Trim(new char[] { ' ' });


                            if (str != "")
                            {
                                if (DocName.Length < str.Length) str = str.Substring(0, DocName.Length);
                                levReslt = "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString();

                                if (LevenshteinDistance(DocName, str) < DocName.Length * 0.2)
                                {
                                    lastSubdocumentPage = lastPage;
                                    //MessageBox.Show(DocName + ": " + DocName.Length.ToString() + "\n" + str + ": " + str.Length.ToString() + "\n" + "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //currentPage = lastPage-1;   
                                }
                                else
                                {
                                    str = recognize(inputPdfFilePath, lastPage, 3, 2, 230).Replace("\n", " ").Trim(new char[] { ' ' });
                                    if (DocName.Length < str.Length) str = str.Substring(0, DocName.Length);
                                    levReslt = "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString();
                                    if (LevenshteinDistance(DocName, str) < DocName.Length * 0.5)
                                    {
                                        lastSubdocumentPage = lastPage;
                                        //MessageBox.Show(DocName + ": " + DocName.Length.ToString() + "\n" + str + ": " + str.Length.ToString() + "\n" + "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //currentPage = lastPage-1;   
                                    }
                                    else break;

                                }
                            }
                            else break;

                        }
                    }
                    else if (LevenshteinDistance(recognize(inputPdfFilePath, currentPage, 4, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }), "АКТвходногоконтроля") < 10)
                    //else if (recognize(inputPdf, currentPage, 3, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }) == "АКТвходногоконтроля")
                    {
                        currentDoc++;
                        DocumentClass = "Акт входного контроля";
                        DocumentClassCode = "АВК";

                        firstSubdocumentPage = currentPage;
                        lastSubdocumentPage = currentPage;

                        //AOSRtitle = ""; //АВК 
                        //Priltitle = recognize(inputPdf, currentPage, 3, 2, 190).Trim(new char[] { ' ' });
                        DocName = "Акт входного контроля";//recognize(inputPdf, currentPage, 3, 2, 190).Trim(new char[] { ' ' });
                    }
                    else if (recognize(inputPdfFilePath, currentPage, 6, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }).IndexOf("сполн") >= 0)
                    {
                        currentDoc++;
                        DocumentClass = "Исполнительная схема";
                        DocumentClassCode = "ИС";

                        firstSubdocumentPage = currentPage;
                        lastSubdocumentPage = currentPage;

                        //AOSRtitle = ""; //ИС
                        //Priltitle = recognize(inputPdf, currentPage, 4, 2, 190).Trim(new char[] { ' ' });
                        DocName = recognize(inputPdfFilePath, currentPage, 6, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });
                        if (DocName.IndexOf("сполн") > 0)
                        {
                            DocName = DocName.Substring(DocName.IndexOf("сполн") - 1, DocName.Length - DocName.IndexOf("сполн") - 1);
                        }
                        else if (DocName.IndexOf("сполн") == 0)
                        {
                            DocName = DocName.Substring(DocName.IndexOf("сполн"), DocName.Length - DocName.IndexOf("сполн"));
                        }
                        else
                        {
                            currentDoc++;

                            firstSubdocumentPage = currentPage;
                            lastSubdocumentPage = currentPage;
                        }

                    }
                    else
                    {
                        string OCRstr = "";
                        OCRstr = recognize(inputPdfFilePath, currentPage, 7, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' });
                        int indexOfSubstr;
                        if (OCRstr.IndexOf("Д") == -1) indexOfSubstr = OCRstr.IndexOf("д");
                        else indexOfSubstr = OCRstr.IndexOf("Д");

                        if (indexOfSubstr != -1 & OCRstr.Length > 54 + indexOfSubstr)
                        {
                            OCRstr = OCRstr.Substring(indexOfSubstr, 54);
                            if (LevenshteinDistance(OCRstr, "ДОКУМЕНТОКАЧЕСТВЕБЕТОННОЙСМЕСИЗАДАННОГОКАЧЕСТВАПАРТИИ") < 40)
                            {
                                currentDoc++;
                                DocumentClass = "Документ о качестве";
                                DocumentClassCode = "ПАСП";

                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;

                                DocName = recognize(inputPdfFilePath, currentPage, 7, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });

                                if (DocName.IndexOf("ДОК") >= 0)
                                {
                                    DocName = DocName.Substring(DocName.IndexOf("ДОК"), DocName.Length - DocName.IndexOf("ДОК"));
                                }
                                else
                                {
                                    DocName = recognize(inputPdfFilePath, currentPage, 7, 2, 130).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });
                                    if (DocName.IndexOf("ДОК") >= 0)
                                    {
                                        DocName = DocName.Substring(DocName.IndexOf("ДОК"), DocName.Length - DocName.IndexOf("ДОК"));
                                    }
                                    else
                                    {
                                        currentDoc++;
                                        //DocumentClass = "Прочее";
                                        //DocumentClassCode = "ПРОЧ";
                                        firstSubdocumentPage = currentPage;
                                        lastSubdocumentPage = currentPage;
                                    }
                                }

                            }
                            else
                            {
                                currentDoc++;
                                //DocumentClass = "Прочее";
                                //DocumentClassCode = "ПРОЧ";

                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;
                            }
                        }
                        else
                        {
                            currentDoc++;
                            //DocumentClass = "Прочее";
                            //DocumentClassCode = "ПРОЧ";

                            firstSubdocumentPage = currentPage;
                            lastSubdocumentPage = currentPage;
                        }
                    }

                    if (firstSubdocumentPage == lastSubdocumentPage) altFileNeme = pdfFileInfo.Name.Substring(0, pdfFileInfo.Name.IndexOf("pdf") - 1) + " (лист " + firstSubdocumentPage.ToString() + ").pdf";
                    else altFileNeme = pdfFileInfo.Name.Substring(0, pdfFileInfo.Name.IndexOf("pdf") - 1) + " (листы " + firstSubdocumentPage.ToString() + " - " + lastSubdocumentPage.ToString() + ").pdf";

                    //int DocumentTom = 11;
                    //string DocumentTomFmt = "00";
                    //string Partition = "КЖ";
                    //int DocumentBook = 1;
                    //string DocumentBookFmt = "00";
                    string currentDocFmt = "000";

                    //DocCode = "ВММК-ИД-" + DocumentTom.ToString(DocumentTomFmt) + "-" + Partition + "-" + DocumentBook.ToString(DocumentBookFmt) + "-" + DocumentClass + "-" + currentDoc.ToString(currentDocFmt);
                    DocCode = DocumentSetCode + "-" + DocumentClassCode + "-" + currentDoc.ToString(currentDocFmt);

                    FileName = DocCode + "-" + DocName;
                    if (FileName.Length > 256) FileName = FileName.Substring(0, 256);
                    FileName = FileName + FileType;
                }
                else
                {
                    FileName = "сраница принадлежит документу";

                    //DocCode = levReslt;
                }

                
                VMMC_Core.Document document = new VMMC_Core.Document(sessionInfo).GetDocument(DocCode);
                if (document == null) 
                {
                    document = new VMMC_Core.Document(sessionInfo) { DocumentId = Guid.NewGuid(), DocumentCode = DocCode, DocumentName = DocName, DocumentClassId = new VMMC_Core.Class(sessionInfo).getClass(DocumentClass).ClassId };
                }
                if (document != null)
                {
                    if (document.Revisions == null) document.Revisions = new ObservableCollection<Revision>();
                    VMMC_Core.Revision revision = document.Revisions.Where(x => x.Number == 0).FirstOrDefault();
                    if (revision == null) document.Revisions.Add(new VMMC_Core.Revision(sessionInfo) { RevisionId = Guid.NewGuid(), DocumentId = document.DocumentId, Number = 0 });
                    revision = document.Revisions.Where(x => x.Number == 0).FirstOrDefault();

                    if (revision != null)
                    {
                        if (revision.Files == null) revision.Files = new ObservableCollection<Files>();
                        VMMC_Core.Files file = revision.Files.Where(x => x.FileName == FileName).FirstOrDefault();
                        if (file == null)
                        {
                            file = new VMMC_Core.Files(sessionInfo) { FileName = FileName, FileType = FileType, RevisionId = revision.RevisionId };
                            file.Checksum = file.ComputeMD5Checksum(file.LocalPath);
                            revision.Files.Add(file);
                        }
                        file = revision.Files.Where(x => x.FileName == FileName).FirstOrDefault();
                    }
                    document.StatusInfo = firstSubdocumentPage.ToString() + " - " + lastSubdocumentPage.ToString();
                    docList.Add(document);
                }
            }

            pdfParserViewModel.DocumentsCollection = docList;
            return pdfParserViewModel;
        }
        private void AsyncParsing(object sender, DoWorkEventArgs e)    // This event handler is where the time-consuming work is done.
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
                //break;
            }
            else
            {
                //selectSaveFolder();
                //string output = selectFolder();
                string outputSplit = string.Empty;
                int DPI = (int)(DPIvalue);

                //string[] prevDocInfo;
                FileInfo pdfFileInfo = new FileInfo(inputPdf);
                int currentDoc = 0;
                //string[] DocInfo;
                //string[] prevDocInfo;

                int firstSubdocumentPage = 0;
                int lastSubdocumentPage = 0;

                string levReslt = "";
                int pageCounnt = PDF_PageCount(inputPdf);
                string FileName = "";
                //pageCounnt = 10;

                //FilesDataTable
                DataTable FilesDataTable = new DataTable();

                //FilesDataTable.Columns.Add("altFileNeme");
                FilesDataTable.Columns.Add("DocumentCode");
                FilesDataTable.Columns.Add("DocumentName");
                FilesDataTable.Columns.Add("Revision");
                FilesDataTable.Columns.Add("DocumentClass");
                FilesDataTable.Columns.Add("DocumentSetCode");
                FilesDataTable.Columns.Add("DocumentSetName");
                FilesDataTable.Columns.Add("FirstPage");
                FilesDataTable.Columns.Add("LastPage");
                FilesDataTable.Columns.Add("FileName");
                FilesDataTable.Columns.Add("FileType");
                FilesDataTable.Columns.Add("FilePath");
                FilesDataTable.Columns.Add("HashSumm");
                FilesDataTable.Columns.Add("Atr_Number"); //Add("Номер");
                FilesDataTable.Columns.Add("Atr_Date"); //Add("Дата");
                FilesDataTable.Columns.Add("Atr_Organization"); //Add("Организация");
                FilesDataTable.Columns.Add("Atr_Manufacturer"); //Add("Изготовитель");
                FilesDataTable.Columns.Add("Atr_Supplier"); //Add("Поставщик");
                FilesDataTable.Columns.Add("Atr_Control"); //Add("Поставщик");
                FilesDataTable.Columns.Add("Atr_SMR"); //Add("Поставщик");
                FilesDataTable.Columns.Add("Atr_WorkType"); //Add("Поставщик");
                FilesDataTable.Columns.Add("Atr_Material"); //Add("Материал");
                FilesDataTable.Columns.Add("Atr_Validity"); //Add("Срок действия");
                FilesDataTable.Columns.Add("Rel_AOSR");
                FilesDataTable.Columns.Add("Rel_AVK");
                FilesDataTable.Columns.Add("Rel_IS");


                FilesDataTable.Columns["Revision"].DataType = System.Type.GetType("System.Int32");
                FilesDataTable.Columns["FirstPage"].DataType = System.Type.GetType("System.Int32");
                FilesDataTable.Columns["LastPage"].DataType = System.Type.GetType("System.Int32");

                //DataRow row = FilesDataTable.NewRow();



                for (int currentPage = 1; currentPage <= pageCounnt; currentPage++)
                //for (int currentPage = 70; currentPage < 80; currentPage++)
                {
                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        //break;
                    }
                    else
                    {
                        int countRows = pageCounnt;
                        int CurrentRow = currentPage;

                        worker.ReportProgress(currentPage * 100 / pageCounnt);



                        string altFileNeme = "";
                        string FileType = ".pdf";
                        string DocCode = "";
                        string DocName = "";
                        int DocRevision = 0;//все ИД помечаются 0 ревизией
                        string DocumentClass = "";
                        string DocumentClassCode = "";

                        if (currentPage == 1 || currentPage > lastSubdocumentPage)//int.Parse(prevDocInfo[6]))
                        {

                            if (LevenshteinDistance(recognize(inputPdf, currentPage, 1, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }), "АКТосвидетельствованияскрытыхработ") < 10)
                            //if (recognize(inputPdf, currentPage, 1, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }) == "АКТосвидетельствованияскрытыхработ")

                            {
                                currentDoc++;

                                DocumentClass = "Акт освидетельствования скрытых работ";
                                DocumentClassCode = "АОСР";
                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;
                                DocName = recognize(inputPdf, currentPage, 3, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "'").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });


                                for (int lastPage = currentPage + 1; lastPage < pageCounnt; lastPage++)
                                {
                                    if (worker.CancellationPending == true)
                                    {
                                        e.Cancel = true;
                                        //break;
                                    }
                                    else
                                    {
                                        //DocName = recognize(inputPdf, lastPage, 5, 2, 100);
                                        string str = recognize(inputPdf, lastPage, 3, 2, 190).Replace("\n", " ").Trim(new char[] { ' ' });


                                        if (str != "")
                                        {
                                            if (DocName.Length < str.Length) str = str.Substring(0, DocName.Length);
                                            levReslt = "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString();

                                            if (LevenshteinDistance(DocName, str) < DocName.Length * 0.2)
                                            {
                                                lastSubdocumentPage = lastPage;
                                                //MessageBox.Show(DocName + ": " + DocName.Length.ToString() + "\n" + str + ": " + str.Length.ToString() + "\n" + "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                //currentPage = lastPage-1;   
                                            }
                                            else
                                            {
                                                str = recognize(inputPdf, lastPage, 3, 2, 230).Replace("\n", " ").Trim(new char[] { ' ' });
                                                if (DocName.Length < str.Length) str = str.Substring(0, DocName.Length);
                                                levReslt = "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString();
                                                if (LevenshteinDistance(DocName, str) < DocName.Length * 0.5)
                                                {
                                                    lastSubdocumentPage = lastPage;
                                                    //MessageBox.Show(DocName + ": " + DocName.Length.ToString() + "\n" + str + ": " + str.Length.ToString() + "\n" + "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                    //currentPage = lastPage-1;   
                                                }
                                                else break;

                                            }
                                        }
                                        else break;
                                    }
                                }
                            }
                            else if (LevenshteinDistance(recognize(inputPdf, currentPage, 4, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }), "АКТвходногоконтроля") < 10)
                            //else if (recognize(inputPdf, currentPage, 3, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }) == "АКТвходногоконтроля")
                            {
                                currentDoc++;
                                DocumentClass = "Акт входного контроля";
                                DocumentClassCode = "АВК";

                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;

                                //AOSRtitle = ""; //АВК 
                                //Priltitle = recognize(inputPdf, currentPage, 3, 2, 190).Trim(new char[] { ' ' });
                                DocName = "Акт входного контроля";//recognize(inputPdf, currentPage, 3, 2, 190).Trim(new char[] { ' ' });
                            }
                            else if (recognize(inputPdf, currentPage, 6, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }).IndexOf("сполн") >= 0)
                            {
                                currentDoc++;
                                DocumentClass = "Исполнительная схема";
                                DocumentClassCode = "ИС";

                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;

                                //AOSRtitle = ""; //ИС
                                //Priltitle = recognize(inputPdf, currentPage, 4, 2, 190).Trim(new char[] { ' ' });
                                DocName = recognize(inputPdf, currentPage, 6, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });
                                if (DocName.IndexOf("сполн") > 0)
                                {
                                    DocName = DocName.Substring(DocName.IndexOf("сполн") - 1, DocName.Length - DocName.IndexOf("сполн") - 1);
                                }
                                else if (DocName.IndexOf("сполн") == 0)
                                {
                                    DocName = DocName.Substring(DocName.IndexOf("сполн"), DocName.Length - DocName.IndexOf("сполн"));
                                }
                                else
                                {
                                    currentDoc++;

                                    firstSubdocumentPage = currentPage;
                                    lastSubdocumentPage = currentPage;
                                }

                            }
                            else
                            {
                                string OCRstr = "";
                                OCRstr = recognize(inputPdf, currentPage, 7, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' });
                                int indexOfSubstr;
                                if (OCRstr.IndexOf("Д") == -1) indexOfSubstr = OCRstr.IndexOf("д");
                                else indexOfSubstr = OCRstr.IndexOf("Д");

                                if (indexOfSubstr != -1 & OCRstr.Length > 54 + indexOfSubstr)
                                {
                                    OCRstr = OCRstr.Substring(indexOfSubstr, 54);
                                    if (LevenshteinDistance(OCRstr, "ДОКУМЕНТОКАЧЕСТВЕБЕТОННОЙСМЕСИЗАДАННОГОКАЧЕСТВАПАРТИИ") < 40)
                                    {
                                        currentDoc++;
                                        DocumentClass = "Документ о качестве";
                                        DocumentClassCode = "ПАСП";

                                        firstSubdocumentPage = currentPage;
                                        lastSubdocumentPage = currentPage;

                                        DocName = recognize(inputPdf, currentPage, 7, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });

                                        if (DocName.IndexOf("ДОК") >= 0)
                                        {
                                            DocName = DocName.Substring(DocName.IndexOf("ДОК"), DocName.Length - DocName.IndexOf("ДОК"));
                                        }
                                        else
                                        {
                                            DocName = recognize(inputPdf, currentPage, 7, 2, 130).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });
                                            if (DocName.IndexOf("ДОК") >= 0)
                                            {
                                                DocName = DocName.Substring(DocName.IndexOf("ДОК"), DocName.Length - DocName.IndexOf("ДОК"));
                                            }
                                            else
                                            {
                                                currentDoc++;
                                                //DocumentClass = "Прочее";
                                                //DocumentClassCode = "ПРОЧ";
                                                firstSubdocumentPage = currentPage;
                                                lastSubdocumentPage = currentPage;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        currentDoc++;
                                        //DocumentClass = "Прочее";
                                        //DocumentClassCode = "ПРОЧ";

                                        firstSubdocumentPage = currentPage;
                                        lastSubdocumentPage = currentPage;
                                    }
                                }
                                else
                                {
                                    currentDoc++;
                                    //DocumentClass = "Прочее";
                                    //DocumentClassCode = "ПРОЧ";

                                    firstSubdocumentPage = currentPage;
                                    lastSubdocumentPage = currentPage;
                                }
                            }

                            if (firstSubdocumentPage == lastSubdocumentPage) altFileNeme = pdfFileInfo.Name.Substring(0, pdfFileInfo.Name.IndexOf("pdf") - 1) + " (лист " + firstSubdocumentPage.ToString() + ").pdf";
                            else altFileNeme = pdfFileInfo.Name.Substring(0, pdfFileInfo.Name.IndexOf("pdf") - 1) + " (листы " + firstSubdocumentPage.ToString() + " - " + lastSubdocumentPage.ToString() + ").pdf";

                            //int DocumentTom = 11;
                            //string DocumentTomFmt = "00";
                            //string Partition = "КЖ";
                            //int DocumentBook = 1;
                            //string DocumentBookFmt = "00";
                            string currentDocFmt = "000";

                            //DocCode = "ВММК-ИД-" + DocumentTom.ToString(DocumentTomFmt) + "-" + Partition + "-" + DocumentBook.ToString(DocumentBookFmt) + "-" + DocumentClass + "-" + currentDoc.ToString(currentDocFmt);
                            DocCode = DocumentSetCode + "-" + DocumentClassCode + "-" + currentDoc.ToString(currentDocFmt);

                            FileName = DocCode + "-" + DocName;
                            if (FileName.Length > 256) FileName = FileName.Substring(0, 256);
                            FileName = FileName + FileType;
                        }
                        else
                        {
                            FileName = "сраница принадлежит документу";

                            //DocCode = levReslt;
                        }

                        DataRow row = FilesDataTable.NewRow();

                        row["FileName"] = FileName;
                        row["FileType"] = FileType;
                        //row["altFileNeme"] = altFileNeme;
                        row["DocumentCode"] = DocCode;
                        row["DocumentName"] = DocName;
                        row["Revision"] = DocRevision;
                        row["DocumentClass"] = DocumentClass;
                        row["DocumentSetCode"] = DocumentSetCode;
                        row["FirstPage"] = firstSubdocumentPage;
                        row["LastPage"] = lastSubdocumentPage;


                        FilesDataTable.Rows.Add(row);
                        currentPage = lastSubdocumentPage;
                    }
                }

                DataTable ResultDT = new DataTable();
                DataSet ResultDS = new DataSet();

                ResultDT = FilesDataTable;
                //ResultDS = new DataSet();
                //ResultDS.Tables.Add(ResultDT);
                ParsingPdfDS = new DataSet();
                ParsingPdfDS.Tables.Add(ResultDT);
                //ListOfFilesDS.Tables[0] = FilesTable;
                //System.Threading.Thread.Sleep(5000); //для отладки работы progressBar            
            }
        }
        private DataSet Parcing1() // This event handler is where the time-consuming work is done.
        {
            string outputSplit = string.Empty;
            int DPI = (int)(DPIvalue);

            //string[] prevDocInfo;
            FileInfo pdfFileInfo = new FileInfo(inputPdf);
            int currentDoc = 0;
            //string[] DocInfo;
            //string[] prevDocInfo;

            int firstSubdocumentPage = 0;
            int lastSubdocumentPage = 0;

            string levReslt = "";
            int pageCounnt = PDF_PageCount(inputPdf);
            string FileName = "";
            //pageCounnt = 10;

            //FilesDataTable
            DataTable FilesDataTable = new DataTable();

            //FilesDataTable.Columns.Add("altFileNeme");
            FilesDataTable.Columns.Add("DocumentCode");
            FilesDataTable.Columns.Add("DocumentName");
            FilesDataTable.Columns.Add("Revision");
            FilesDataTable.Columns.Add("DocumentClass");
            FilesDataTable.Columns.Add("DocumentSetCode");
            FilesDataTable.Columns.Add("DocumentSetName");
            FilesDataTable.Columns.Add("FirstPage");
            FilesDataTable.Columns.Add("LastPage");
            FilesDataTable.Columns.Add("FileName");
            FilesDataTable.Columns.Add("FileType");
            FilesDataTable.Columns.Add("FilePath");
            FilesDataTable.Columns.Add("HashSumm");
            FilesDataTable.Columns.Add("Atr_Number"); //Add("Номер");
            FilesDataTable.Columns.Add("Atr_Date"); //Add("Дата");
            FilesDataTable.Columns.Add("Atr_Organization"); //Add("Организация");
            FilesDataTable.Columns.Add("Atr_Manufacturer"); //Add("Изготовитель");
            FilesDataTable.Columns.Add("Atr_Supplier"); //Add("Поставщик");
            FilesDataTable.Columns.Add("Atr_Control"); //Add("Поставщик");
            FilesDataTable.Columns.Add("Atr_SMR"); //Add("Поставщик");
            FilesDataTable.Columns.Add("Atr_WorkType"); //Add("Поставщик");
            FilesDataTable.Columns.Add("Atr_Material"); //Add("Материал");
            FilesDataTable.Columns.Add("Atr_Validity"); //Add("Срок действия");
            FilesDataTable.Columns.Add("Rel_AOSR");
            FilesDataTable.Columns.Add("Rel_AVK");
            FilesDataTable.Columns.Add("Rel_IS");


            FilesDataTable.Columns["Revision"].DataType = System.Type.GetType("System.Int32");
            FilesDataTable.Columns["FirstPage"].DataType = System.Type.GetType("System.Int32");
            FilesDataTable.Columns["LastPage"].DataType = System.Type.GetType("System.Int32");

            //DataRow row = FilesDataTable.NewRow();



            for (int currentPage = 1; currentPage <= pageCounnt; currentPage++)
            //for (int currentPage = 70; currentPage < 80; currentPage++)
            {


                int countRows = pageCounnt;
                int CurrentRow = currentPage;

                string altFileNeme = "";
                string FileType = ".pdf";
                string DocCode = "";
                string DocName = "";
                int DocRevision = 0;//все ИД помечаются 0 ревизией
                string DocumentClass = "";
                string DocumentClassCode = "";

                if (currentPage == 1 || currentPage > lastSubdocumentPage)//int.Parse(prevDocInfo[6]))
                {

                    if (LevenshteinDistance(recognize(inputPdf, currentPage, 1, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }), "АКТосвидетельствованияскрытыхработ") < 10)
                    //if (recognize(inputPdf, currentPage, 1, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }) == "АКТосвидетельствованияскрытыхработ")

                    {
                        currentDoc++;

                        DocumentClass = "Акт освидетельствования скрытых работ";
                        DocumentClassCode = "АОСР";
                        firstSubdocumentPage = currentPage;
                        lastSubdocumentPage = currentPage;
                        DocName = recognize(inputPdf, currentPage, 3, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "'").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });


                        for (int lastPage = currentPage + 1; lastPage < pageCounnt; lastPage++)
                        {


                            //DocName = recognize(inputPdf, lastPage, 5, 2, 100);
                            string str = recognize(inputPdf, lastPage, 3, 2, 190).Replace("\n", " ").Trim(new char[] { ' ' });


                            if (str != "")
                            {
                                if (DocName.Length < str.Length) str = str.Substring(0, DocName.Length);
                                levReslt = "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString();

                                if (LevenshteinDistance(DocName, str) < DocName.Length * 0.2)
                                {
                                    lastSubdocumentPage = lastPage;
                                    //MessageBox.Show(DocName + ": " + DocName.Length.ToString() + "\n" + str + ": " + str.Length.ToString() + "\n" + "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //currentPage = lastPage-1;   
                                }
                                else
                                {
                                    str = recognize(inputPdf, lastPage, 3, 2, 230).Replace("\n", " ").Trim(new char[] { ' ' });
                                    if (DocName.Length < str.Length) str = str.Substring(0, DocName.Length);
                                    levReslt = "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString();
                                    if (LevenshteinDistance(DocName, str) < DocName.Length * 0.5)
                                    {
                                        lastSubdocumentPage = lastPage;
                                        //MessageBox.Show(DocName + ": " + DocName.Length.ToString() + "\n" + str + ": " + str.Length.ToString() + "\n" + "Расстояние Левенштейна: " + LevenshteinDistance(DocName, str).ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //currentPage = lastPage-1;   
                                    }
                                    else break;

                                }
                            }
                            else break;

                        }
                    }
                    else if (LevenshteinDistance(recognize(inputPdf, currentPage, 4, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }), "АКТвходногоконтроля") < 10)
                    //else if (recognize(inputPdf, currentPage, 3, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }) == "АКТвходногоконтроля")
                    {
                        currentDoc++;
                        DocumentClass = "Акт входного контроля";
                        DocumentClassCode = "АВК";

                        firstSubdocumentPage = currentPage;
                        lastSubdocumentPage = currentPage;

                        //AOSRtitle = ""; //АВК 
                        //Priltitle = recognize(inputPdf, currentPage, 3, 2, 190).Trim(new char[] { ' ' });
                        DocName = "Акт входного контроля";//recognize(inputPdf, currentPage, 3, 2, 190).Trim(new char[] { ' ' });
                    }
                    else if (recognize(inputPdf, currentPage, 6, 2, 190).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' }).IndexOf("сполн") >= 0)
                    {
                        currentDoc++;
                        DocumentClass = "Исполнительная схема";
                        DocumentClassCode = "ИС";

                        firstSubdocumentPage = currentPage;
                        lastSubdocumentPage = currentPage;

                        //AOSRtitle = ""; //ИС
                        //Priltitle = recognize(inputPdf, currentPage, 4, 2, 190).Trim(new char[] { ' ' });
                        DocName = recognize(inputPdf, currentPage, 6, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });
                        if (DocName.IndexOf("сполн") > 0)
                        {
                            DocName = DocName.Substring(DocName.IndexOf("сполн") - 1, DocName.Length - DocName.IndexOf("сполн") - 1);
                        }
                        else if (DocName.IndexOf("сполн") == 0)
                        {
                            DocName = DocName.Substring(DocName.IndexOf("сполн"), DocName.Length - DocName.IndexOf("сполн"));
                        }
                        else
                        {
                            currentDoc++;

                            firstSubdocumentPage = currentPage;
                            lastSubdocumentPage = currentPage;
                        }

                    }
                    else
                    {
                        string OCRstr = "";
                        OCRstr = recognize(inputPdf, currentPage, 7, 2, 130).Replace("\n", " ").Replace(" ", "").Trim(new char[] { ' ' });
                        int indexOfSubstr;
                        if (OCRstr.IndexOf("Д") == -1) indexOfSubstr = OCRstr.IndexOf("д");
                        else indexOfSubstr = OCRstr.IndexOf("Д");

                        if (indexOfSubstr != -1 & OCRstr.Length > 54 + indexOfSubstr)
                        {
                            OCRstr = OCRstr.Substring(indexOfSubstr, 54);
                            if (LevenshteinDistance(OCRstr, "ДОКУМЕНТОКАЧЕСТВЕБЕТОННОЙСМЕСИЗАДАННОГОКАЧЕСТВАПАРТИИ") < 40)
                            {
                                currentDoc++;
                                DocumentClass = "Документ о качестве";
                                DocumentClassCode = "ПАСП";

                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;

                                DocName = recognize(inputPdf, currentPage, 7, 2, 190).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });

                                if (DocName.IndexOf("ДОК") >= 0)
                                {
                                    DocName = DocName.Substring(DocName.IndexOf("ДОК"), DocName.Length - DocName.IndexOf("ДОК"));
                                }
                                else
                                {
                                    DocName = recognize(inputPdf, currentPage, 7, 2, 130).Replace("\n", " ").Replace("/", "_").Replace("\\", "_").Replace("\"", "_").Replace(":", "").Replace("?", "").Replace("|", "").Trim(new char[] { ' ' });
                                    if (DocName.IndexOf("ДОК") >= 0)
                                    {
                                        DocName = DocName.Substring(DocName.IndexOf("ДОК"), DocName.Length - DocName.IndexOf("ДОК"));
                                    }
                                    else
                                    {
                                        currentDoc++;
                                        //DocumentClass = "Прочее";
                                        //DocumentClassCode = "ПРОЧ";
                                        firstSubdocumentPage = currentPage;
                                        lastSubdocumentPage = currentPage;
                                    }
                                }

                            }
                            else
                            {
                                currentDoc++;
                                //DocumentClass = "Прочее";
                                //DocumentClassCode = "ПРОЧ";

                                firstSubdocumentPage = currentPage;
                                lastSubdocumentPage = currentPage;
                            }
                        }
                        else
                        {
                            currentDoc++;
                            //DocumentClass = "Прочее";
                            //DocumentClassCode = "ПРОЧ";

                            firstSubdocumentPage = currentPage;
                            lastSubdocumentPage = currentPage;
                        }
                    }

                    if (firstSubdocumentPage == lastSubdocumentPage) altFileNeme = pdfFileInfo.Name.Substring(0, pdfFileInfo.Name.IndexOf("pdf") - 1) + " (лист " + firstSubdocumentPage.ToString() + ").pdf";
                    else altFileNeme = pdfFileInfo.Name.Substring(0, pdfFileInfo.Name.IndexOf("pdf") - 1) + " (листы " + firstSubdocumentPage.ToString() + " - " + lastSubdocumentPage.ToString() + ").pdf";

                    //int DocumentTom = 11;
                    //string DocumentTomFmt = "00";
                    //string Partition = "КЖ";
                    //int DocumentBook = 1;
                    //string DocumentBookFmt = "00";
                    string currentDocFmt = "000";

                    //DocCode = "ВММК-ИД-" + DocumentTom.ToString(DocumentTomFmt) + "-" + Partition + "-" + DocumentBook.ToString(DocumentBookFmt) + "-" + DocumentClass + "-" + currentDoc.ToString(currentDocFmt);
                    DocCode = DocumentSetCode + "-" + DocumentClassCode + "-" + currentDoc.ToString(currentDocFmt);

                    FileName = DocCode + "-" + DocName;
                    if (FileName.Length > 256) FileName = FileName.Substring(0, 256);
                    FileName = FileName + FileType;
                }
                else
                {
                    FileName = "сраница принадлежит документу";

                    //DocCode = levReslt;
                }

                DataRow row = FilesDataTable.NewRow();

                row["FileName"] = FileName;
                row["FileType"] = FileType;
                //row["altFileNeme"] = altFileNeme;
                row["DocumentCode"] = DocCode;
                row["DocumentName"] = DocName;
                row["Revision"] = DocRevision;
                row["DocumentClass"] = DocumentClass;
                row["DocumentSetCode"] = DocumentSetCode;
                row["FirstPage"] = firstSubdocumentPage;
                row["LastPage"] = lastSubdocumentPage;


                FilesDataTable.Rows.Add(row);
                currentPage = lastSubdocumentPage;

            }

            DataTable ResultDT = new DataTable();
            DataSet ResultDS = new DataSet();

            ResultDT = FilesDataTable;
            //ResultDS = new DataSet();
            //ResultDS.Tables.Add(ResultDT);
            DataSet resultDS = new DataSet();
            resultDS.Tables.Add(ResultDT);

            //ListOfFilesDS.Tables[0] = FilesTable;
            //System.Threading.Thread.Sleep(5000); //для отладки работы progressBar            
            return resultDS;
        }
        public string[] recognizeText(Bitmap Сropped_Bitmap, int LangIndex)
        {
            var documentText = new StringBuilder();
            string[] recognizedResult;
            string MeanConfidence;
            string lang1 = "eng+rus";
            string lang2 = "eng";
            string lang3 = "rus";
            string lang4 = "eng+rus";//number
            string[] lang = { lang1, lang2, lang3, lang4 };
            string tessdataDir = @"./tessdata";
            try
            {
                //MessageBox.Show(tessdataDir.ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(lang[Recognize_lang_ComboBox.SelectedIndex].ToString(), "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //using (var engine = new TesseractEngine(@"tessdata", "eng", EngineMode.Default))
                //using (var engine = new TesseractEngine(@"C:\test\vmmc\tessdata", lang[Recognize_lang_ComboBox.SelectedIndex], EngineMode.Default))
                //using (var engine = new TesseractEngine(tessdataDir, , EngineMode.Default))

                using (var engine = new TesseractEngine(tessdataDir, lang[LangIndex], EngineMode.Default))
                //using (var engine = new TesseractEngine(tessdataDir, lang[Recognize_lang_ComboBox.SelectedIndex]))
                {
                    if (LangIndex == 4) engine.SetVariable("tessedit_char_whitelist", "1234567890");
                    using (Page recognizedPage = engine.Process(Сropped_Bitmap))
                    {
                        string recognizedText = recognizedPage.GetText();
                        documentText.Append(recognizedText);
                        MeanConfidence = recognizedPage.GetMeanConfidence().ToString();
                    }
                    recognizedResult = new String[] { documentText.ToString(), MeanConfidence };
                    return recognizedResult;

                }
            }

            catch (Exception expt)
            {
                //MessageBox.Show(expt.Message, "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(expt.StackTrace, "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }

        }
        public int PDF_PageCount(string PathToPdf)
        {
            //читаем документ, проверяем кол-во страниц

            var document = PdfiumViewer.PdfDocument.Load(PathToPdf);
            int PageCount = document.PageCount;

            //MessageBox.Show("There are " + PageCount + " pages in the original file.", "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return PageCount;
        }
        public string recognize(string inputPdf, int CurentPage, int RectType, int LangIndex, int DPI)
        {
            string result = "";
            try
            {
                //Bitmap croppedBitmap;
                Bitmap originalBitmap = new Bitmap(ConverPdfToImg_shrt(inputPdf, CurentPage, DPI));


                string[] res = recognizeText(ImgCrop(originalBitmap, RecognizeAreaRectangle(RectType, originalBitmap.Width, originalBitmap.Height)), LangIndex);

                if (res.Length > 0) result = res[0];

                return result;
            }
            catch (Exception expt)
            {
                //MessageBox.Show(expt.Message, "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(expt.StackTrace, "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return result;
            }
        }
        private void ExtractPages(string sourcePDFpath, string outputPDFpath, int startpage, int endpage)
        {
            iTextSharp.text.pdf.PdfReader reader = null;
            iTextSharp.text.Document sourceDocument = null;
            iTextSharp.text.pdf.PdfCopy pdfCopyProvider = null;
            iTextSharp.text.pdf.PdfImportedPage importedPage = null;
            try
            {
                reader = new iTextSharp.text.pdf.PdfReader(sourcePDFpath);
                sourceDocument = new iTextSharp.text.Document(reader.GetPageSizeWithRotation(startpage));
                pdfCopyProvider = new iTextSharp.text.pdf.PdfCopy(sourceDocument, new System.IO.FileStream(outputPDFpath, System.IO.FileMode.Create));

                sourceDocument.Open();

                for (int i = startpage; i <= endpage; i++)
                {
                    importedPage = pdfCopyProvider.GetImportedPage(reader, i);
                    pdfCopyProvider.AddPage(importedPage);
                }
                sourceDocument.Close();
                reader.Close();
            }
            catch { }
        }        
        public static Bitmap ConverPdfToImg_shrt(string input_Pdf, int Page, int DPI)
        {
            float DPI_X = (float)DPI * 50 / 72F;
            float DPI_Y = (float)DPI * 50 / 72F;

            Bitmap result_Bitmap;
            // Create PDF Document

            // Load PDF Document into WinForms Control
            try
            {
                using (var pdfDocument = PdfiumViewer.PdfDocument.Load(input_Pdf))
                {
                    SizeF sizeInPoints = pdfDocument.PageSizes[Page - 1];
                    int widthInPixels = (int)Math.Round(sizeInPoints.Width * (float)DPI_X / 72F);
                    int heightInPixels = (int)Math.Round(sizeInPoints.Height * (float)DPI_Y / 72F);
                    using (System.Drawing.Image image = pdfDocument.Render(Page - 1, widthInPixels, heightInPixels, DPI_X, DPI_Y, true))
                    {
                        //image.Save(output_Png, System.Drawing.Imaging.ImageFormat.Png);
                        result_Bitmap = new Bitmap(image);
                    }
                }
                //File.Delete(output_Png);
                return result_Bitmap;
            }

            catch (Exception expt)
            {
                //MessageBox.Show(expt.Message, "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(expt.StackTrace, "MyProgram", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return null;
            }
        }
        public System.Drawing.Rectangle RecognizeAreaRectangle(int AreaNumber, int ImgWidth, int ImgHeight)
        {
            System.Drawing.Rectangle cropRect;
            switch (AreaNumber)
            {
                case 0:// ничего не выбрано
                    cropRect = new System.Drawing.Rectangle(
                        1,
                        1,
                        1,
                        1
                        );
                    return cropRect;

                case 1: //титул АОСР
                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 3.403)),
                        ((int)(ImgHeight / 2.547)),
                        ((int)(ImgWidth / 2.404)),
                        ((int)(ImgHeight / 19.483))
                        );
                    return cropRect;

                case 2: //номер АОСР
                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 14.767)),
                        ((int)(ImgHeight / 2.292)),
                        ((int)(ImgWidth / 3.900)),
                        ((int)(ImgHeight / 19.483))
                        );
                    return cropRect;

                case 3://наименование АОСР 

                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 2.479545)),
                        ((int)(ImgHeight / 154.2)),
                        ((int)(ImgWidth / 1.897391)),
                        ((int)(ImgHeight / 22.02857))
                        );
                    return cropRect;

                case 4://титул АВК

                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 2.756667)),
                        ((int)(ImgHeight / 6.284946)),
                        ((int)(ImgWidth / 3.70852)),
                        ((int)(ImgHeight / 14.79747))
                        );
                    return cropRect;

                case 5://номер АВК
                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 28.69231)),
                        ((int)(ImgHeight / 4.906977)),
                        ((int)(ImgWidth / 5.144827)),
                        ((int)(ImgHeight / 17.58333))
                        );
                    return cropRect;

                case 6://ИС титул

                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 1.45273)),
                        ((int)(ImgHeight / 1.100642)),
                        ((int)(ImgWidth / 5.803192)),
                        ((int)(ImgHeight / 14.14679))
                        );
                    return cropRect;

                case 7: //документ о качестве бетонной смеси
                    cropRect = new System.Drawing.Rectangle(
                        ((int)(ImgWidth / 21.31429)),
                        ((int)(ImgHeight / 6.098266)),
                        ((int)(ImgWidth / 1.110119)),
                        ((int)(ImgHeight / 18.18966))
                        );
                    return cropRect;



                default:
                    throw new InvalidOperationException(string.Format("Unexpected area: [{0}].", AreaNumber));
            }
        }
        public Bitmap ImgCrop(Bitmap Input_Bitmap, System.Drawing.Rectangle RecognizeAreaRectangle)
        {
            Bitmap croppedBitmap;
            Bitmap originalBitmap;
            System.Drawing.Rectangle cropRect;


            try
            {
                //получаем битмап для распознавания
                using (var bmpTemp = new Bitmap(Input_Bitmap))
                {
                    originalBitmap = new Bitmap(bmpTemp);
                    croppedBitmap = new Bitmap(bmpTemp);
                    cropRect = RecognizeAreaRectangle;
                    croppedBitmap = croppedBitmap.Clone(cropRect, System.Drawing.Imaging.PixelFormat.DontCare);
                }

                return croppedBitmap;

            }
            catch (Exception expt)
            {
                //MessageBox.Show(expt.Message, "MyProgram - ImgCrop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //MessageBox.Show(expt.StackTrace, "MyProgram - ImgCrop", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return Input_Bitmap;
            }
        }
        public int LevenshteinDistance(string firstWord, string secondWord)
        {
            var n = firstWord.Length + 1;
            var m = secondWord.Length + 1;
            var matrixD = new int[n, m];

            const int deletionCost = 1;
            const int insertionCost = 1;

            for (var i = 0; i < n; i++)
            {
                matrixD[i, 0] = i;
            }

            for (var j = 0; j < m; j++)
            {
                matrixD[0, j] = j;
            }

            for (var i = 1; i < n; i++)
            {
                for (var j = 1; j < m; j++)
                {
                    var substitutionCost = firstWord[i - 1] == secondWord[j - 1] ? 0 : 1;

                    matrixD[i, j] = Minimum(matrixD[i - 1, j] + deletionCost,          // удаление
                                            matrixD[i, j - 1] + insertionCost,         // вставка
                                            matrixD[i - 1, j - 1] + substitutionCost); // замена
                }
            }

            return matrixD[n - 1, m - 1];
        }
        static int Minimum(int a, int b, int c) //=> (a = a < b ? a : b) < c ? a : c;
        {
            if (a < b)
            {
                if (a < c) return a;
                else return c;
            }
            else
            {
                if (b < c) return b;
                else return c;
            }
        }
        public void SplitDocument(string pathToSourcePdf, string pathToNewPdf, int firstPage, int lastPage)
        {
            //File.Copy(pathToSourcePdf, pathToNewPdf, false);
            try
            {
                using (var document = PdfiumViewer.PdfDocument.Load(pathToSourcePdf))
                {

                    for (int i = document.PageCount - 1; i >= 0; i--)
                    {
                        if (i < firstPage - 1 || i > lastPage - 1) document.DeletePage(i);
                    }
                    document.Save(pathToNewPdf);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
            }
        }
        private void Spliting()
        {
            
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {               

                dialog.ShowDialog();


                if (dialog.SelectedPath != null)
                {
                    Spliting(dialog.SelectedPath); 
                }
            }
            IsPDFsplited = true;
        }
        private void Spliting(string splitDirectioryPath)    // This event handler is where the time-consuming work is done.
        {
            //PdfParcerViewModel pdfParserViewModel = new PdfParcerViewModel(sessionInfo);
            //ObservableCollection<VMMC_Core.Document> docList = new ObservableCollection<VMMC_Core.Document>();
            //countRows = ParsingPdfDS.Tables[0].Rows.Count;
            //CurrentRow = 0;
            foreach (VMMC_Core.Document document in DocumentsCollection)
            {
                foreach (VMMC_Core.Revision revision in document.Revisions)
                {
                    foreach (VMMC_Core.Files file in revision.Files)
                    {

                        string firstPage = document.StatusInfo.Split('-')[0];
                        string lastPage = document.StatusInfo.Split('-')[1];

                        string fileName = document.DocumentCode + "-" + document.DocumentName;
                        if (fileName.Length > 256) fileName = fileName.Substring(0, 256);
                        fileName = fileName + ".pdf";

                        string splitFilePath = splitDirectioryPath + "\\" + fileName;

                        if (splitFilePath.Length > 254)
                        {
                            splitFilePath = splitDirectioryPath + "\\" + fileName.Substring(0, (254 - splitDirectioryPath.Length - 2)) + ".pdf";
                        }
                        ExtractPages(SelectSourcePdfViewModel.SourcePdfPath, splitFilePath, int.Parse(firstPage), int.Parse(lastPage));
                        file.LocalPath = splitFilePath;
                        file.Checksum = file.ComputeMD5Checksum(file.LocalPath);
                    }
                }

            }

            //for (int i = 0; i < ParsingPdfDS.Tables[0].Rows.Count; i++)
            //{
            //    DataRow row = ParsingPdfDS.Tables[0].Rows[i];

            //    CurrentRow++;


            //    string FileName = row["FileName"].ToString();
            //    string SplitPath = "";
            //    int count = FileName.Length + SplitDirectioryPath.Length + 2;
            //    if (count > 254)
            //    {
            //        SplitPath = SplitDirectioryPath + "\\" + FileName.Substring(0, (254 - SplitDirectioryPath.Length - 2)) + ".pdf";
            //        ExtractPages(inputPdf, SplitPath, int.Parse(row["FirstPage"].ToString()), int.Parse(row["LastPage"].ToString()));
            //    }
            //    else
            //    {
            //        SplitPath = SplitDirectioryPath + "\\" + FileName;
            //        ExtractPages(inputPdf, SplitPath, int.Parse(row["FirstPage"].ToString()), int.Parse(row["LastPage"].ToString()));
            //    }


            //    ParsingPdfDS.Tables[0].Rows[i]["FilePath"] = SplitPath;

            //    //var checksum = new CheckByFileNeme();
            //    ParsingPdfDS.Tables[0].Rows[i]["HashSumm"] = checksum.ComputeMD5Checksum(row["FilePath"].ToString());

            //}

            //pdfParserViewModel.DocumentsCollection = docList;
            //return pdfParserViewModel;
        }

        /**
        private void RefreshForm(object sender, EventArgs e)
        {
            bool PDFselected = true;
            if (PDFselected)
            {
                BitmapImage Original_Bitmap = new BitmapImage(ConverPdfToImg_shrt(inputPdf, (int)(Crnt_page.Value), (int)(DPI_value.Value)));
                //this.picture1.Size = new Size(Original_Bitmap.Width, Original_Bitmap.Height);
                //this.picture1.Size = new Size(400, 400);
                //this.picture1.SizeMode = PictureBoxSizeMode.Zoom;

                if (Recognize_area_ComboBox.SelectedIndex == 0) TextBox1.Text = "";
                else this.TextBox1.Text = recognize(inputPdf, (int)(Crnt_page.Value), Recognize_area_ComboBox.SelectedIndex, Recognize_lang_ComboBox.SelectedIndex, (int)(DPI_value.Value));

                //this.pdfRenderer.Page = (int)(Crnt_page.Value) - 1;
                //this.Crnt_page.Value = (int)(pdfRenderer.Page) + 1;
                DataGridView_ColumnsEdit();
                Refresh();
            }
        }
        private void RefreshFormOnNewPage(object sender, EventArgs e)
        {
            if (PDFselected)
            {
                if (ViewArea_ComboBox.SelectedItem.ToString() == "Картинка" || ViewArea_ComboBox.SelectedItem.ToString() == "New!") //if (ViewArea_ComboBox.SelectedIndex == 0)
                {
                    Crnt_page.Value = CurrentSelectedPageNumber;
                    Original_Bitmap = new Bitmap(ConverPdfToImg_shrt(inputPdf, (int)(CurrentSelectedPageNumber), (int)(DPI_value.Value)));
                    //Original_Bitmap = new Bitmap(ConverPdfToImg_shrt(inputPdf, (int)(Crnt_page.Value), (int)(DPI_value.Value)));
                    this.picture1.Size = new Size(Original_Bitmap.Width, Original_Bitmap.Height);

                    Refresh();
                }
                if (Recognize_area_ComboBox.SelectedIndex != 0) Recognize_area_ComboBox.SelectedIndex = 0;

                //this.TextBox1.Text = recognize(inputPdf, (int)(Crnt_page.Value), Recognize_area_ComboBox.SelectedIndex, Recognize_lang_ComboBox.SelectedIndex, (int)(DPI_value.Value));
                if (changePdfPageInViewer)
                {
                    Crnt_page.Value = CurrentSelectedPageNumber;
                    this.pdfRenderer.Page = CurrentSelectedPageNumber - 1;
                    //this.pdfRenderer.Page = (int)(Crnt_page.Value) - 1;
                }
                changePdfPageInViewer = true;
                //this.Crnt_page.Value = (int)(pdfRenderer.Page) + 1;
            }
        }
        private void InstanceRectangleIntersection1(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            if (PDFselected)
            {
                //Bitmap originalBitmap = new Bitmap(ConverPdfToImg_shrt(inputPdf, (int)(Crnt_page.Value), (int)(DPI_value.Value)));
                Bitmap originalBitmap = Original_Bitmap;
                System.Drawing.Rectangle cropRectangle = RecognizeAreaRectangle(Recognize_area_ComboBox.SelectedIndex, originalBitmap.Width, originalBitmap.Height);

                try
                {

                    //this.picture1.Size = new Size(originalBitmap.Width, originalBitmap.Height);
                    Graphics gBitmap = Graphics.FromImage(originalBitmap);
                    Point ulCorner = new Point(0, 0);
                    e.Graphics.DrawImage(originalBitmap, ulCorner);
                    //e.Graphics.DrawRectangle(Pens.Blue, customRectangle);
                    e.Graphics.DrawRectangle(Pens.Red, cropRectangle);
                }
                catch (OutOfMemoryException ex)
                {
                    MessageBox.Show("Ошибка чтения картинки");

                    MessageBox.Show(ex.Message.ToString());
                    return;
                }
            }
        }
        
        private void AsyncSpliting(object sender, DoWorkEventArgs e)    // This event handler is where the time-consuming work is done.
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
                //break;
            }
            else
            {
                countRows = ParsingPdfDS.Tables[0].Rows.Count;
                CurrentRow = 0;
                for (int i = 0; i < ParsingPdfDS.Tables[0].Rows.Count; i++)
                {
                    DataRow row = ParsingPdfDS.Tables[0].Rows[i];

                    if (worker.CancellationPending == true)
                    {
                        e.Cancel = true;
                        //break;
                    }
                    else
                    {
                        if (dataGridView1.Rows[i].Cells["DocumentCode"].Value.ToString() != "Исключен пользователем")
                        {
                            CurrentRow++;
                            worker.ReportProgress(CurrentRow * 100 / countRows);

                            //SplitDocument(inputPdf, SplitDirectioryPath + @"\\" + row["altFileNeme"].ToString(), int.Parse(row["FirstPage"].ToString()), int.Parse(row["LastPage"].ToString()));
                            string FileName = row["FileName"].ToString();
                            string SplitPath = "";
                            int count = FileName.Length + SplitDirectioryPath.Length + 2;
                            if (count > 254)
                            {
                                SplitPath = SplitDirectioryPath + "\\" + FileName.Substring(0, (254 - SplitDirectioryPath.Length - 2)) + ".pdf";
                                ExtractPages(inputPdf, SplitPath, int.Parse(row["FirstPage"].ToString()), int.Parse(row["LastPage"].ToString()));
                            }
                            else
                            {
                                SplitPath = SplitDirectioryPath + "\\" + FileName;
                                ExtractPages(inputPdf, SplitPath, int.Parse(row["FirstPage"].ToString()), int.Parse(row["LastPage"].ToString()));
                            }

                            //SplitDocument(inputPdf, SplitPath, int.Parse(row["FirstPage"].ToString()), int.Parse(row["LastPage"].ToString()));

                            ParsingPdfDS.Tables[0].Rows[i]["FilePath"] = SplitPath;

                            var checksum = new CheckByFileNeme();
                            ParsingPdfDS.Tables[0].Rows[i]["HashSumm"] = checksum.ComputeMD5Checksum(row["FilePath"].ToString());
                        }
                    }

                }
            }
        }
        
        /**/

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPdfParcerPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }
}
