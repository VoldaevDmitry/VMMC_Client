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
using NPOI.SS.UserModel;
using System.Collections.ObjectModel;

namespace VMMC_ExcelParcer
{
    public class ExportTo
    {        
        public void ExportToExcel2(ObservableCollection<VMMC_Core.Complekt> ComplektsCollection)
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
                            if(ComplektsCollection[i - 1].Info != null) dgItemCell.Add(ComplektsCollection[i - 1].Info);//
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
                        // Обход и чтение данных
                            int currentRowNumber = 1;
                        for (int rowIndex = 1; rowIndex < countRow+1; rowIndex++)
                        {
                            int rowNumberColumnIndex = 0;
                            int objectColumnIndex = 1;
                            int codeColumnIndex = 2;
                            int nameColumnIndex = 3;

                            // Ячейки без данных по умолчанию пусты
                            IRow currentRow = workbook.GetSheetAt(0).GetRow(rowIndex);
                            ICell currentRowCell = currentRow.GetCell(codeColumnIndex);

                            int lastInRangeRowIndex = rowIndex;
                            int correction = 0;

                            for (int i = rowIndex + 1; i < countRow; i++)
                            {
                                IRow lastinRangeRow = workbook.GetSheetAt(0).GetRow(i);
                                ICell lastinRangeRowCell = lastinRangeRow.GetCell(codeColumnIndex);

                                if (currentRowCell.StringCellValue == lastinRangeRowCell.StringCellValue)
                                {
                                    //workbook.GetSheetAt(0).GetRow(i).GetCell(rowNumberColumnIndex).SetCellValue("");
                                    workbook.GetSheetAt(0).GetRow(i).GetCell(objectColumnIndex).SetCellValue("");
                                    workbook.GetSheetAt(0).GetRow(i).GetCell(codeColumnIndex).SetCellValue("");
                                    workbook.GetSheetAt(0).GetRow(i).GetCell(nameColumnIndex).SetCellValue("");
                                    lastInRangeRowIndex = i;
                                    correction++;
                                }
                                else break;
                            }
                            if (rowIndex != lastInRangeRowIndex)
                            {
                                //int rowNumber = int.Parse(workbook.GetSheetAt(0).GetRow(rowIndex).GetCell(rowNumberColumnIndex).StringCellValue);
                                //int newRowNumber = 1;
                                //if (rowNumber > 1) 
                                //{
                                //    newRowNumber = int.Parse(workbook.GetSheetAt(0).GetRow(rowIndex-1).GetCell(rowNumberColumnIndex).StringCellValue) +1;
                                //}
                                
                                
                                //var cra1 = new NPOI.SS.Util.CellRangeAddress(rowIndex, lastInRangeRowIndex, rowNumberColumnIndex, rowNumberColumnIndex);
                                var cra2 = new NPOI.SS.Util.CellRangeAddress(rowIndex, lastInRangeRowIndex, objectColumnIndex, objectColumnIndex);
                                var cra3 = new NPOI.SS.Util.CellRangeAddress(rowIndex, lastInRangeRowIndex, codeColumnIndex, codeColumnIndex);
                                var cra4 = new NPOI.SS.Util.CellRangeAddress(rowIndex, lastInRangeRowIndex, nameColumnIndex, nameColumnIndex);
                                //workbook.GetSheetAt(0).AddMergedRegion(cra1);
                                workbook.GetSheetAt(0).AddMergedRegion(cra2);
                                workbook.GetSheetAt(0).AddMergedRegion(cra3);
                                workbook.GetSheetAt(0).AddMergedRegion(cra4);
                            }
                            //workbook.GetSheetAt(0).GetRow(rowIndex).GetCell(rowNumberColumnIndex).SetCellValue(currentRowNumber.ToString());
                            currentRowNumber++;
                            rowIndex = lastInRangeRowIndex;
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
