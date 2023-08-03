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
    public class Complekt
    {
        private VMMC_Core.SessionInfo sessionInfo;
        public Guid ComplektId { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public string ComplektCode { get; set; }
        public string ComplektName { get; set; }
        public Guid ComplektClassId { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public string Info { get; set; }
        public bool IsExistInDB { get; set; }


        public Complekt(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public ObservableCollection<VMMC_Core.Complekt> GetDbComplektsList()
        {
            ObservableCollection<VMMC_Core.Complekt> complektList = new ObservableCollection<VMMC_Core.Complekt>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [ComplektId], [Code], [Name], [ClassId] FROM [dbo].[Complekts] ORDER BY [Code]";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Complekt newComplekt = new VMMC_Core.Complekt(sessionInfo)
                        {
                            ComplektId = Guid.Parse(dr["ComplektId"].ToString()),
                            ComplektCode = dr["Code"].ToString(),
                            ComplektName = dr["Name"].ToString(),
                            ComplektClassId = Guid.Parse(dr["ClassId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        complektList.Add(newComplekt);
                    }
                }

                return complektList;
            }
        }
        public VMMC_Core.Complekt GetComplekt(string complektCode)
        {
            VMMC_Core.Complekt complekt = new VMMC_Core.Complekt(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [ComplektId], [Code], [Name], [ClassId] FROM [dbo].[Complekts] WHERE [Code] = '" + complektCode + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        complekt.ComplektId = Guid.Parse(dr["ComplektId"].ToString());
                        complekt.ComplektCode = dr["Code"].ToString();
                        complekt.ComplektName = dr["Name"].ToString();
                        complekt.ComplektClassId = Guid.Parse(dr["ClassId"].ToString());
                    }
                }

                return complekt;
            }
        }
        public ObservableCollection<VMMC_Core.Complekt> GetComplektsListFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.Complekt> complektList = new ObservableCollection<VMMC_Core.Complekt>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД                
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Complekt newComplekt = new VMMC_Core.Complekt(sessionInfo)
                        {
                            ComplektId = Guid.Parse(dr["ComplektId"].ToString()),
                            ComplektCode = dr["Code"].ToString(),
                            ComplektName = dr["Name"].ToString(),
                            ComplektClassId = Guid.Parse(dr["ClassId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        complektList.Add(newComplekt);
                    }
                }

                return complektList;
            }
        }

        public string CreateDBComplekt()
        {
            int systemTypeId = 5; // для комплектов всегда 5
            Guid projectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519");//ВММК
            projectId = sessionInfo.ProjectId;

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            connectionString = sessionInfo.ConnectionString;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                VMMC_Core.Complekt existComplekt = GetComplekt(ComplektCode);
                if (existComplekt.ComplektCode == null)
                {
                    string createDbObjectResult = new VMMC_Core.DbObject(sessionInfo).CreateDbObject(ComplektId, ComplektClassId, systemTypeId, projectId);



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Complekts]";
                        string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Complekts] ( [ComplektId], [Name], [Code], [ClassId]) VALUES ( @ComplektId, @ComplektName, @ComplektCode, @ClassId )";

                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                        // Add the parameters for the InsertCommand.
                        commandToIsert.Parameters.Add(new SqlParameter("@ComplektId", SqlDbType.UniqueIdentifier)).Value = ComplektId;
                        commandToIsert.Parameters.Add(new SqlParameter("@ComplektName", SqlDbType.NVarChar)).Value = ComplektName;
                        commandToIsert.Parameters.Add(new SqlParameter("@ComplektCode", SqlDbType.NVarChar)).Value = ComplektCode;
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = ComplektClassId;

                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();
                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Complekts. Guid записи: [" + ComplektId.ToString() + "]";
                        Status = "Ok";
                        StatusInfo = logString;
                    }

                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Complekts, произошла ошибка. Документ с таким же кодом существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Complekts, произошла ошибка. " + e.Message;
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

    }
}
