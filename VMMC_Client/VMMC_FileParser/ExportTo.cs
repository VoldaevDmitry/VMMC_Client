using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NPOI.XSSF.UserModel;//apache 2.0

using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace VMMC_FileParser
{
    public class ExportTo
    {
        public void ExportToExcel(ObservableCollection<VMMC_Core.LocalFile> localFilesCollection, string folderPath)
        {

            //Рабочая книга Excel
            XSSFWorkbook workbook;
            //Лист в книге Excel
            XSSFSheet worksheet;

            //Создаем рабочую книгу
            workbook = new XSSFWorkbook();
            //Создаём лист в книге
            worksheet = (XSSFSheet)workbook.CreateSheet("Лист 1");

            //Количество заполняемых строк
            //int countRow = DataGridViewWithFilter1.RowCount;
            int countRow = localFilesCollection.Count;
            //Количество заполняемых столбцов
            //int countColumn = DataGridViewWithFilter1.ColumnCount;
            //int countColumn = documetnsGrid.Columns.Count;

            string path = string.Empty;
            //LocalFile ttt1 = (LocalFile)documetnsGrid.Items[0];

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.ShowDialog();
                    if (saveDialog.FileName != "")
                    {

                        for (int i = 0; i < 1; i++)
                        {
                            //Создаем строку
                            var HeaderRow = worksheet.CreateRow(i);

                            List<string> dgColumnHeader = new List<string>();
                            dgColumnHeader.Add("Хэш сумма");
                            dgColumnHeader.Add("Папка - уровень 1");
                            dgColumnHeader.Add("Папка - уровень 2");
                            dgColumnHeader.Add("Папка - уровень 3");
                            dgColumnHeader.Add("Имя файла");
                            dgColumnHeader.Add("Шифр документа");
                            dgColumnHeader.Add("Имя документа");
                            dgColumnHeader.Add("Изм");
                            dgColumnHeader.Add("Тип файла");
                            dgColumnHeader.Add("Путь файла");
                            //Запись имен колонок
                            for (int j = 0; j < dgColumnHeader.Count; j++)
                            {
                                //в строке создаём ячеёку с указанием имени столбца
                                var currentCell = HeaderRow.CreateCell(j);

                                //в ячейку запишем текущее имя столбца
                                currentCell.SetCellValue(dgColumnHeader[j].ToString());

                                //Выравним размер столбца по содержимому
                                worksheet.AutoSizeColumn(j);

                            }
                        }
                        for (int i = 1; i < countRow + 1; i++)
                        {
                            //Создаем строку
                            var currentRow = worksheet.CreateRow(i);




                            //if (dgItem.Checksum == null) dgItem.Checksum = ComputeMD5Checksum(dgItem.LocalFilePath);
                            //else if (dgItem.Checksum.Length != 32) dgItem.Checksum = ComputeMD5Checksum(dgItem.LocalFilePath);

                            List<string> dgItemCell = new List<string>();
                            dgItemCell.Add(localFilesCollection[i - 1].Checksum);
                            dgItemCell.Add(FolderCheck(localFilesCollection[i - 1].LocalFilePath, localFilesCollection[i - 1].LocalFileName, folderPath, 6)[0]);//Папка - уровень 1
                            dgItemCell.Add(FolderCheck(localFilesCollection[i - 1].LocalFilePath, localFilesCollection[i - 1].LocalFileName, folderPath, 6)[1]);//Папка - уровень 2
                            dgItemCell.Add(FolderCheck(localFilesCollection[i - 1].LocalFilePath, localFilesCollection[i - 1].LocalFileName, folderPath, 6)[2]);//Папка - уровень 3
                            dgItemCell.Add(localFilesCollection[i - 1].LocalFileName.ToString());
                            dgItemCell.Add(localFilesCollection[i - 1].Document.DocumentCode.ToString());
                            dgItemCell.Add(localFilesCollection[i - 1].Document.DocumentName.ToString());
                            dgItemCell.Add(localFilesCollection[i - 1].Revision.Number.ToString());
                            dgItemCell.Add(localFilesCollection[i - 1].LocalFileType.ToString());
                            dgItemCell.Add(localFilesCollection[i - 1].LocalFilePath.ToString());

                            //Запускаем цикл по столбцам
                            for (int j = 0; j < dgItemCell.Count; j++)
                            {
                                //в строке создаём ячеёку с указанием столбца
                                var currentCell = currentRow.CreateCell(j);
                                //в ячейку запишем информацию о текущем столбце и строке

                                //currentCell.SetCellValue(exportExcel_DataTable.Rows[i - 1][j].ToString());

                                currentCell.SetCellValue(dgItemCell[j].ToString());
                            }
                        }


                        path = saveDialog.FileName;

                        if (path.LastIndexOf(".xlsx") < 0) path = saveDialog.FileName + ".xlsx";

                        // Удалим файл если он есть уже
                        if (!File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        //запишем всё в файл
                        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(fs);
                        }
                        //Откроем файл
                        Process.Start(path);
                    }
                }
                //return path;
            }
            catch (System.Exception ex)
            {
                //return path;
            }
        }
        public void ExportToExcel2(List<VMMC_Core.Complekt> ComplektsCollection)
        {

            //Рабочая книга Excel
            XSSFWorkbook workbook;
            //Лист в книге Excel
            XSSFSheet worksheet;

            //Создаем рабочую книгу
            workbook = new XSSFWorkbook();
            //Создаём лист в книге
            worksheet = (XSSFSheet)workbook.CreateSheet("Лист 1");

            //Количество заполняемых строк
            //int countRow = DataGridViewWithFilter1.RowCount;
            int countRow = ComplektsCollection.Count;
            //Количество заполняемых столбцов
            //int countColumn = DataGridViewWithFilter1.ColumnCount;
            //int countColumn = documetnsGrid.Columns.Count;

            string path = string.Empty;
            //LocalFile ttt1 = (LocalFile)documetnsGrid.Items[0];

            try
            {
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.ShowDialog();
                    if (saveDialog.FileName != "")
                    {

                        for (int i = 0; i < 1; i++)
                        {
                            //Создаем строку
                            var HeaderRow = worksheet.CreateRow(i);

                            List<string> dgColumnHeader = new List<string>();
                            dgColumnHeader.Add("№");
                            dgColumnHeader.Add("Объект");
                            dgColumnHeader.Add("Комплект РД");
                            dgColumnHeader.Add("Наименование комплекта");
                            dgColumnHeader.Add("Ревизия");
                            dgColumnHeader.Add("Кол-во РД в комплекте");

                            //Запись имен колонок
                            for (int j = 0; j < dgColumnHeader.Count; j++)
                            {
                                //в строке создаём ячеёку с указанием имени столбца
                                var currentCell = HeaderRow.CreateCell(j);

                                //в ячейку запишем текущее имя столбца
                                currentCell.SetCellValue(dgColumnHeader[j].ToString());

                                //Выравним размер столбца по содержимому
                                worksheet.AutoSizeColumn(j);

                            }
                        }
                        for (int i = 1; i < countRow + 1; i++)
                        {
                            //Создаем строку
                            var currentRow = worksheet.CreateRow(i);




                            //if (dgItem.Checksum == null) dgItem.Checksum = ComputeMD5Checksum(dgItem.LocalFilePath);
                            //else if (dgItem.Checksum.Length != 32) dgItem.Checksum = ComputeMD5Checksum(dgItem.LocalFilePath);

                            List<string> dgItemCell = new List<string>();
                            dgItemCell.Add(i.ToString());
                            dgItemCell.Add("ММЦ");//
                            dgItemCell.Add(ComplektsCollection[i - 1].ComplektCode);//
                            dgItemCell.Add(ComplektsCollection[i - 1].ComplektName);//
                            dgItemCell.Add(ComplektsCollection[i - 1].Status);//
                            dgItemCell.Add(ComplektsCollection[i - 1].StatusInfo);//
                            

                            //Запускаем цикл по столбцам
                            for (int j = 0; j < dgItemCell.Count; j++)
                            {
                                //в строке создаём ячеёку с указанием столбца
                                var currentCell = currentRow.CreateCell(j);
                                //в ячейку запишем информацию о текущем столбце и строке

                                //currentCell.SetCellValue(exportExcel_DataTable.Rows[i - 1][j].ToString());

                                currentCell.SetCellValue(dgItemCell[j].ToString());
                            }
                        }


                        path = saveDialog.FileName;

                        if (path.LastIndexOf(".xlsx") < 0) path = saveDialog.FileName + ".xlsx";

                        // Удалим файл если он есть уже
                        if (!File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        //запишем всё в файл
                        using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            workbook.Write(fs);
                        }
                        //Откроем файл
                        Process.Start(path);
                    }
                }
                //return path;
            }
            catch (System.Exception ex)
            {
                //return path;
            }
        }
        private string[] FolderCheck(string pathFile, string nameFile, string pathFolder, int Generation)
        {
            string text = pathFile.Substring(pathFolder.Length + 1);
            string[] folders = text.Split(new char[] { '\\' });
            int count = folders.Length;
            string[] subresult = new string[] { count.ToString(), folders[0] };
            string[] result = new string[Generation];
            for (int i = 0; i < Generation; i++)
            {
                if (i < count - 1) result[i] = folders[i];
                else result[i] = null;
            }

            return result;
        }


    }
}
