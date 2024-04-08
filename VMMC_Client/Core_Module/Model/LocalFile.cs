using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace Core_Module
{
    public class LocalFile : INotifyPropertyChanged
    {
        //private string localFileName;
        //private string localFileType;
        private string localFilePath;
        private string checksum;

        private Core_Module.Relationship relationship;
        private Core_Module.Revision revision;
        private Core_Module.Document document;
        private Core_Module.Complekt complekt;

        private FileInfo file;

        public string status;
        public string statusInfo;

        public LocalFile(string path)
        {
            localFilePath = path;
            file = new FileInfo(localFilePath);
            //checksum = ComputeMD5Checksum(localFilePath);

        }
        public string Checksum
        {
            get { return checksum; }
            set
            {
                checksum = value;
                OnLocalFilePropertyChanged("Checksum");
            }
        }
        public string LocalFileName
        {
            get { return  file.Name; }
        }
        public string LocalFileType
        {
            get { return file.Extension; }
        }
        public string LocalFilePath
        {
            get { return localFilePath; }
            set
            {
                localFilePath = value;
                OnLocalFilePropertyChanged("LocalFilePath");
                OnLocalFilePropertyChanged("Checksum");
                OnLocalFilePropertyChanged("LocalFileName");
                OnLocalFilePropertyChanged("LocalFileType");
            }
        }
        public Core_Module.Revision Revision
        {
            get { return revision; }
            set
            {
                revision = value;
                OnLocalFilePropertyChanged("Revision");
            }
        }
        public Core_Module.Relationship Relationship
        {
            get { return relationship; }
            set
            {
                relationship = value;
                OnLocalFilePropertyChanged("Relationship");
            }
        }
        public Core_Module.Document Document
        {
            get { return document; }
            set
            {
                document = value;
                OnLocalFilePropertyChanged("Document");
            }
        } 
        public Core_Module.Complekt Complekt
        {
            get { return complekt; }
            set
            {
                complekt = value;
                OnLocalFilePropertyChanged("Complekt");
            }            
        }
        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                OnLocalFilePropertyChanged("Status");
            }
        }
        public string StatusInfo
        {
            get { return statusInfo; }
            set
            {
                statusInfo = value;
                OnLocalFilePropertyChanged("StatusInfo");
            }
        }


        public string[] FindCode(string nameFile)
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
        public string FindDocumentCode(string nameFile)
        {
            string pattern1 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)((_\s)|_|-|\s|\.)((И|и)зм(\.|-|_\s|_|\s)\d+)?";

            string pattern2 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)";

            string match1 = Regex.Match(nameFile, pattern1).ToString();
            match1 = match1.Replace("ВК.2.", "ВК-2-");
            match1 = match1.Replace("ВК.2", "ВК-2");

            string DocCode = Regex.Match(match1, pattern2).ToString();
            DocCode = DocCode.Trim(new char[] { '-', '_', '.', ' ' });           

            return DocCode;
        }
        public string FindDocumentName(string nameFile)
        {
            string pattern1 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)((_\s)|_|-|\s|\.)((И|и)зм(\.|-|_\s|_|\s)\d+)?";

            string match1 = Regex.Match(nameFile, pattern1).ToString();
            match1 = match1.Replace("ВК.2.", "ВК-2-");
            match1 = match1.Replace("ВК.2", "ВК-2");

            string DocName = Regex.Replace(nameFile, pattern1, "");
            DocName = DocName.Substring(0, DocName.LastIndexOf(".") + 1);
            DocName = DocName.Trim(new char[] { '-', '_', '.', ' ' });
            
            return DocName;
        }
        public int FindRevision(string nameFile)
        {

            string pattern1 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)((_\s)|_|-|\s|\.)((И|и)зм(\.|-|_\s|_|\s)\d+)?";
                        
            string pattern4 = @"((И|и)зм(\.|-|_\s|_|\s)\d+)";

            string match1 = Regex.Match(nameFile, pattern1).ToString();
            match1 = match1.Replace("ВК.2.", "ВК-2-");
            match1 = match1.Replace("ВК.2", "ВК-2");

            string DocRevision = Regex.Match(match1, pattern4).ToString();

            int rev = 0;

            if (DocRevision != "")
            {
                DocRevision = DocRevision.Substring(3);
                DocRevision = DocRevision.Trim(new char[] { '-', '_', '.', ' ' });
                //bool success = Int32.TryParse(revStr, out rev); //  TryParse(revision_str);
                rev = int.Parse(DocRevision);
            }

            return rev;
        }
        public string CheckDocumentInfo()
        {
            string statusInfo = "";
            if (Document.DocumentCode == "") statusInfo += "Не заполнено поле шифра документа. \n";
            if (Document.DocumentName == "") statusInfo += "Не заполнено поле наименования документа. \n";
            if (Revision.Number == null) statusInfo += "Не заполнено поле ревизии документа. \n";

            return statusInfo;
        }
        public string CheckComplektinfo()
        {
            string statusInfo = "";
            if (Complekt.ComplektCode == "") statusInfo += "Не заполнено поле шифра комплекта. \n";
            //if (targetFile.ComplektName == "") statusInfo += "Не заполнено поле наименования документа. \n";

            return statusInfo;
        }
        public string FindComplektCode(string nameFile)
        {

            string pattern1 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)((_\s)|_|-|\s|\.)((И|и)зм(\.|-|_\s|_|\s)\d+)?";

            string pattern2 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})-)|(ВК\.2\.)|(ВК\.2-))(\w{1}|\w{2}|(\w{1}-d{1}))-((\d*-\d*)|\d*.\d{1}|\d*)";

            string pattern3 = @"^ВММК-РД-(\d{2}|(\d{1}\.\d{2}))-(((\w*|(ТП3\.АСС,АУППТ,СОУЭ))-((\d{2}\.\d*)|(\d{2}\.\d*\.\d*)|\d{2}|\d{1})))";


            string match1 = Regex.Match(nameFile, pattern1).ToString();
            match1 = match1.Replace("ВК.2.", "ВК-2-");
            match1 = match1.Replace("ВК.2", "ВК-2");

            string DocCode = Regex.Match(match1, pattern2).ToString();
            DocCode = DocCode.Trim(new char[] { '-', '_', '.', ' ' });

            string ComplektCode = Regex.Match(DocCode, pattern3).ToString();


            return ComplektCode;
        }
        public string ComputeMD5Checksum(string path)
        {

            string result = string.Empty;

            try
            {
                using (FileStream fs = System.IO.File.OpenRead(path))
                {
                    MD5 md5 = new MD5CryptoServiceProvider();
                    byte[] fileData = new byte[fs.Length];
                    fs.Read(fileData, 0, (int)fs.Length);
                    byte[] checkSum = md5.ComputeHash(fileData);
                    result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                }
            }

            catch (Exception exp)
            {
                /*MessageBox.Show("An error occurred while attempting to load the file. The error is:"
                                + System.Environment.NewLine + exp.ToString() + System.Environment.NewLine);*/

                string error = exp.ToString();

                string pattern1 = @".Переполнение в результате выполнения арифметической операции.";
                string pattern2 = @".Слишком длинный путь или имя файла.";
                string pattern3 = @".Не удалось найти часть пути.";
                string pattern4 = @"' не найден.";


                //убираю из имени файла шифр, получаю описательную часть

                string match = Regex.Match(error, pattern1).ToString();
                if (Regex.Match(error, pattern1).ToString() != "") result = "Переполнение в результате выполнения арифметической операции";
                else if (Regex.Match(error, pattern2).ToString() != "") result = "Слишком длинный путь или имя файла";
                else if (Regex.Match(error, pattern3).ToString() != "") result = "Не удалось найти часть пути";
                else if (Regex.Match(error, pattern4).ToString() != "") result = "Файл не найден";
                else result = "Ошибка";

                return result;
            }
            return result;
        }

        public string CheckChecksum()
        {
            string statusInfo = "";

            if (Checksum == null) Checksum = ComputeMD5Checksum(LocalFilePath);
            else if (Checksum.Length != 32) Checksum = ComputeMD5Checksum(LocalFilePath);

            if (Checksum.Length != 32) statusInfo = "Не удалось вычислить контрольную сумму. " + Checksum + ". \n";
            return statusInfo;
        }
        public string CheckForDublicatesInList(ObservableCollection<Core_Module.LocalFile> filesCollection)
        {
            string statusInfo = "";
            if (Checksum.Length == 32)
            {
                IEnumerable<Core_Module.LocalFile> dublicates = filesCollection.Where(x => x.Checksum == Checksum && x.LocalFilePath != LocalFilePath);

                if (dublicates.Count() > 0)
                {
                    statusInfo = "Обнаружено " + dublicates.Count().ToString() + " повторений данного файла в текущем списке. \n";
                    //foreach (LocalFile file in dublicates) statusInfo += file.LocalFilePath + "\n "; 
                }
            }
            return statusInfo;
        }
        public string CheckForDublicatesInDataBase(ObservableCollection<Core_Module.Files> dbFilesCollection)
        {
            string statusInfo = "";
            if (Checksum.Length == 32)
            {
                
                IEnumerable<Core_Module.Files> dublicates = dbFilesCollection.Where(x => x.Checksum == Checksum);

                if (dublicates.Count() > 0)
                {
                    statusInfo = "Обнаружено " + dublicates.Count().ToString() + " повторений данного файла в базе данных. \n";
                }

            }
            return statusInfo;
        }
        private string CheckForDublicatesInDbDocument(Core_Module.SessionInfo session)
        {
            string statusInfo = "";
            //VMMC_Core.Document doc = getDocument(targetFile.Document.DocumentCode);
            Core_Module.Document doc = new Core_Module.Document(session).GetDocument(Document.DocumentCode);
            Core_Module.Revision rev = new Core_Module.Revision(session).GetRevision(doc.DocumentId, Revision.Number);


            if (Checksum.Length == 32)
            {
                ObservableCollection<Core_Module.Files> dbFilesCollection = new Core_Module.Files(session).GetFilesByRevision(rev.RevisionId);

                IEnumerable<Core_Module.Files> dublicates = dbFilesCollection.Where(x => x.Checksum != Checksum && x.FileName == LocalFileName);

                if (dublicates.Count() > 0)
                {
                    statusInfo = "Для текущей ревизии уже был загружен файл с тем же  " + dublicates.Count().ToString() + " повторений данного файла. \n";
                }

            }
            return statusInfo;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnLocalFilePropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }











    }
}
