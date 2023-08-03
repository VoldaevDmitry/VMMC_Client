using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VMMC_Core
{
    public class Files
    {
        private VMMC_Core.SessionInfo sessionInfo;
        public int FileId { get; set; }
        public Guid FileGuid{ get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public Guid RevisionId { get; set; }
        public int FileSize { get; set; }
        public string LocalPath { get; set; }
        public string Checksum { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public Files(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }

        public ObservableCollection<VMMC_Core.Files> GetFilesByRevision(Guid revisionId)
        {
            ObservableCollection<VMMC_Core.Files> files = new ObservableCollection<VMMC_Core.Files>();

            files.Clear();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files] WHERE RevisionId='" + revisionId.ToString() + "' ";
                //string sql = @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files_1] WHERE RevisionId='" + revisionId.ToString() + "' ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Files newFile = new VMMC_Core.Files(sessionInfo)
                        {
                            FileId = int.Parse(dr["FileId"].ToString()),
                            FileName = dr["FileName"].ToString(),
                            FileType = dr["FileType"].ToString(),
                            RevisionId = Guid.Parse(dr["RevisionId"].ToString()),
                            //FileSize = (int)dr["FileSize"],
                            Checksum = dr["HASHSUM"].ToString()
                        };
                        //newFile.FileSize = 
                        files.Add(newFile);
                    }
                }
                return files;
            }
        }
        public VMMC_Core.Files GetDbFile(string checksum)
        {
            VMMC_Core.Files dbFile = new VMMC_Core.Files(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files] WHERE HASHSUM='" + checksum + "' ";
                //string sql = @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files_1] WHERE HASHSUM='" + checksum + "' ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        dbFile.FileId = int.Parse(dr["FileId"].ToString());
                        dbFile.FileName = dr["FileName"].ToString();
                        dbFile.FileType = dr["FileType"].ToString();
                        dbFile.RevisionId = Guid.Parse(dr["RevisionId"].ToString());
                        dbFile.Checksum = dr["HASHSUM"].ToString();


                    }
                }
                return dbFile;
            }
        }
        public ObservableCollection<VMMC_Core.Files> GetDbFiles()
        {
            ObservableCollection<VMMC_Core.Files> files = new ObservableCollection<VMMC_Core.Files>();
            files.Clear();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            connectionString = sessionInfo.ConnectionString;

            SqlConnection conn = new SqlConnection(connectionString);  // создаём объект для подключения к БД
            conn.Open();// устанавливаем соединение с БД
            string sql = /*"USE [Rakushka] " +*/
                @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files]";
                //@"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files_1]";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VMMC_Core.Files newFile = new VMMC_Core.Files(sessionInfo)
                    {
                        FileId = int.Parse(dr["FileId"].ToString()),
                        FileName = dr["FileName"].ToString(),
                        FileType = dr["FileType"].ToString(),
                        RevisionId = Guid.Parse(dr["RevisionId"].ToString()),
                        //FileSize = (int)dr["FileSize"],
                        Checksum = dr["HASHSUM"].ToString()
                    };
                    //newFile.FileSize = 
                    files.Add(newFile);
                }
            }
            return files;
        }
        public string CreateDbFile() //new version using Stored Procedure
        {
            string logString = "";
            try
            {
                VMMC_Core.Files existFile = GetDbFile(Checksum);
                if (existFile.FileName == null)
                {
                    FileLoaderServiceReference.FileLoaderServiceClient client = new FileLoaderServiceReference.FileLoaderServiceClient();

                    FileLoaderServiceReference.LoadFileInput inputFile = new FileLoaderServiceReference.LoadFileInput();
                                        

                    inputFile.FileInf = new System.IO.FileInfo(@"C:\temp\" + FileName);
                    inputFile.FileData = File.ReadAllBytes(LocalPath);


                    inputFile.Host = Environment.MachineName;
                    inputFile.RepositoryId = 1;
                    inputFile.RevisionId = RevisionId;
                    inputFile.User = Environment.UserName;
                    inputFile.ProjectName = sessionInfo.ProjectCode;


                    FileLoaderServiceReference.LoadFileResult result = client.LoadFile(inputFile);

                    logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Files_1. Guid записи: [" + FileId.ToString() + "]";
                    Status = "Ok";
                    StatusInfo = logString;

                    //return result.Message;
                    return logString;
                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Files, произошла ошибка. Файл с такой же суммой уже существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Files, произошла ошибка. " + e.Message;
                Status = "Error";
                StatusInfo = logString;
            }

            return logString;
        }
        public string CreateDbFileByValues(string filePath, string revisionId) //new version using Stored Procedure
        {

            FileLoaderServiceReference.FileLoaderServiceClient client = new FileLoaderServiceReference.FileLoaderServiceClient();

            FileLoaderServiceReference.LoadFileInput inputFile = new FileLoaderServiceReference.LoadFileInput();


            inputFile.FileInf = new System.IO.FileInfo(filePath);
            inputFile.FileData = File.ReadAllBytes(filePath);

            inputFile.Host = Environment.MachineName;
            inputFile.RepositoryId = 1;
            inputFile.RevisionId = Guid.Parse(revisionId);
            inputFile.User = Environment.UserName;


            FileLoaderServiceReference.LoadFileResult result = client.LoadFile(inputFile);

            return result.Message;
        }
        public string deleteFileFromDataBase(int fileid)
        {
            string logString = "";
            try
            {
                //connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
                string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
                string sql = "DELETE FROM [" + sessionInfo.DataBaseName + "].[dbo].[Files] WHERE [FileId] = " + fileid.ToString();
                //string sql = "DELETE FROM [" + sessionInfo.DataBaseName + "].[dbo].[Files_1] WHERE [FileId] = " + fileid.ToString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand commandToDelete = new SqlCommand(sql, connection);
                    commandToDelete.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                logString = "При удалении записи пользователем " + sessionInfo.UserName + " из таблицы Files_1, произошла ошибка. " + e.Message;
            }
            return logString;
        }

        public ObservableCollection<VMMC_Core.Files> GetFilesByRevision_filestream(Guid revisionId)
        {
            ObservableCollection<VMMC_Core.Files> files = new ObservableCollection<VMMC_Core.Files>();

            files.Clear();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files] WHERE RevisionId='" + revisionId.ToString() + "' ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Files newFile = new VMMC_Core.Files(sessionInfo)
                        {
                            FileGuid = Guid.Parse(dr["FileId"].ToString()),
                            FileName = dr["FileName"].ToString(),
                            FileType = dr["FileType"].ToString(),
                            RevisionId = Guid.Parse(dr["RevisionId"].ToString()),
                            //FileSize = (int)dr["FileSize"],
                            Checksum = dr["HASHSUM"].ToString()
                        };
                        //newFile.FileSize = 
                        files.Add(newFile);
                    }
                }
                return files;
            }
        }
        public VMMC_Core.Files GetDbFile_filestream(string checksum)
        {
            VMMC_Core.Files dbFile = new VMMC_Core.Files(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files] WHERE HASHSUM='" + checksum + "' ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {

                        dbFile.FileGuid = Guid.Parse(dr["FileId"].ToString());
                        dbFile.FileName = dr["FileName"].ToString();
                        dbFile.FileType = dr["FileType"].ToString();
                        dbFile.RevisionId = Guid.Parse(dr["RevisionId"].ToString());
                        dbFile.Checksum = dr["HASHSUM"].ToString();


                    }
                }
                return dbFile;
            }
        }
        public ObservableCollection<VMMC_Core.Files> GetDbFiles_filestream()
        {
            ObservableCollection<VMMC_Core.Files> files = new ObservableCollection<VMMC_Core.Files>();
            files.Clear();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;


            SqlConnection conn = new SqlConnection(connectionString);  // создаём объект для подключения к БД
            conn.Open();// устанавливаем соединение с БД
            string sql = /*"USE [Rakushka] " +*/
                @"SELECT FileId, FileName, FileType, FileSize, HASHSUM, RevisionId FROM [dbo].[Files]";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    VMMC_Core.Files newFile = new VMMC_Core.Files(sessionInfo)
                    {
                        FileGuid = Guid.Parse(dr["FileId"].ToString()),
                        FileName = dr["FileName"].ToString(),
                        FileType = dr["FileType"].ToString(),
                        RevisionId = Guid.Parse(dr["RevisionId"].ToString()),
                        //FileSize = (int)dr["FileSize"],
                        Checksum = dr["HASHSUM"].ToString()
                    };
                    //newFile.FileSize = 
                    files.Add(newFile);
                }
            }
            return files;
        }
        public string CreateDbFile_filestream()
        {
            //connectionString = @"Server=" + SQLServer + ";Database=" + SQLDataBase + ";Trusted_Connection=True";

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            string logString = "";
            try
            {
                VMMC_Core.Files existFile = GetDbFile(Checksum);
                if (existFile.FileName == null)
                {
                    FileInfo fileInfo = new FileInfo(LocalPath);

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        Guid id;
                        using (var transaction = connection.BeginTransaction())
                        {
                            string sqlFilePath;
                            byte[] tranId;
                            using (var sqlCommandInsert = new SqlCommand())
                            {
                                sqlCommandInsert.CommandType = CommandType.StoredProcedure;
                                sqlCommandInsert.CommandText = "dbo.FileInsert";
                                sqlCommandInsert.Connection = connection;
                                sqlCommandInsert.Transaction = transaction;

                                sqlCommandInsert.Parameters.Add(new SqlParameter("@RevisionId", SqlDbType.UniqueIdentifier)
                                {
                                    Value = RevisionId
                                });
                                sqlCommandInsert.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar)
                                {
                                    Value = FileName
                                });

                                sqlCommandInsert.Parameters.Add(new SqlParameter("@FileType", SqlDbType.NVarChar)
                                {
                                    Value = fileInfo.Extension
                                });
                                sqlCommandInsert.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.BigInt)
                                {
                                    Value = fileInfo.Length
                                });
                                sqlCommandInsert.Parameters.Add(new SqlParameter("@HashSum", SqlDbType.NVarChar)
                                {
                                    Value = Checksum
                                });

                                using (var reader = sqlCommandInsert.ExecuteReader())
                                {
                                    reader.Read();
                                    id = FileGuid;
                                    sqlFilePath = (string)reader[1];
                                    tranId = (byte[])reader[2];

                                };

                                using (var sqlFileStream = new SqlFileStream(sqlFilePath, tranId, FileAccess.ReadWrite))
                                {
                                    using (var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                                    {
                                        fileStream.CopyTo(sqlFileStream);
                                    }
                                }
                                transaction.Commit();
                                logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Files. Guid записи: [" + FileId.ToString() + "]";
                                Status = "Ok";
                                StatusInfo = logString;                                
                            }
                        }
                    }
                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Files, произошла ошибка. Файл с такой же суммой уже существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Files, произошла ошибка. " + e.Message;
                Status = "Error";
                StatusInfo = logString;
            }

            return logString;
        }
        public string CreateDbFileByValues_filestream(Guid fileId, string fileName, Guid revisionId, string filePath, string hashSum) //new version using Stored Procedure
        {
            //connectionString = @"Server=" + SQLServer + ";Database=" + SQLDataBase + ";Trusted_Connection=True";

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            string logString = "";
            try
            {

                FileInfo fileInfo = new FileInfo(filePath);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Guid id;
                    using (var transaction = connection.BeginTransaction())
                    {
                        string sqlFilePath;
                        byte[] tranId;
                        using (var sqlCommandInsert = new SqlCommand())
                        {
                            sqlCommandInsert.CommandType = CommandType.StoredProcedure;
                            sqlCommandInsert.CommandText = "dbo.FileInsert";
                            sqlCommandInsert.Connection = connection;
                            sqlCommandInsert.Transaction = transaction;

                            sqlCommandInsert.Parameters.Add(new SqlParameter("@RevisionId", SqlDbType.UniqueIdentifier)
                            {
                                Value = revisionId
                            });
                            sqlCommandInsert.Parameters.Add(new SqlParameter("@FileName", SqlDbType.NVarChar)
                            {
                                Value = fileName
                            });

                            sqlCommandInsert.Parameters.Add(new SqlParameter("@FileType", SqlDbType.NVarChar)
                            {
                                Value = fileInfo.Extension
                            });
                            sqlCommandInsert.Parameters.Add(new SqlParameter("@FileSize", SqlDbType.BigInt)
                            {
                                Value = fileInfo.Length
                            });
                            sqlCommandInsert.Parameters.Add(new SqlParameter("@HashSum", SqlDbType.NVarChar)
                            {
                                Value = hashSum
                            });

                            using (var reader = sqlCommandInsert.ExecuteReader())
                            {
                                reader.Read();
                                id = fileId;
                                sqlFilePath = (string)reader[1];
                                tranId = (byte[])reader[2];

                            };

                            using (var sqlFileStream = new SqlFileStream(sqlFilePath, tranId, FileAccess.ReadWrite))
                            {
                                using (var fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                                {
                                    fileStream.CopyTo(sqlFileStream);
                                }
                            }
                            transaction.Commit();
                            logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Files. Guid записи: [" + fileId.ToString() + "]";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Files, произошла ошибка. " + e.Message;
            }

            return logString;
        }
        public string deleteFileFromDataBase_filestream(Guid fileid)
        {
            string logString = "";
            try
            {
                //connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
                string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
                string sql = "DELETE FROM [" + sessionInfo.DataBaseName + "].[dbo].[Files] WHERE [FileId] = '" + fileid.ToString()+"' ";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand commandToDelete = new SqlCommand(sql, connection);
                    commandToDelete.ExecuteNonQuery();

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                logString = "При удалении записи пользователем " + sessionInfo.UserName + " из таблицы Files, произошла ошибка. " + e.Message;
            }

            return logString;
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
    }
}
