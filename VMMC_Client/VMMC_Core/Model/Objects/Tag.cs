using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VMMC_Core
{
    public class Tag
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public Guid TagId { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public string Position { get; set; }
        public string TagName { get; set; }
        public string Characteristic { get; set; }
        public Guid TreeItemId { get; set; }
        public Guid TagClassId { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public Tag(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public VMMC_Core.Tag GetTag(string position)
        {
            VMMC_Core.Tag tag = new VMMC_Core.Tag(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [Position], [Name], [Characteristic], [TreeItemId], [ClassId] FROM [dbo].[Tags] WHERE [Position] = '" + position + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        tag.TagId = Guid.Parse(dr["Id"].ToString());
                        tag.Position = dr["Position"].ToString();
                        tag.TagName = dr["Name"].ToString();
                        tag.Characteristic = dr["Characteristic"].ToString();
                        //tag.TreeItemId = Guid.Parse(dr["TreeItemId"].ToString());
                        tag.TagClassId = Guid.Parse(dr["ClassId"].ToString());
                    }
                    //if (dr["TreeItemId"].ToString() != "")
                    //    tag.TreeItemId = Guid.Parse(dr["TreeItemId"].ToString());
                    return tag;
                }
                else return null;
            }
        }
        public List<VMMC_Core.Tag> GetDbTagsList()
        {
            List<VMMC_Core.Tag> tagList = new List<VMMC_Core.Tag>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [Position], [Name], [Characteristic], [TreeItemId], [ClassId] FROM [dbo].[Tags] ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Tag newTag = new VMMC_Core.Tag(sessionInfo)
                        {
                            TagId = Guid.Parse(dr["Id"].ToString()),
                            Position = dr["Position"].ToString(),
                            TagName = dr["Name"].ToString(),
                            Characteristic = dr["Characteristic"].ToString(),
                            TagClassId = Guid.Parse(dr["ClassId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        if (dr["TreeItemId"].ToString() != "") 
                            newTag.TreeItemId = Guid.Parse(dr["TreeItemId"].ToString());

                        tagList.Add(newTag);
                    }
                }

                return tagList;
            }
        }

        public List<VMMC_Core.Tag> GetDbTagsListFromQuery(string sql)
        {
            List<VMMC_Core.Tag> tagList = new List<VMMC_Core.Tag>();

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
                        VMMC_Core.Tag newTag = new VMMC_Core.Tag(sessionInfo)
                        {
                            TagId = Guid.Parse(dr["Id"].ToString()),
                            Position = dr["Position"].ToString(),
                            TagName = dr["Name"].ToString(),
                            Characteristic = dr["Characteristic"].ToString(),
                            TagClassId = Guid.Parse(dr["ClassId"].ToString()),
                            Status = "Exist",
                            IsExistInDB = true
                            //Revisions = new VMMC_Core.Revision().getDbRevisionsList(DocumentId, SQLServer, SQLDataBase)
                        };
                        if (dr["TreeItemId"].ToString() != "")
                            newTag.TreeItemId = Guid.Parse(dr["TreeItemId"].ToString());

                        tagList.Add(newTag);
                    }
                }

                return tagList;
            }
        }
        
        public string CreateDBTag()
        {
            int systemTypeId = 3; // для тегов всегда 3
            Guid projectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519");//ВММК
            projectId = sessionInfo.ProjectId;

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                VMMC_Core.Tag existTag = GetTag(Position);
                if (existTag == null)
                {
                    string createDbObjectResult = new VMMC_Core.DbObject(sessionInfo).CreateDbObject(TagId, TagClassId, systemTypeId, projectId);



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Tags]";
                        string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Tags] ([Id], [Position], [Name], [Characteristic], [TreeItemId], [ClassId]) VALUES ( @TagId, @Position, @TagName, @Characteristic, @TreeItemId, @ClassId )";
                        if (TreeItemId == Guid.Parse("00000000-0000-0000-0000-000000000000")) insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Tags] ([Id], [Position], [Name], [Characteristic], [ClassId]) VALUES ( @TagId, @Position, @TagName, @Characteristic, @ClassId )";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);
                        if (Characteristic == null) Characteristic = "";
                        // Add the parameters for the InsertCommand.
                        commandToIsert.Parameters.Add(new SqlParameter("@TagId", SqlDbType.UniqueIdentifier)).Value = TagId;
                        commandToIsert.Parameters.Add(new SqlParameter("@Position", SqlDbType.NVarChar)).Value = Position;
                        commandToIsert.Parameters.Add(new SqlParameter("@TagName", SqlDbType.NVarChar)).Value = TagName;
                        commandToIsert.Parameters.Add(new SqlParameter("@Characteristic", SqlDbType.NVarChar)).Value = Characteristic;
                        if (TreeItemId != Guid.Parse("00000000-0000-0000-0000-000000000000")) commandToIsert.Parameters.Add(new SqlParameter("@TreeItemId", SqlDbType.UniqueIdentifier)).Value = TreeItemId;
                        commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = TagClassId;

                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();
                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Tags. Guid записи: [" + TagId.ToString() + "]";
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
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Tags, произошла ошибка. Тег с такой же позицией существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Tags, произошла ошибка. " + e.Message;
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
