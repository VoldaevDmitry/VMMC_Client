using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module
{
    public class Attribute
    {

        public VMMC_Core.SessionInfo sessionInfo;
        public Guid AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
        public Guid? MeasureGroupId { get; set; }
        public int AtributeDataTypeId { get; set; }
        public bool IsEnum { get; set; }
        public bool AllowMultiselect { get; set; }
        public bool AllowMultiValues { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public Attribute(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public VMMC_Core.Attribute GetAttribute(Guid attributeId)
        {
            VMMC_Core.Attribute attribute = new VMMC_Core.Attribute(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [AttributeName], [AttributeDescription], [MeasureGroupId], [AtributeDataTypeId], [IsEnum], [AllowMultiselect], [AllowMultiValues] 
FROM [dbo].[Attributes] WHERE [Id] = '" + attributeId.ToString() + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        
                        attribute.AttributeId = Guid.Parse(dr["Id"].ToString());
                        attribute.AttributeName = dr["AttributeName"].ToString();
                        attribute.AttributeDescription = dr["AttributeDescription"].ToString();
                        attribute.MeasureGroupId = Guid.Parse(dr["MeasureGroupId"].ToString());
                        attribute.AtributeDataTypeId = int.Parse(dr["AtributeDataTypeId"].ToString());
                        attribute.IsEnum = dr["IsEnum"].ToString() == "1" ? true:false ;
                        attribute.AllowMultiselect = dr["AllowMultiselect"].ToString() == "True" ? true : false;
                        attribute.AllowMultiValues = dr["AllowMultiValues"].ToString() == "True" ? true : false;
                        attribute.Status = "Exist";
                        attribute.IsExistInDB = true;
                    }
                    return attribute;
                }
                else return null;
            }
        }
        public VMMC_Core.Attribute SearchAttribute(string attributeName)
        {
            VMMC_Core.Attribute attribute = new VMMC_Core.Attribute(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [AttributeName], [AttributeDescription], [MeasureGroupId], [AtributeDataTypeId], [IsEnum], [AllowMultiselect], [AllowMultiValues] 
FROM [dbo].[Attributes] WHERE [AttributeName] = '" + attributeName + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        string str = dr["IsEnum"].ToString();
                        attribute.AttributeId = Guid.Parse(dr["Id"].ToString());
                        attribute.AttributeName = dr["AttributeName"].ToString();
                        attribute.AttributeDescription = dr["AttributeDescription"].ToString();
                        attribute.MeasureGroupId = Guid.Parse(dr["MeasureGroupId"].ToString());
                        attribute.AtributeDataTypeId = int.Parse(dr["AtributeDataTypeId"].ToString());
                        attribute.IsEnum = dr["IsEnum"].ToString() == "True" ? true : false;
                        attribute.AllowMultiselect = dr["AllowMultiselect"].ToString() == "True" ? true : false;
                        attribute.AllowMultiValues = dr["AllowMultiValues"].ToString() == "True" ? true : false;
                        attribute.Status = "Exist";
                        attribute.IsExistInDB = true;
                    }
                return attribute;
                }
                else return null;
            }
        }
        public List<VMMC_Core.Attribute> GetDbAttributesList()
        {
            List<VMMC_Core.Attribute> attributeList = new List<VMMC_Core.Attribute>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [AttributeName], [AttributeDescription], [MeasureGroupId], [AtributeDataTypeId], [IsEnum], [AllowMultiselect], [AllowMultiValues] FROM [dbo].[Attributes] ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Attribute newAttribute = new VMMC_Core.Attribute(sessionInfo)
                        {
                            AttributeId = Guid.Parse(dr["Id"].ToString()),
                            AttributeName = dr["AttributeName"].ToString(),
                            AttributeDescription = dr["AttributeDescription"].ToString(),
                            MeasureGroupId = Guid.Parse(dr["MeasureGroupId"].ToString()),
                            AtributeDataTypeId = int.Parse(dr["AtributeDataTypeId"].ToString()),
                            IsEnum = dr["IsEnum"].ToString() == "1" ? true : false,
                            AllowMultiselect = dr["AllowMultiselect"].ToString() == "1" ? true : false,
                            AllowMultiValues = dr["AllowMultiValues"].ToString() == "1" ? true : false,
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        attributeList.Add(newAttribute);
                    }
                }

                return attributeList;
            }
        }
        public string CreateDBAttribute()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";


            try
            {
                if (AttributeId != null && AttributeName != null && AttributeName != "")
                {
                    VMMC_Core.Attribute existAttribute = SearchAttribute(AttributeName);

                    if (existAttribute == null)
                    { /*create new atr*/

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Attributes] ";

                            string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Attributes] ([Id], [AttributeName], [MeasureGroupId], [AtributeDataTypeId], [IsEnum], [AllowMultiselect], [AllowMultiValues]) " +
                                "VALUES (@AttributeId, @AttributeName, @MeasureGroupId, @AtributeDataTypeId, @IsEnum, @AllowMultiselect, @AllowMultiValues)";
                            
                            if (AttributeDescription != null) insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Attributes] ([Id], [AttributeName], [AttributeDescription], [MeasureGroupId], [AtributeDataTypeId], [IsEnum], [AllowMultiselect], [AllowMultiValues]) " +
                                "VALUES (@AttributeId, @AttributeName, @AttributeDescription, @MeasureGroupId, @AtributeDataTypeId, @IsEnum, @AllowMultiselect, @AllowMultiValues)";

                            SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                            // Create the InsertCommand.
                            SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                            // Add the parameters for the InsertCommand.
                            commandToIsert.Parameters.Add(new SqlParameter("@AttributeId", SqlDbType.UniqueIdentifier)).Value = AttributeId;
                            commandToIsert.Parameters.Add(new SqlParameter("@AttributeName", SqlDbType.NVarChar)).Value = AttributeName;
                            if (AttributeDescription != null) commandToIsert.Parameters.Add(new SqlParameter("@AttributeDescription", SqlDbType.NVarChar)).Value = AttributeDescription;
                            commandToIsert.Parameters.Add(new SqlParameter("@MeasureGroupId", SqlDbType.UniqueIdentifier)).Value = MeasureGroupId;
                            commandToIsert.Parameters.Add(new SqlParameter("@AtributeDataTypeId", SqlDbType.Int)).Value = AtributeDataTypeId;
                            commandToIsert.Parameters.Add(new SqlParameter("@IsEnum", SqlDbType.Bit)).Value = IsEnum;
                            commandToIsert.Parameters.Add(new SqlParameter("@AllowMultiselect", SqlDbType.Bit)).Value = AllowMultiselect;
                            commandToIsert.Parameters.Add(new SqlParameter("@AllowMultiValues", SqlDbType.Bit)).Value = AllowMultiValues;

                            adapter.InsertCommand = commandToIsert;
                            commandToIsert.ExecuteNonQuery();
                            logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Attributes. Guid записи: [" + AttributeId.ToString() + "]";



                            Status = "Ok";
                            StatusInfo = logString;
                        }
                       
                    }
                    else
                    { /*error*/
                        logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Attributes, произошла ошибка. Атрибут существует в БД";
                        Status = "Error";
                        StatusInfo = logString;
                    }
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Attributes, произошла ошибка. " + e.Message;
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
