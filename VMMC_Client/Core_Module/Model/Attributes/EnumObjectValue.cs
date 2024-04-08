using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module
{
    public class EnumObjectValue
    {

        public VMMC_Core.SessionInfo sessionInfo;
        public Guid EnumObjectValueId { get; set; }
        public Guid AttributeObjectValueId { get; set; }
        public VMMC_Core.AttributeObjectValue AttributeObjectValue { get; set; }
        public VMMC_Core.EnumAttributeValue EnumAttributeValue { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }

        public EnumObjectValue(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }
        public VMMC_Core.EnumObjectValue GetEnumObjectValue(Guid valueId, Guid enumId)
        {
            VMMC_Core.EnumObjectValue enumObjectValue = new VMMC_Core.EnumObjectValue(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [ValueId], [EnumId] FROM [dbo].[EnumObjectValues] WHERE [ValueId] = '" + valueId.ToString() + "' and [EnumId] = '" + enumId.ToString() + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        enumObjectValue.EnumObjectValueId = Guid.Parse(dr["Id"].ToString());
                        enumObjectValue.AttributeObjectValue = new AttributeObjectValue(sessionInfo) { AttributeObjectValueId = Guid.Parse(dr["ValueId"].ToString()) };
                        enumObjectValue.EnumAttributeValue = new EnumAttributeValue(sessionInfo) { EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString()) };
                        enumObjectValue.Status = "Exist";
                        enumObjectValue.IsExistInDB = true;
                    }
                    return enumObjectValue;
                }
                else return null;

            }
        }
        public ObservableCollection<VMMC_Core.EnumObjectValue> GetEnumObjectValuesList(Guid valueId)
        {
            ObservableCollection<VMMC_Core.EnumObjectValue> enumObjectValuesList = new ObservableCollection<VMMC_Core.EnumObjectValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [ValueId], [EnumId] FROM [dbo].[EnumObjectValues] WHERE [ValueId] = '" + valueId.ToString() + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.EnumObjectValue newEnumObjectValue = new VMMC_Core.EnumObjectValue(sessionInfo);
                        newEnumObjectValue.EnumObjectValueId = Guid.Parse(dr["Id"].ToString());
                        newEnumObjectValue.AttributeObjectValue = new AttributeObjectValue(sessionInfo) { AttributeObjectValueId = Guid.Parse(dr["ValueId"].ToString()) };
                        //newEnumObjectValue.EnumAttributeValue = new EnumAttributeValue(sessionInfo) { EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString()) };
                        newEnumObjectValue.EnumAttributeValue = new EnumAttributeValue(sessionInfo).GetEnumAttributeValueById( Guid.Parse(dr["EnumId"].ToString()) );
                        newEnumObjectValue.Status = "Exist";
                        newEnumObjectValue.IsExistInDB = true;
                        enumObjectValuesList.Add(newEnumObjectValue);
                    }
                }

                return enumObjectValuesList;
            }
        }
        public ObservableCollection<VMMC_Core.EnumObjectValue> GetAvailibleEnumObjectValuesList(Guid valueId)
        {
            ObservableCollection<VMMC_Core.EnumAttributeValue> availibleEnumAttributeValueList = new VMMC_Core.EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(valueId);
            

            if (availibleEnumAttributeValueList.Count > 0)
            {
                ObservableCollection<VMMC_Core.EnumObjectValue> availibleEnumObjectValuesList = new ObservableCollection<VMMC_Core.EnumObjectValue>();

                foreach (VMMC_Core.EnumAttributeValue enumAttributeValue in availibleEnumAttributeValueList)
                {
                    VMMC_Core.EnumObjectValue newEnumObjectValue = new VMMC_Core.EnumObjectValue(sessionInfo);
                    newEnumObjectValue.EnumObjectValueId = Guid.NewGuid();
                    newEnumObjectValue.AttributeObjectValue = new VMMC_Core.AttributeObjectValue(sessionInfo) { AttributeObjectValueId = valueId };
                    newEnumObjectValue.EnumAttributeValue = enumAttributeValue;
                    availibleEnumObjectValuesList.Add(newEnumObjectValue);
                }
                
                return availibleEnumObjectValuesList;
            }

            else return null;
        }
        public List<VMMC_Core.EnumObjectValue> GetDbEnumValuesList()
        {
            List<VMMC_Core.EnumObjectValue> documentList = new List<VMMC_Core.EnumObjectValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

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
                        VMMC_Core.EnumObjectValue newDocument = new VMMC_Core.EnumObjectValue(sessionInfo)
                        {

                            EnumObjectValueId = Guid.Parse(dr["Id"].ToString()),
                            AttributeObjectValue = new AttributeObjectValue(sessionInfo) { AttributeObjectValueId = Guid.Parse(dr["ValueId"].ToString()) },
                            EnumAttributeValue = new EnumAttributeValue(sessionInfo) { EnumAttributeValueId = Guid.Parse(dr["EnumId"].ToString()) },
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

        public string CreateDBEnumObjectValue()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";


            try
            {
                if (AttributeObjectValue != null && EnumAttributeValue != null)
                {
                    VMMC_Core.EnumObjectValue existEnumObjectValue = GetEnumObjectValue(AttributeObjectValue.AttributeObjectValueId, EnumAttributeValue.EnumAttributeValueId);
                    
                    if (existEnumObjectValue == null|| existEnumObjectValue.EnumObjectValueId== Guid.Empty)
                    { /*create new aov*/

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "SELECT [Id], [ValueId], [EnumId] FROM [dbo].[EnumObjectValues]";
                            string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[EnumObjectValues] ([Id], [ValueId], [EnumId]) VALUES (@EnumObjectValueId, @AttributeObjectValueId, @EnumAttributeValueId)";

                            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            // Create the InsertCommand.
                            SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                            // Add the parameters for the InsertCommand.
                            commandToIsert.Parameters.Add(new SqlParameter("@EnumObjectValueId", SqlDbType.UniqueIdentifier)).Value = EnumObjectValueId;
                            commandToIsert.Parameters.Add(new SqlParameter("@AttributeObjectValueId", SqlDbType.UniqueIdentifier)).Value = AttributeObjectValue.AttributeObjectValueId;
                            commandToIsert.Parameters.Add(new SqlParameter("@EnumAttributeValueId", SqlDbType.UniqueIdentifier)).Value = EnumAttributeValue.EnumAttributeValueId;

                            adapter.InsertCommand = commandToIsert;
                            commandToIsert.ExecuteNonQuery();
                            logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу EnumObjectValues. Guid записи: [" + EnumObjectValueId.ToString() + "]";



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
