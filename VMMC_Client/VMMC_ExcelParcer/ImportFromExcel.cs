using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
 
namespace VMMC_ExcelParcer
{
    /// <summary>
    /// Преобразование класса справки между файлом Excel и DataTable
    /// </summary>
    public class ImportFromExcel : IDisposable
    {




        /// <summary>
        /// Записываем данные DataTable в указанный файл Excel
        /// </summary>
        /// <param name = "TargetFileNamePath"> путь к целевому файлу excel </param>
        /// <param name = "sourceData"> данные для записи </param>
        /// <param name = "sheetName"> Имя листа в таблице Excel, вы можете начать самостоятельно в зависимости от ситуации </param>
        /// <param name = "IsWriteColumnName"> Следует ли записывать имя столбца таблицы данных </param>
        /// <returns> возвращает количество записанных строк </returns>
        public static int DataTableToExcel(string TargetFileNamePath, DataTable sourceData, string sheetName, bool IsWriteColumnName)
        {

            // проверка данных
            if (!File.Exists(TargetFileNamePath))
            {
                // Путь к файлу excel не существует
                throw new ArgumentException("Путь к файлу excel не существует или файл excel не создан");
            }
            if (sourceData == null)
            {
                throw new ArgumentException("DataTable для записи не может быть пустым");
            }

            if (sheetName == null && sheetName.Length == 0)
            {
                throw new ArgumentException("Имя листа в excel не может быть пустым или не может быть пустой строкой");
            }



            // Создаем соответствующую книгу в соответствии с суффиксом файла Excel
            IWorkbook workbook = null;
            if (TargetFileNamePath.IndexOf(". xlsx") > 0)
            {// версия Excel 2007 г.
                workbook = new XSSFWorkbook();
            }
            else if (TargetFileNamePath.IndexOf(". xls") > 0) // версия Excel 2003 г.
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                return -1; // Ни совпадение, ни переданный файл не является файлом Excel, возврат напрямую
            }



            // Имя листа для листа Excel
            ISheet sheet = workbook.CreateSheet(sheetName);
            if (sheet == null) return -1; // Если лист не может быть создан, вернемся напрямую


            // Количество строк, записанных в Excel
            int WriteRowCount = 0;



            // Указываем необходимость записать имя столбца, затем записываем имя столбца DataTable, записываем имя столбца в первую строку
            if (IsWriteColumnName)
            {
                // Таблица листа создает новую строку, первую строку
                IRow ColumnNameRow = sheet.CreateRow(0); // индекс 0 представляет первую строку
                                                         // Записываем имя столбца DataTable
                for (int colunmNameIndex = 0; colunmNameIndex < sourceData.Columns.Count; colunmNameIndex++)
                {
                    ColumnNameRow.CreateCell(colunmNameIndex).SetCellValue(sourceData.Columns[colunmNameIndex].ColumnName);
                }
                WriteRowCount++;
            }


            //ввод данных 
            for (int row = 0; row < sourceData.Rows.Count; row++)
            {
                // таблица листов создает новую строку
                IRow newRow = sheet.CreateRow(WriteRowCount);
                for (int column = 0; column < sourceData.Columns.Count; column++)
                {

                    newRow.CreateCell(column).SetCellValue(sourceData.Rows[row][column].ToString());

                }

                WriteRowCount++; // Записываем следующую строку
            }


            // Записываем в Excel
            FileStream fs = new FileStream(TargetFileNamePath, FileMode.Open, FileAccess.Write);
            workbook.Write(fs);

            fs.Flush();
            fs.Close();

            workbook.Close();
            return WriteRowCount;
        }





        /// <summary>
        /// Чтение данных из Excel в DataTable
        /// </summary>
        /// <param name = "sourceFileNamePath"> Путь к файлу Excel </param>
        /// <param name = "sheetName"> имя рабочего листа в файле Excel </param>
        /// <param name = "IsHasColumnName"> Имеет ли файл имена столбцов </param>
        /// <returns> набор результатов DataTable для данных, считанных из Excel </returns>
        public ObservableCollection<DataTable> ExcelToDataTable(string sourceFileNamePath, bool IsHasColumnName)
        {

            if (!File.Exists(sourceFileNamePath))
            {
                throw new ArgumentException("Путь к файлу excel не существует или файл excel не создан");
            }

            //if (sheetName == null || sheetName.Length == 0)
            //{
            //    throw new ArgumentException("Имя рабочего листа не может быть пустым");
            //}

            // Создаем соответствующую книгу в соответствии с суффиксом файла Excel
            IWorkbook workbook = null;
            // открываем файл
            FileStream fs = new FileStream(sourceFileNamePath, FileMode.Open, FileAccess.Read);
            if (sourceFileNamePath.IndexOf(".xlsx") > 0)
            {// Версия Excel 2007 г.
                workbook = new XSSFWorkbook(fs);
            }
            else if (sourceFileNamePath.IndexOf(". xls") > 0) // версия Excel 2003 г.
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                return null; // Ни совпадение, ни переданный файл не является файлом Excel, возврат напрямую
            }

            ObservableCollection<DataTable> listDT = new ObservableCollection<DataTable>();

            for (int sheetNameIndex = 0; sheetNameIndex < workbook.NumberOfSheets; sheetNameIndex++)
            {


                // Получить рабочий лист
                //ISheet sheet = workbook.GetSheet(sheetName);
                ISheet sheet = workbook.GetSheetAt(sheetNameIndex);

                // Не могу получить, возвращаем напрямую
                if (sheet == null) return null;



                // Начинаем чтение номера строки
                int StartReadRow = 0;
                DataTable targetTable = new DataTable();



                // Если в таблице есть имена столбцов, добавляем имена столбцов для DataTable
                if (IsHasColumnName)
                {
                    // Получаем первую строку рабочего листа для чтения
                    IRow columnNameRow = sheet.GetRow(0); // 0 представляет первую строку
                                                          // Получаем количество столбцов в строке (то есть длину строки)
                    if (columnNameRow != null)
                    {
                        int CellLength = columnNameRow.LastCellNum;

                        // Ход чтения
                        for (int columnNameIndex = 0; columnNameIndex < CellLength; columnNameIndex++)
                        {
                            // Если не пусто, читать
                            if (columnNameRow.GetCell(columnNameIndex) != null)
                            {
                                // Получаем значение ячейки
                                string cellValue = columnNameRow.GetCell(columnNameIndex).StringCellValue;
                                if (cellValue != null)
                                {
                                    // Добавляем имена столбцов для DataTable
                                    targetTable.Columns.Add(new DataColumn(cellValue));
                                }
                            }
                        }
                    }

                    StartReadRow++;
                }



                /// Начать чтение данных в таблице листа

                // Получаем количество строк в файле листа
                int RowLength = sheet.LastRowNum;
                // Переход и чтение построчно
                for (int RowIndex = StartReadRow; RowIndex <= RowLength; RowIndex++)
                {
                    // Получаем строку данных, соответствующую нижнему индексу в таблице листа
                    IRow currentRow = sheet.GetRow(RowIndex); // RowIndex представляет строку RowIndex + 1

                    if (currentRow == null) continue; // Указывает, что в текущей строке нет данных, затем продолжить
                                                      // Получаем количество столбцов в строке Row, то есть длину в строке Row
                    int currentColumnLength = currentRow.LastCellNum;

                    // Создаем строки данных DataTable
                    DataRow dataRow = targetTable.NewRow();
                    // Обход и чтение данных
                    for (int columnIndex = 0; columnIndex < targetTable.Columns.Count; columnIndex++)
                    {
                        // Ячейки без данных по умолчанию пусты
                        if (currentRow.GetCell(columnIndex) != null)
                        {
                            dataRow[columnIndex] = currentRow.GetCell(columnIndex);
                        }
                    }
                    // Добавляем строки данных DataTable в DataTable
                    targetTable.Rows.Add(dataRow);
                }
                listDT.Add(targetTable);
            }


            // Освобождаем ресурсы
            fs.Close();
            workbook.Close();

            return listDT;
        }


        #region IDisposable members

        public void Dispose()
        {

        }

        #endregion
    }
}