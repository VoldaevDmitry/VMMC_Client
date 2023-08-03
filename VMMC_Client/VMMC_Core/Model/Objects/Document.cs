using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core
{
    public class Document
    {

        public VMMC_Core.SessionInfo sessionInfo;
        public Guid DocumentId { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public string DocumentName { get; set; }
        public string DocumentCode { get; set; }
        public Guid DocumentClassId { get; set; }
        public ObservableCollection<Revision> Revisions{get; set;}
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public Document(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public VMMC_Core.Document GetDocument(string documentCode)
        {
            VMMC_Core.Document document = new VMMC_Core.Document(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            connectionString = sessionInfo.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [DocumentId], [Name], [Code], [ClassId] FROM [dbo].[Documents] WHERE [Code] = '" + documentCode+"' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        document.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        document.DocumentCode = dr["Code"].ToString();
                        document.DocumentName = dr["Name"].ToString();
                        document.DocumentClassId = Guid.Parse(dr["ClassId"].ToString());
                        document.IsExistInDB = true;
                    }
                    return document;
                }
                else return null;

            }
        }
        public ObservableCollection<VMMC_Core.Document> GetDbDocumentsList()
        {
            ObservableCollection<VMMC_Core.Document> documentList = new ObservableCollection<VMMC_Core.Document>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            connectionString = sessionInfo.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [DocumentId], [Code], [Name], [ClassId] FROM [dbo].[Documents] order by [Code] ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo)
                        {
                            DocumentId = Guid.Parse(dr["DocumentId"].ToString()),
                            DocumentCode = dr["Code"].ToString(),
                            DocumentName = dr["Name"].ToString(),
                            DocumentClassId = Guid.Parse(dr["ClassId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        documentList.Add(newDocument);
                    }
                }

                return documentList;
            }
        }
        public string CreateDBDocument()
        {
            int systemTypeId = 2; // для документов всегда 2
            Guid projectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519");//ВММК
            projectId = sessionInfo.ProjectId;

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                VMMC_Core.Document existDocument = GetDocument(DocumentCode);
                if (existDocument == null)
                {
                    string createDbObjectResult = new VMMC_Core.DbObject(sessionInfo).CreateDbObject(DocumentId, DocumentClassId, systemTypeId, projectId);



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Documents]";
                        string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Documents] ( [DocumentId], [Name], [Code], [ClassId]) VALUES ( @DocumentId, @DocumentName, @DocumentCode, @ClassId )";

                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                        // Add the parameters for the InsertCommand.
                        commandToIsert.Parameters.Add(new SqlParameter("@DocumentId", SqlDbType.UniqueIdentifier)).Value = DocumentId;
                        commandToIsert.Parameters.Add(new SqlParameter("@DocumentName", SqlDbType.NVarChar)).Value = DocumentName;
                        commandToIsert.Parameters.Add(new SqlParameter("@DocumentCode", SqlDbType.NVarChar)).Value = DocumentCode;
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = DocumentClassId;

                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();
                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Documents. Guid записи: [" + DocumentId.ToString() + "]";
                        Status = "Ok";
                        StatusInfo = logString;
                    }

                    //string CreateDBRevisionRowResult = new VMMC_Core.Revision(sessionInfo).CreateDBRevisionByValues(Guid.NewGuid(), DocumentId, 0); //нулевая ревизия по дефолту
                    //foreach (VMMC_Core.Revision revision in Revisions)
                    //{
                    //    //string CreateDBRevisionRowResult = new VMMC_Core.Revision(sessionInfo).CreateDBRevisionByValues(revision.RevisionId, DocumentId, revision.Number);
                    //    string CreateDBRevisionRowResult = new VMMC_Core.Revision(sessionInfo).CreateDBRevision();
                    //}


                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Documents, произошла ошибка. Документ с таким же кодом существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Documents, произошла ошибка. " + e.Message;
                Status = "Error";
                StatusInfo = logString;
                if (e.InnerException != null) innerException = e.InnerException.ToString();
                stackTrace = e.StackTrace;
                errorType = e.Source;
            }

            VMMC_Core.DbLog newLog = new VMMC_Core.DbLog(sessionInfo)
            {
                RecordId = Guid.NewGuid(),
                Message = StatusInfo,
                Type = Status,
                InnerException = innerException,
                StackTrace = stackTrace,
                ErrorType = errorType                
            };

            newLog.CreateLog();

            return logString;
        }
        public string CreateDBDocumentByValues(Guid documentId, string documentName, string documentCode, Guid documentClassId, Guid projectId, int documentRevision)
        {
            int systemTypeId = 2; // для документов всегда 2

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";

            try
            {
                string createDbObjectResult = new VMMC_Core.DbObject(sessionInfo).CreateDbObject(documentId, documentClassId, systemTypeId, projectId);



                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Documents]";
                    string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Documents] ( [DocumentId], [Name], [Code], [ClassId]) VALUES ( @DocumentId, @DocumentName, @DocumentCode, @ClassId )";

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    // Create the InsertCommand.
                    SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                    // Add the parameters for the InsertCommand.
                    commandToIsert.Parameters.Add(new SqlParameter("@DocumentId", SqlDbType.UniqueIdentifier)).Value = documentId;
                    commandToIsert.Parameters.Add(new SqlParameter("@DocumentName", SqlDbType.NVarChar)).Value = documentName;
                    commandToIsert.Parameters.Add(new SqlParameter("@DocumentCode", SqlDbType.NVarChar)).Value = documentCode;
                    commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = documentClassId;

                    adapter.InsertCommand = commandToIsert;
                    commandToIsert.ExecuteNonQuery();
                    logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Documents. Guid записи: [" + documentId.ToString() + "]";
                }

                string CreateDBRevisionRowResult = new VMMC_Core.Revision(sessionInfo).CreateDBRevisionByValues(Guid.NewGuid(), documentId, documentRevision);
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Documents, произошла ошибка. " + e.Message;
            }

           

            return logString;
        }

    }


}
