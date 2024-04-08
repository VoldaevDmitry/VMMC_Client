using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module
{
    public class DbLog
    {
        private Core_Module.SessionInfo sessionInfo;
        public Guid RecordId { get; set; }
        public DateTime Time { get; set; }
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string UserFIO { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public string InnerException { get; set; }
        public string StackTrace { get; set; }
        public string ErrorType { get; set; }
        public DbLog(Core_Module.SessionInfo session)
        {
            sessionInfo = session;

        }

        public string CreateLog()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT [RecordId], [Time], [HostName], [UserName], [UserFIO], [Message], [Type], [InnerException], [StackTrace], [ErrorType] FROM [dbo].[Logs]";
                    string insertsql = "INSERT INTO ["+ sessionInfo.DataBaseName + "].[dbo].[Logs] ([RecordId], [Time], [HostName], [UserName], [UserFIO], [Message], [Type], [InnerException], [StackTrace], [ErrorType]) VALUES (@RecordId, @Time, @HostName, @UserName, @UserFIO, @Message, @Type, @InnerException, @StackTrace, @ErrorType)";

                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    // Create the InsertCommand.
                    SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                    // Add the parameters for the InsertCommand.
                    commandToIsert.Parameters.Add(new SqlParameter("@RecordId", SqlDbType.UniqueIdentifier)).Value = RecordId;
                    commandToIsert.Parameters.Add(new SqlParameter("@Time", SqlDbType.DateTime)).Value = DateTime.Now;
                    commandToIsert.Parameters.Add(new SqlParameter("@HostName", SqlDbType.NVarChar)).Value = sessionInfo.HostName;
                    commandToIsert.Parameters.Add(new SqlParameter("@UserName", SqlDbType.NVarChar)).Value = sessionInfo.UserName;
                    commandToIsert.Parameters.Add(new SqlParameter("@UserFIO", SqlDbType.NVarChar)).Value = sessionInfo.UserFIO;
                    commandToIsert.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar)).Value = Message;
                    if (Type=="Ok") commandToIsert.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar)).Value = "Info";
                    else commandToIsert.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar)).Value = Type;
                    commandToIsert.Parameters.Add(new SqlParameter("@InnerException", SqlDbType.NVarChar)).Value = InnerException;
                    commandToIsert.Parameters.Add(new SqlParameter("@StackTrace", SqlDbType.NVarChar)).Value = StackTrace;
                    commandToIsert.Parameters.Add(new SqlParameter("@ErrorType", SqlDbType.NVarChar)).Value = ErrorType;

                    adapter.InsertCommand = commandToIsert;
                    commandToIsert.ExecuteNonQuery();
                    logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Logs. Guid записи: [" + RecordId.ToString() + "]";
                }
                
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Logs, произошла ошибка. " + e.Message;
            }

            return logString;


        }

    }
}
