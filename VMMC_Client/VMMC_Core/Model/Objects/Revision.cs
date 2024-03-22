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
    public class Revision
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public Guid RevisionId { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public Guid DocumentId { get; set; }
        public int Number { get; set; }
        public DateTime RevisionDate { get; set; }
        public bool IsCurrent { get; set; }
        public ObservableCollection<Files> Files { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public Revision(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public VMMC_Core.Revision GetRevision(Guid documentId, int revNumber)
        {
            VMMC_Core.Revision revision = new VMMC_Core.Revision(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [RevisionId], [DocumentId], [Number], [RevisionDate], [IsCurrent] FROM [dbo].[Revisions] WHERE [DocumentId] = '" + documentId.ToString() + "' AND [Number] = " + revNumber.ToString();

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        revision.RevisionId = Guid.Parse(dr["RevisionId"].ToString());
                        revision.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        revision.Number = int.Parse(dr["Number"].ToString());
                        revision.RevisionDate = DateTime.Parse(dr["RevisionDate"].ToString());
                        revision.IsCurrent = (bool)dr["IsCurrent"];
                    }
                }

                return revision;
            }
        }
        public ObservableCollection<VMMC_Core.Revision> GetDbDocumentRevisionsList(Guid documentId)
        {
            //VMMC_Core.Revision revision = new VMMC_Core.Revision();
            ObservableCollection<VMMC_Core.Revision> revisionList = new ObservableCollection<VMMC_Core.Revision>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [RevisionId], [DocumentId], [Number], [RevisionDate], [IsCurrent] FROM [dbo].[Revisions] WHERE [DocumentId] = '" + documentId.ToString() + "' ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Revision newRevision = new Revision(sessionInfo);

                        newRevision.RevisionId = Guid.Parse(dr["RevisionId"].ToString());
                        newRevision.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        if (dr["Number"].ToString() == "X") newRevision.Number = 0;
                        else newRevision.Number = int.Parse(dr["Number"].ToString());
                        //newRevision.RevisionDate = DateTime.Parse(dr["RevisionDate"].ToString());
                        newRevision.IsCurrent = (bool)dr["IsCurrent"];
                        newRevision.Status = "Exist";
                        newRevision.IsExistInDB = true;

                        revisionList.Add(newRevision);
                    }
                }

                return revisionList;
            }
        }
        public ObservableCollection<VMMC_Core.Revision> GetDbRevisionsList()
        {
            //VMMC_Core.Revision revision = new VMMC_Core.Revision();
            ObservableCollection<VMMC_Core.Revision> revisionList = new ObservableCollection<VMMC_Core.Revision>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            connectionString = sessionInfo.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [RevisionId], [DocumentId], [Number], [RevisionDate], [IsCurrent] FROM [dbo].[Revisions] ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Revision newRevision = new Revision(sessionInfo);

                        newRevision.RevisionId = Guid.Parse(dr["RevisionId"].ToString());
                        newRevision.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        if (dr["Number"].ToString() == "X") newRevision.Number = 0;
                        else newRevision.Number = int.Parse(dr["Number"].ToString());
                        //newRevision.Number = int.Parse(dr["Number"].ToString());
                        //newRevision.RevisionDate = DateTime.Parse(dr["RevisionDate"].ToString());
                        newRevision.IsCurrent = (bool)dr["IsCurrent"];
                        newRevision.Status = "Exist";
                        newRevision.IsExistInDB = true;

                        revisionList.Add(newRevision);
                    }
                }

                return revisionList;
            }
        }
        public string CreateDBRevision()
        {
            int systemTypeId = 8; // для ревизии всегда 8
            Guid projectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519");//ВММК
            projectId = sessionInfo.ProjectId;
            Guid classId = new VMMC_Core.Class(sessionInfo).getClass("Ревизия - базовый класс").ClassId;

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                VMMC_Core.Revision existRevision = GetRevision(DocumentId, Number);
                if (existRevision.RevisionId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    string createDbObjectResult = new VMMC_Core.DbObject(sessionInfo).CreateDbObject(RevisionId, classId, systemTypeId, projectId);

                    bool isCurent = IsCurentRevision(DocumentId, Number);

                    if (isCurent) UpdateDocumentCurrentRevision(DocumentId);

                    string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Revisions]";
                    string insertsql =
                        "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Revisions] ( [RevisionId], [DocumentId], [Number], [RevisionDate], [IsCurrent] ) " +
                        " VALUES ( @RevisionId, @DocumentId, @Number, @RevisionDate, @IsCurrent)";


                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                        commandToIsert.Parameters.Add(new SqlParameter("@RevisionId", SqlDbType.UniqueIdentifier)).Value = RevisionId;
                        commandToIsert.Parameters.Add(new SqlParameter("@DocumentId", SqlDbType.UniqueIdentifier)).Value = DocumentId;
                        commandToIsert.Parameters.Add(new SqlParameter("@Number", SqlDbType.NVarChar)).Value = Number.ToString();
                        if (RevisionDate != DateTime.Parse("01.01.0001 0:00:00")) commandToIsert.Parameters.Add(new SqlParameter("@RevisionDate", SqlDbType.DateTime)).Value = RevisionDate;
                        else commandToIsert.Parameters.Add(new SqlParameter("@RevisionDate", SqlDbType.NVarChar)).Value = "";
                        commandToIsert.Parameters.Add(new SqlParameter("@IsCurrent", SqlDbType.Bit)).Value = isCurent;

                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();

                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Revisions. Guid записи: [" + RevisionId.ToString() + "]";
                        Status = "Ok";
                        StatusInfo = logString;
                    }
                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Revisions, произошла ошибка. Ревизия уже существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Revisions, произошла ошибка. " + e.Message;
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
        public string CreateDBRevisionByValues(Guid revisionId, Guid documentId, int documentRevision/*, bool isCurrent*/)
        {
            bool isCurent = IsCurentRevision(documentId, documentRevision);

            if (isCurent) UpdateDocumentCurrentRevision(documentId);

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            string logString = "";
            try
            {
                string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Revisions]";
                string insertsql =
                    "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Revisions] ( [RevisionId], [DocumentId], [Number], [RevisionDate], [IsCurrent] ) " +
                    " VALUES ( @RevisionId, @DocumentId, @Number, @RevisionDate, @IsCurrent)";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    // Create the InsertCommand.
                    SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                    commandToIsert.Parameters.Add(new SqlParameter("@RevisionId", SqlDbType.UniqueIdentifier)).Value = revisionId;
                    commandToIsert.Parameters.Add(new SqlParameter("@DocumentId", SqlDbType.UniqueIdentifier)).Value = documentId;
                    commandToIsert.Parameters.Add(new SqlParameter("@Number", SqlDbType.NVarChar)).Value = documentRevision.ToString();
                    commandToIsert.Parameters.Add(new SqlParameter("@RevisionDate", SqlDbType.NVarChar)).Value = RevisionDate;
                    commandToIsert.Parameters.Add(new SqlParameter("@IsCurrent", SqlDbType.Bit)).Value = isCurent;

                    adapter.InsertCommand = commandToIsert;
                    commandToIsert.ExecuteNonQuery();

                    logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Revisions. Guid записи: [" + RevisionId.ToString() + "]";
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Revisions, произошла ошибка. " + e.Message;
            }

            return logString;

        }
        public bool IsCurentRevision(Guid documentId, int documentRevision)
        {
            bool result = false;
            try
            {
                string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();// устанавливаем соединение с БД
                    string sql = @"SELECT [RevisionId], [DocumentId], [Number], [RevisionDate], [IsCurrent] FROM [dbo].[Revisions] WHERE [DocumentId] = '" + documentId.ToString() + "' and [IsCurrent] = 1";
                    // Создать объект Command.
                    SqlCommand cmd = new SqlCommand(sql, connection);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (int.Parse(dr["Number"].ToString()) <= documentRevision) result = true;
                        }
                    }
                    else result = true;

                }
            }
            catch (Exception e)
            {
                return false;
            }

            return result;


        }
        public void UpdateDocumentCurrentRevision(Guid documentId)
        {
            try
            {
                string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();// устанавливаем соединение с БД
                    string updatesql = "UPDATE [" + sessionInfo.DataBaseName + "].[dbo].[Revisions] SET [IsCurrent] = 0 WHERE [DocumentId] = '" + documentId.ToString() + "' and [IsCurrent] = 1";
                    // Создать объект Command.
                    // Create the InsertCommand.
                    SqlCommand commandToUpdate = new SqlCommand(updatesql, connection);


                    commandToUpdate.ExecuteNonQuery();

                }
            }
            catch (Exception e)
            {
                
            }
        }
    }
}
