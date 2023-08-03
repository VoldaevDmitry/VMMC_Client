using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace VMMC_Core
{
    public class AttributeObjectValue
    {

        public VMMC_Core.SessionInfo sessionInfo;
        public Guid AttributeObjectValueId { get; set; }
        //public Guid AttributeId { get; set; }
        //public Guid ObjectId { get; set; }
        public VMMC_Core.Attribute Attribute { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public decimal? NumberValue { get; set; }
        public string StringValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public List<VMMC_Core.EnumObjectValue> EnumObjectValuesList { get; set; }
        public VMMC_Core.EnumObjectValue EnumObjectValue { get; set; }
        public List<VMMC_Core.EnumAttributeValue> AvailibleEnumAttributeValueList { get; set; }
        public List<VMMC_Core.EnumObjectValue> AvailibleValuesList { get; set; }
        public Guid? MeasureId { get; set; }
        public string DisplayValue { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }



        public AttributeObjectValue(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }
        public VMMC_Core.AttributeObjectValue GetAttributeObjectValueById(Guid attributeObjectValueId)
        {
            VMMC_Core.AttributeObjectValue attributeObjectValue = new VMMC_Core.AttributeObjectValue(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id],[AttributeId],[ObjectId],[NumberValue],[StringValue],[DateTimeValue],[MeasureId],[DisplayValue]
FROM [dbo].[AttributeObjectValues] WHERE [Id] = '" + attributeObjectValueId.ToString() + "'";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        attributeObjectValue.AttributeObjectValueId = Guid.Parse(dr["Id"].ToString());
                        //attributeObjectValue.Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["AttributeId"].ToString()) };
                        attributeObjectValue.Attribute = new Attribute(sessionInfo).GetAttribute(Guid.Parse(dr["AttributeId"].ToString()));
                        attributeObjectValue.Object = new DbObject(sessionInfo) { ObjectId = Guid.Parse(dr["ObjectId"].ToString()) };
                        attributeObjectValue.StringValue = dr["StringValue"].ToString();                        
                        attributeObjectValue.Status = "Exist";
                        attributeObjectValue.IsExistInDB = true;
                    }
                }
                return attributeObjectValue;
            }
        }
        public VMMC_Core.AttributeObjectValue GetAttributeObjectValue(Guid attributeId, Guid objectId)
        {
            VMMC_Core.AttributeObjectValue attributeObjectValue = new VMMC_Core.AttributeObjectValue(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"select [Id],[AttributeId],[ObjectId],[NumberValue],[StringValue],[DateTimeValue],[MeasureId],[DisplayValue]
FROM [dbo].[AttributeObjectValues] WHERE [AttributeId] = '" + attributeId.ToString() + "' and [ObjectId] = '" + objectId.ToString() + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        attributeObjectValue.AttributeObjectValueId = Guid.Parse(dr["Id"].ToString());
                        attributeObjectValue.Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["AttributeId"].ToString()) };
                        attributeObjectValue.Object = new DbObject(sessionInfo) { ObjectId = Guid.Parse(dr["ObjectId"].ToString()) };
                        attributeObjectValue.StringValue = dr["StringValue"].ToString();
                        attributeObjectValue.Status = "Exist";
                        attributeObjectValue.IsExistInDB = true;
                    }
                }
                return attributeObjectValue;
            }
        }
        public ObservableCollection<VMMC_Core.AttributeObjectValue> GetDbAttributeObjectValuesList(Guid objectId)
        {
            ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValueList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id],[AttributeId],[ObjectId],[NumberValue],[StringValue],[DateTimeValue],[MeasureId],[DisplayValue] FROM [dbo].[AttributeObjectValues] WHERE [ObjectId] = '"+ objectId .ToString() + "' ORDER BY (SELECT [AttributeName] FROM [dbo].[Attributes] WHERE [Id] = [AttributeId]) ";
                // Создать объект Command.
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.AttributeObjectValue newAttributeObjectValue = new VMMC_Core.AttributeObjectValue(sessionInfo)
                        {
                            AttributeObjectValueId = Guid.Parse(dr["Id"].ToString()),
                            Attribute = new Attribute(sessionInfo).GetAttribute(Guid.Parse(dr["AttributeId"].ToString())),// { AttributeId = Guid.Parse(dr["AttributeId"].ToString()) },
                            Object = new DbObject(sessionInfo) { ObjectId = Guid.Parse(dr["ObjectId"].ToString()) },
                            //StringValue = dr["StringValue"].ToString(),
                            Status = "Exist",
                            IsExistInDB = true
                        };
                        if (dr["DateTimeValue"].ToString() != "") newAttributeObjectValue.DateTimeValue = DateTime.Parse(dr["DateTimeValue"].ToString());
                        else if (dr["NumberValue"].ToString() != "") newAttributeObjectValue.NumberValue = decimal.Parse(dr["NumberValue"].ToString());
                        else if (dr["StringValue"].ToString() != "") newAttributeObjectValue.StringValue = dr["StringValue"].ToString();
                        else 
                        {
                            newAttributeObjectValue.EnumObjectValuesList = new VMMC_Core.EnumObjectValue(sessionInfo).GetEnumObjectValuesList(newAttributeObjectValue.AttributeObjectValueId);
                            newAttributeObjectValue.EnumObjectValue = newAttributeObjectValue.EnumObjectValuesList[0];
                            //newAttributeObjectValue.AvailibleEnumAttributeValueList = new EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(newAttributeObjectValue.Attribute.AttributeId);
                            newAttributeObjectValue.AvailibleValuesList = new EnumObjectValue(sessionInfo).GetAvailibleEnumObjectValuesList(newAttributeObjectValue.Attribute.AttributeId);
                        }

                        attributeObjectValueList.Add(newAttributeObjectValue);
                    }
                }
                return attributeObjectValueList;
            }
        }
        public ObservableCollection<VMMC_Core.AttributeObjectValue> GetDbAttributesValuesList()
        {
            ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValueList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"[Id],[AttributeId],[ObjectId],[NumberValue],[StringValue],[DateTimeValue],[MeasureId],[DisplayValue] FROM [dbo].[[AttributeObjectValues] ";
                // Создать объект Command.
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.AttributeObjectValue newAttributeObjectValue = new VMMC_Core.AttributeObjectValue(sessionInfo)
                        {
                            AttributeObjectValueId = Guid.Parse(dr["Id"].ToString()),
                            Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["AttributeId"].ToString()) },
                            Object = new DbObject(sessionInfo) { ObjectId = Guid.Parse(dr["ObjectId"].ToString()) },
                            //StringValue = dr["StringValue"].ToString(),
                            Status = "Exist",
                            IsExistInDB = true
                        };
                        if (dr["DateTimeValue"].ToString() == "") newAttributeObjectValue.DateTimeValue = DateTime.Parse(dr["DateTimeValue"].ToString());
                        else if (dr["NumberValue"].ToString() == "") newAttributeObjectValue.NumberValue = decimal.Parse(dr["NumberValue"].ToString());
                        else newAttributeObjectValue.StringValue = dr["StringValue"].ToString();

                        attributeObjectValueList.Add(newAttributeObjectValue);
                    }
                }
                return attributeObjectValueList;
            }
        }
        public ObservableCollection<VMMC_Core.AttributeObjectValue> GetAttributesValuesListFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValueList = new ObservableCollection<VMMC_Core.AttributeObjectValue>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();// устанавливаем соединение с БД

                    SqlCommand cmd = new SqlCommand(sql, conn);

                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            VMMC_Core.AttributeObjectValue newAttributeObjectValue = new VMMC_Core.AttributeObjectValue(sessionInfo)
                            {
                                AttributeObjectValueId = Guid.Parse(dr["Id"].ToString()),
                                //Attribute = new Attribute(sessionInfo) { AttributeId = Guid.Parse(dr["AttributeId"].ToString()) },
                                Attribute = new Attribute(sessionInfo).GetAttribute(Guid.Parse(dr["AttributeId"].ToString())),
                                //Object = new DbObject(sessionInfo) { ObjectId = Guid.Parse(dr["ObjectId"].ToString()) },
                                Object = new DbObject(sessionInfo).GetObject(Guid.Parse(dr["ObjectId"].ToString())),
                            };
                            string ttttt = dr["NumberValue"].ToString();
                            if (dr["DateTimeValue"].ToString() != "") 
                                newAttributeObjectValue.DateTimeValue = DateTime.Parse(dr["DateTimeValue"].ToString());
                            else if (dr["NumberValue"].ToString() != "") 
                                newAttributeObjectValue.NumberValue = decimal.Parse(dr["NumberValue"].ToString());
                            else if (dr["StringValue"].ToString() != "") 
                                newAttributeObjectValue.StringValue = dr["StringValue"].ToString();
                            try
                            {
                                string ttt = dr["EnumId"].ToString();
                                if (dr["EnumId"].ToString() != "")
                                {
                                    //EnumObjectValue newEnumObjectValue = new EnumObjectValue(sessionInfo) { EnumObjectValueId = Guid.NewGuid(), AttributeObjectValue = newAttributeObjectValue, EnumAttributeValue = new EnumAttributeValue(sessionInfo) { EnumAttributeValueId = Guid.Parse(dr["AttributeId"].ToString()) } };
                                    EnumObjectValue newEnumObjectValue = new EnumObjectValue(sessionInfo) { EnumObjectValueId = Guid.NewGuid(), AttributeObjectValue = newAttributeObjectValue, EnumAttributeValue = new EnumAttributeValue(sessionInfo).GetEnumAttributeValueById(Guid.Parse(dr["EnumId"].ToString())) };
                                    newAttributeObjectValue.EnumObjectValuesList = new List<EnumObjectValue>();
                                    newAttributeObjectValue.EnumObjectValuesList.Add(newEnumObjectValue);

                                    newAttributeObjectValue.EnumObjectValue = newEnumObjectValue;
                                    newAttributeObjectValue.AvailibleEnumAttributeValueList = new EnumAttributeValue(sessionInfo).GetAvailableEnumAttributeValuesList(newAttributeObjectValue.Attribute.AttributeId);
                                    newAttributeObjectValue.AvailibleValuesList = new EnumObjectValue(sessionInfo).GetAvailibleEnumObjectValuesList(newAttributeObjectValue.Attribute.AttributeId);
                                    foreach (EnumObjectValue enumValue in newAttributeObjectValue.EnumObjectValuesList)
                                    {
                                        EnumAttributeValue SelectedValue = newAttributeObjectValue.AvailibleEnumAttributeValueList.Where(x => x.EnumValueStr == enumValue.EnumAttributeValue.EnumValueStr).FirstOrDefault();
                                        SelectedValue.IsSelected = true;
                                    }
                                }
                            }
                            catch (Exception) {}

                            attributeObjectValueList.Add(newAttributeObjectValue);
                        }
                    }
                    return attributeObjectValueList;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public string CreateDBAttributeObjectValue()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";


            try
            {
                if (Attribute != null && Object != null)
                {
                    VMMC_Core.AttributeObjectValue existAttributeObjectValue = GetAttributeObjectValue(Attribute.AttributeId, Object.ObjectId);
                    if (Attribute.AttributeName == null) Attribute = Attribute.GetAttribute(Attribute.AttributeId);

                    if ((existAttributeObjectValue.AttributeObjectValueId == null || existAttributeObjectValue.AttributeObjectValueId == Guid.Empty /*existAttributeObjectValue.AttributeObjectValueId.ToString() == "00000000-0000-0000-0000-000000000000"*/) || Attribute.AllowMultiValues)
                    { /*create new aov*/

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValue] ";
                            string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValues] ([Id], [AttributeId], [ObjectId], [NumberValue], [StringValue], [DateTimeValue], [MeasureId], [DisplayValue]) VALUES (@AttributeObjectValueId, @AttributeId, @ObjectId, @NumberValue, @StringValue, @DateTimeValue, @MeasureId, @DisplayValue)";
                            insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValues] ([Id], [AttributeId], [ObjectId] ) VALUES (@AttributeObjectValueId, @AttributeId, @ObjectId)";

                            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            // Create the InsertCommand.
                            SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                            // Add the parameters for the InsertCommand.
                            commandToIsert.Parameters.Add(new SqlParameter("@AttributeObjectValueId", SqlDbType.UniqueIdentifier)).Value = AttributeObjectValueId;
                            commandToIsert.Parameters.Add(new SqlParameter("@AttributeId", SqlDbType.UniqueIdentifier)).Value = Attribute.AttributeId;
                            commandToIsert.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.UniqueIdentifier)).Value = Object.ObjectId;

                            adapter.InsertCommand = commandToIsert;
                            commandToIsert.ExecuteNonQuery();                            
                            logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу AttributeObjectValueId. Guid записи: [" + AttributeObjectValueId.ToString() + "]";                            



                            Status = "Ok";
                            StatusInfo = logString;
                        }

                        if (Attribute.IsEnum) 
                        {
                            if(EnumObjectValue != null) EnumObjectValue.CreateDBEnumObjectValue();
                            else 
                            { /*error*/
                                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу AttributeObjectValueId, произошла ошибка. Не удалось распознать атрибут";
                                Status = "Error";
                                StatusInfo = logString;
                            }
                        }
                        else 
                        {
                            UpdateDBAttributeObjectValue();
                        }
                    }
                    else 
                    { /*error*/
                        logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу AttributeObjectValueId, произошла ошибка. Атрибут существует в БД";
                        Status = "Error";
                        StatusInfo = logString;
                    }                    
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу AttributeObjectValueId, произошла ошибка. " + e.Message;
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
        public string UpdateDBAttributeObjectValue()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                if (Attribute != null && Object != null)
                {
                    VMMC_Core.AttributeObjectValue existAttributeObjectValue = GetAttributeObjectValueById(AttributeObjectValueId);

                    if (existAttributeObjectValue.AttributeObjectValueId != null)
                    { /*create new aov*/

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValue]";
                            string insertsql = "";
                            if (NumberValue != null) insertsql = "UPDATE [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValues] SET [NumberValue] = @NumberValue, [MeasureId] = @MeasureId, [DateTimeValue] = NULL, [StringValue] = NULL WHERE [Id] = '" + AttributeObjectValueId + "'";
                            else if (DateTimeValue != null ) insertsql = "UPDATE [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValues] SET [DateTimeValue] = @DateTimeValue, [StringValue] = NULL, [NumberValue] = NULL, [MeasureId] = NULL WHERE [Id] = '" + AttributeObjectValueId + "'";
                            else insertsql = "UPDATE [" + sessionInfo.DataBaseName + "].[dbo].[AttributeObjectValues] SET [StringValue] = @StringValue, [NumberValue] = NULL, [MeasureId] = NULL, [DateTimeValue] = NULL WHERE [Id] = '" + AttributeObjectValueId + "'";

                            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            // Create the InsertCommand.
                            SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                            // Add the parameters for the InsertCommand.
                            if (NumberValue != null) commandToIsert.Parameters.Add(new SqlParameter("@NumberValue", SqlDbType.Decimal)).Value = NumberValue;
                            if (DateTimeValue != null) commandToIsert.Parameters.Add(new SqlParameter("@DateTimeValue", SqlDbType.DateTime)).Value = DateTimeValue;
                            if (StringValue != null) commandToIsert.Parameters.Add(new SqlParameter("@StringValue", SqlDbType.NVarChar)).Value = StringValue;

                            adapter.InsertCommand = commandToIsert;
                            commandToIsert.ExecuteNonQuery();
                            logString = "Пользователь " + sessionInfo.UserName + " изменил запись в таблицу AttributeObjectValueId. Guid записи: [" + AttributeObjectValueId.ToString() + "]";

                            Status = "Ok";
                            StatusInfo = logString;
                        }   
                    }
                    else
                    { /*error*/
                        logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу AttributeObjectValueId, произошла ошибка. Атрибут не существует в БД";
                        Status = "Error";
                        StatusInfo = logString;
                    }
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу AttributeObjectValueId, произошла ошибка. " + e.Message;
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
