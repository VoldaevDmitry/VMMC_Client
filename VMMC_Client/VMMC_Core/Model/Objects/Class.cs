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
    public class Class
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public string ClassCode { get; set; }
        public string ClassAbbreviation { get; set; }
        public Guid ParenClassId { get; set; }
        public int SystemTypeId { get; set; }

        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public string Info { get; set; }
        public bool IsExistInDB { get; set; }

        public Class(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }

        public VMMC_Core.Class getClass(string search_str)
        {
            VMMC_Core.Class newClass = new VMMC_Core.Class(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            connectionString = sessionInfo.ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [ClassName], [ClassCode], [ParentId], [SystemTypeId], [ClassAbbreviation] FROM [dbo].[Classes] WHERE [ClassName] = '" + search_str+ "' or [ClassCode]= '" + search_str + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        newClass.ClassId = Guid.Parse(dr["Id"].ToString());
                        newClass.ClassName = dr["ClassName"].ToString();
                        newClass.ClassCode = dr["ClassCode"].ToString();
                        if(dr["ParentId"].ToString()!="") newClass.ParenClassId = Guid.Parse(dr["ParentId"].ToString());
                        newClass.SystemTypeId = int.Parse(dr["SystemTypeId"].ToString());
                        newClass.ClassAbbreviation = dr["ClassAbbreviation"].ToString();
                    }
                    return newClass;
                }

                else return null;
            }
        }
        public ObservableCollection<VMMC_Core.Class> getDbClassList()
        {
            ObservableCollection<VMMC_Core.Class> classList = new ObservableCollection<VMMC_Core.Class>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [DocumentId], [Code], [Name], [ClassId] FROM [dbo].[Documents] ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Class newClass = new VMMC_Core.Class(sessionInfo)
                        {
                            ClassId = Guid.Parse(dr["Id"].ToString()),
                            ClassName = dr["ClassName"].ToString(),
                            ClassCode = dr["ClassCode"].ToString(),
                            ParenClassId = Guid.Parse(dr["ParentId"].ToString()),
                            SystemTypeId = int.Parse(dr["SystemTypeId"].ToString()),
                            ClassAbbreviation = dr["ClassAbbreviation"].ToString(),
                        };

                        classList.Add(newClass);
                    }
                }

                return classList;
            }
        }
        public ObservableCollection<Class> getDocumentClasses()
        {
            ObservableCollection<Class> organizations = new ObservableCollection<Class>();
            organizations.Clear();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            SqlConnection conn = new SqlConnection(connectionString);  // создаём объект для подключения к БД
            conn.Open();// устанавливаем соединение с БД
            string sql = /*"USE [Rakushka] " +*/
                @"SELECT [Id] as ClassId
      ,[ClassName] as ClassName
      ,[ClassCode] as ClassCode
      ,[ClassAbbreviation] as ClassAbbreviation
      ,[ParentId] as ParenClassId
      ,[SystemTypeId] as SystemTypeId
  FROM [InfoModelVMMK].[dbo].[Classes]
  WHERE [SystemTypeId] = 2 ";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Class newDocumentClass = new Class(sessionInfo)
                    {
                        ClassId = Guid.Parse(dr["ClassId"].ToString()),
                        ClassName = dr["ClassName"].ToString(),
                        ClassCode = dr["ClassCode"].ToString(),
                        ClassAbbreviation = dr["ClassAbbreviation"].ToString(),
                        //ParenClassId = Guid.Parse(dr["ParenClassId"].ToString()),
                        SystemTypeId = (int)dr["SystemTypeId"]
                    };
                    organizations.Add(newDocumentClass);
                }
            }
            return organizations;
        }

        public string CreateDBClass()
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                VMMC_Core.Class existClass = getClass(ClassCode);
                if (existClass.ClassCode == null)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Classes]";
                        string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Classes] ( [Id], [ClassName], [ClassCode], [ParentId], [SystemTypeId], [ClassAbbreviation]) VALUES ( @ClassId, @ClassName, @ClassCode, @ParentClassId, @SystemTypeId, @ClassAbbreviation )";

                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                        // Add the parameters for the InsertCommand.
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = ClassId;
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassName", SqlDbType.NVarChar)).Value = ClassName;
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassCode", SqlDbType.NVarChar)).Value = ClassCode;
                        commandToIsert.Parameters.Add(new SqlParameter("@ParentClassId", SqlDbType.UniqueIdentifier)).Value = ParenClassId;
                        commandToIsert.Parameters.Add(new SqlParameter("@SystemTypeId", SqlDbType.Int)).Value = SystemTypeId;
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassAbbreviation", SqlDbType.NVarChar)).Value = ClassAbbreviation;

                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();
                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Classes. Guid записи: [" + ClassId.ToString() + "]";
                        Status = "Ok";
                        StatusInfo = logString;
                    }

                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Classes, произошла ошибка. Класс с таким же кодом существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Classes, произошла ошибка. " + e.Message;
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
