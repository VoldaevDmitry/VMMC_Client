using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace VMMC_Core
{
    public class EnumAttributeValue
    {

        public VMMC_Core.SessionInfo sessionInfo;


        public Guid EnumAttributeValueId { get; set; }

        //public VMMC_Core.Attribute Attribute { get; set; }
        public Guid AttributeId { get; set; }
        public decimal? EnumValueNumber { get; set; }
        public string EnumValueStr { get; set; }
        public DateTime? EnumValueDate { get; set; }
        public bool IsSelected { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public VMMC_Core.EnumAttributeValue AvaliableValues { get; set; }

        public EnumAttributeValue(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }
        public VMMC_Core.EnumAttributeValue GetEnumAttributeValue(Guid attributeId, string enumValueStr)
        {
            VMMC_Core.EnumAttributeValue enumAttributeValue = new VMMC_Core.EnumAttributeValue(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [EnumId], [AttributeId], [EnumValueNumber], [EnumValueStr], [EnumValueDate] FROM [dbo].[EnumAttributeValues] WHERE [AttributeId] = '" + attributeId.ToString() + "' and [EnumValueStr] = N'" + enumValueStr + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        enumAttributeValue.EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString());
                        //enumAttributeValue.Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["EnumId"].ToString()) };
                        enumAttributeValue.AttributeId = Guid.Parse(dr["AttributeId"].ToString());
                        enumAttributeValue.EnumValueStr = dr["EnumValueStr"].ToString();
                        enumAttributeValue.Status = "Exist";
                        enumAttributeValue.IsExistInDB = true;
                    }
                    return enumAttributeValue;

                }
                else return null;
            }
        }
        public VMMC_Core.EnumAttributeValue GetEnumAttributeValueById(Guid enumAttributeValueId)
        {
            VMMC_Core.EnumAttributeValue enumAttributeValue = new VMMC_Core.EnumAttributeValue(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [EnumId], [AttributeId], [EnumValueNumber], [EnumValueStr], [EnumValueDate] FROM [dbo].[EnumAttributeValues] WHERE [EnumId] = '" + enumAttributeValueId.ToString() + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        enumAttributeValue.EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString());
                        //enumAttributeValue.Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["EnumId"].ToString()) };
                        enumAttributeValue.AttributeId = Guid.Parse(dr["AttributeId"].ToString());
                        enumAttributeValue.EnumValueStr = dr["EnumValueStr"].ToString();
                        enumAttributeValue.IsSelected = false;
                        enumAttributeValue.Status = "Exist";
                        enumAttributeValue.IsExistInDB = true;
                    }
                }

                return enumAttributeValue;
            }
        }
        public List<VMMC_Core.EnumAttributeValue> GetDbEnumAttributeValuesList()
        {
            List<VMMC_Core.EnumAttributeValue> enumAttributeValueList = new List<VMMC_Core.EnumAttributeValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [EnumId], [AttributeId], [EnumValueNumber], [EnumValueStr], [EnumValueDate] FROM [dbo].[EnumAttributeValues]";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.EnumAttributeValue newEnumAttributeValue = new VMMC_Core.EnumAttributeValue(sessionInfo)
                        {
                            EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString()),
                            //Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["AttributeId"].ToString()) },
                            AttributeId =Guid.Parse(dr["AttributeId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        if (dr["EnumValueNumber"].ToString() != "") newEnumAttributeValue.EnumValueNumber = int.Parse(dr["EnumValueNumber"].ToString());
                        else if (dr["EnumValueDate"].ToString() != "") newEnumAttributeValue.EnumValueDate = DateTime.Parse(dr["EnumValueDate"].ToString());
                        else newEnumAttributeValue.EnumValueStr = dr["EnumValueStr"].ToString();

                        enumAttributeValueList.Add(newEnumAttributeValue);
                    }
                }

                return enumAttributeValueList;
            }
        }
        public List<VMMC_Core.EnumAttributeValue> GetAvailableEnumAttributeValuesList(Guid attributeId)
        {
            List<VMMC_Core.EnumAttributeValue> enumAttributeValueList = new List<VMMC_Core.EnumAttributeValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [EnumId], [AttributeId], [EnumValueNumber], [EnumValueStr], [EnumValueDate] FROM [dbo].[EnumAttributeValues] WHERE [AttributeId] = '"+ attributeId + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.EnumAttributeValue newEnumAttributeValue = new VMMC_Core.EnumAttributeValue(sessionInfo)
                        {
                            EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString()),
                            //Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["EnumId"].ToString()) },
                            AttributeId = Guid.Parse(dr["AttributeId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        if (dr["EnumValueNumber"].ToString() != "") newEnumAttributeValue.EnumValueNumber = int.Parse(dr["EnumValueNumber"].ToString());
                        else if (dr["EnumValueDate"].ToString() != "") newEnumAttributeValue.EnumValueDate = DateTime.Parse(dr["EnumValueDate"].ToString());
                        else newEnumAttributeValue.EnumValueStr = dr["EnumValueStr"].ToString();

                        enumAttributeValueList.Add(newEnumAttributeValue);
                    }
                }

                return enumAttributeValueList;
            }
        }

        public string CreateDBEnumAttributeValue()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";


            try
            {
                if (AttributeId != null && AttributeId != Guid.Empty)
                {
                    VMMC_Core.EnumAttributeValue existEnumAttributeValue = GetEnumAttributeValue(AttributeId, EnumValueStr);

                    if (existEnumAttributeValue == null || existEnumAttributeValue.AttributeId == Guid.Empty)
                    { /*create new aov*/

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "SELECT [EnumId], [AttributeId], [EnumValueNumber], [EnumValueStr], [EnumValueDate] FROM [dbo].[EnumAttributeValues]";
                            string columns = "";
                            if (EnumValueNumber != null) columns += ", [EnumValueNumber]";
                            else if (EnumValueStr != null || EnumValueStr != "") columns += ", [EnumValueStr]";
                            else if (EnumValueDate != null) columns += ", [EnumValueDate]";
                            string values = "";
                            if (EnumValueNumber != null) values += ", @EnumValueNumber";
                            else if (EnumValueStr != null || EnumValueStr != "") values += ", @EnumValueStr";
                            else if (EnumValueDate != null) values += ", @EnumValueDate";

                            string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[EnumAttributeValues] ([EnumId], [AttributeId]"+columns+") VALUES (@EnumId, @AttributeId"+ values + ")";

                            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            // Create the InsertCommand.
                            SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                            // Add the parameters for the InsertCommand.
                            commandToIsert.Parameters.Add(new SqlParameter("@EnumId", SqlDbType.UniqueIdentifier)).Value = EnumAttributeValueId;
                            commandToIsert.Parameters.Add(new SqlParameter("@AttributeId", SqlDbType.UniqueIdentifier)).Value = AttributeId;
                            if (EnumValueNumber != null) commandToIsert.Parameters.Add(new SqlParameter("@EnumValueNumber", SqlDbType.Decimal)).Value = EnumValueNumber;
                            else if (EnumValueStr != null && EnumValueStr != "") commandToIsert.Parameters.Add(new SqlParameter("@EnumValueStr", SqlDbType.NVarChar)).Value = EnumValueStr;
                            else if (EnumValueDate != null) commandToIsert.Parameters.Add(new SqlParameter("@EnumValueDate", SqlDbType.DateTime)).Value = EnumValueDate;

                            adapter.InsertCommand = commandToIsert;
                            commandToIsert.ExecuteNonQuery();
                            logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу EnumAttributeValues. Guid записи: [" + EnumAttributeValueId.ToString() + "]";



                            Status = "Ok";
                            StatusInfo = logString;
                        }

                    }
                    else
                    { /*error*/
                        logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу EnumObjectValues, произошла ошибка. Атрибут существует в БД";
                        Status = "Error";
                        StatusInfo = logString;
                    }
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу EnumObjectValues, произошла ошибка. " + e.Message;
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
