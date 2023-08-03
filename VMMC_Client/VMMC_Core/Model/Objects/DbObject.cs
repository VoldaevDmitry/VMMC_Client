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
    public class DbObject
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public Guid ObjectId { get; set; }
        public string ObjectCode { get; set; }
        public string ObjectName { get; set; }
        public VMMC_Core.Class ObjectClass { get; set; }
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValueCollection { get; set; }
        public ObservableCollection<VMMC_Core.Relationship> RelationshipCollection { get; set; }
        //public Guid ObjectClassId { get; set; }
        //public string ObjectClassName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public int SystemTypeId { get; set; }
        public Guid ProjectId { get; set; }

        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }

        public DbObject(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }


        public VMMC_Core.DbObject GetObject(Guid objectId)
        {
            VMMC_Core.DbObject dbObject = new VMMC_Core.DbObject(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"
SELECT [ObjectId]
,[ClassId]
,[CreatedDate]
,[CreatedBy]
,[LastModifiedDate]
,[LastModifiedBy]
,[SystemTypeId]
,[ProjectId]

,(select [ClassName] from [dbo].[Classes] where [Id] = db_objects.ClassId) as className
	  
,case
when db_objects.SystemTypeId = 1 then (select [TreeItemCode] from [dbo].[TreeItems] where [Id] =  db_objects.ObjectId) --TreeItem
when db_objects.SystemTypeId = 2 then (select [Code] from [dbo].[Documents] where [DocumentId] =  db_objects.ObjectId) --Document
when db_objects.SystemTypeId = 3 then (select [Position] from [dbo].[Tags] where [Id] =  db_objects.ObjectId) --Tag
when db_objects.SystemTypeId = 5 then (select [Code] from [dbo].[Complekts] where [ComplektId] =  db_objects.ObjectId) --Complekt
when db_objects.SystemTypeId = 6 then (select [Code] from [dbo].[Organizations] where [Id] =  db_objects.ObjectId) --Organization
when db_objects.SystemTypeId = 7 then (select [Mark] from [dbo].[Materials] where [Id] =  db_objects.ObjectId) --Material
else null
end as objectCode

,case
when db_objects.SystemTypeId = 1 then (select [TreeItemName] from [dbo].[TreeItems] where [Id] =  db_objects.ObjectId) --TreeItem
when db_objects.SystemTypeId = 2 then (select [Name] from [dbo].[Documents] where [DocumentId] =  db_objects.ObjectId) --Document
when db_objects.SystemTypeId = 3 then (select [Name] from [dbo].[Tags] where [Id] =  db_objects.ObjectId) --Tag
when db_objects.SystemTypeId = 5 then (select [Name] from [dbo].[Complekts] where [ComplektId] =  db_objects.ObjectId) --Complekt
when db_objects.SystemTypeId = 6 then (select [Name] from [dbo].[Organizations] where [Id] =  db_objects.ObjectId) --Organization
when db_objects.SystemTypeId = 7 then (select [Name] from [dbo].[Materials] where [Id] =  db_objects.ObjectId) --Material
else null
end as objectName
,[ProjectId]

FROM [dbo].[Objects] db_objects

WHERE db_objects.ObjectId='" + objectId + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        dbObject.ObjectId = Guid.Parse(dr["ObjectId"].ToString());
                        dbObject.ObjectCode = dr["objectCode"].ToString();
                        dbObject.ObjectName = dr["objectName"].ToString();
                        dbObject.ObjectClass = new VMMC_Core.Class(sessionInfo);
                        dbObject.ObjectClass.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        dbObject.ObjectClass.ClassName = dr["className"].ToString();                        
                        if (dr["CreatedDate"].ToString()!="") dbObject.CreatedDate = DateTime.Parse(dr["CreatedDate"].ToString());
                        dbObject.CreatedBy = dr["CreatedBy"].ToString();
                        if (dr["LastModifiedDate"].ToString() != "") dbObject.LastModifiedDate = DateTime.Parse(dr["LastModifiedDate"].ToString());
                        dbObject.LastModifiedBy = dr["LastModifiedBy"].ToString();
                        dbObject.SystemTypeId = int.Parse(dr["SystemTypeId"].ToString());
                        dbObject.ProjectId = Guid.Parse(dr["ProjectId"].ToString());
                        dbObject.IsExistInDB = true;

                        //treeItem.TreeItemId = Guid.Parse(dr["Id"].ToString());
                        //treeItem.TreeItemName = dr["TreeItemName"].ToString();
                        //treeItem.TreeItemCode = dr["TreeItemCode"].ToString();
                        //treeItem.TreeItemDescription = dr["TreeItemDescription"].ToString();
                        //treeItem.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        //if (dr["ParentId"].ToString() != "") treeItem.ParentId = Guid.Parse(dr["ParentId"].ToString());
                    }
                }

                return dbObject;
            }
        }
        public string GetObjectCode(Guid objectId)
        {
            string objectCode = "";

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.DataBaseName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT	  
,case
when db_objects.SystemTypeId = 1 then (select [TreeItemCode] from [dbo].[TreeItems] where [Id] =  db_objects.ObjectId) --TreeItem
when db_objects.SystemTypeId = 2 then (select [Code] from [dbo].[Documents] where [DocumentId] =  db_objects.ObjectId) --Document
when db_objects.SystemTypeId = 3 then (select [Position] from [dbo].[Tags] where [Id] =  db_objects.ObjectId) --Tag
when db_objects.SystemTypeId = 5 then (select [Code] from [dbo].[Complekts] where [ComplektId] =  db_objects.ObjectId) --Complekt
when db_objects.SystemTypeId = 6 then (select [Code] from [dbo].[Organizations] where [Id] =  db_objects.ObjectId) --Organization
when db_objects.SystemTypeId = 7 then (select [Mark] from [dbo].[Materials] where [Id] =  db_objects.ObjectId) --Material
else null
end as objectCode
FROM [dbo].[Objects] db_objects
WHERE db_objects.ObjectId= '" + objectId + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {                       
                        objectCode = dr["objectCode"].ToString();
                    }
                }

                return objectCode;
            }
        }
        public string GetObjectName(Guid objectId)
        {
            string objectName = "";

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.DataBaseName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT	  
,case
when db_objects.SystemTypeId = 1 then (select [TreeItemName] from [dbo].[TreeItems] where [Id] =  db_objects.ObjectId) --TreeItem
when db_objects.SystemTypeId = 2 then (select [Name] from [dbo].[Documents] where [DocumentId] =  db_objects.ObjectId) --Document
when db_objects.SystemTypeId = 3 then (select [Name] from [dbo].[Tags] where [Id] =  db_objects.ObjectId) --Tag
when db_objects.SystemTypeId = 5 then (select [Name] from [dbo].[Complekts] where [ComplektId] =  db_objects.ObjectId) --Complekt
when db_objects.SystemTypeId = 6 then (select [Name] from [dbo].[Organizations] where [Id] =  db_objects.ObjectId) --Organization
when db_objects.SystemTypeId = 7 then (select [Name] from [dbo].[Materials] where [Id] =  db_objects.ObjectId) --Material
else null
end as objectName
FROM [dbo].[Objects] db_objects
WHERE db_objects.ObjectId= '" + objectId + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        objectName = dr["objectName"].ToString();
                    }
                }

                return objectName;
            }
        }
        public string CreateDbObject(Guid objectId, Guid classId, int systemTypeId, Guid projectId)
        {
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            string logString = "";
            try
            {

                string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[Objects]";
                string insertsql =
                    "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Objects] ( [ObjectId], [ClassId], [CreatedDate], [CreatedBy], [LastModifiedDate], [LastModifiedBy], [SystemTypeId], [ProjectId] )" +
                    "VALUES ( @ObjectId, @ClassId, @CreatedDate, @CreatedBy, @LastModifiedDate, @LastModifiedBy, @SystemTypeId, @ProjectId)";

                //FileParserForm FileForm = new FileParserForm();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                    // Create the InsertCommand.
                    SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                    // Add the parameters for the InsertCommand.
                    //commandToIsert.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.UniqueIdentifier));
                    commandToIsert.Parameters.Add(new SqlParameter("@ObjectId", SqlDbType.UniqueIdentifier)).Value = objectId;
                    commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = classId;
                    commandToIsert.Parameters.Add(new SqlParameter("@CreatedDate", SqlDbType.DateTime)).Value = DateTime.Now;
                    commandToIsert.Parameters.Add(new SqlParameter("@CreatedBy", SqlDbType.NVarChar, 50)).Value = sessionInfo.UserName;
                    commandToIsert.Parameters.Add(new SqlParameter("@LastModifiedDate", SqlDbType.DateTime)).Value = DateTime.Now;
                    commandToIsert.Parameters.Add(new SqlParameter("@LastModifiedBy", SqlDbType.NVarChar, 50)).Value = sessionInfo.UserName;
                    commandToIsert.Parameters.Add(new SqlParameter("@SystemTypeId", SqlDbType.Int)).Value = systemTypeId;
                    commandToIsert.Parameters.Add(new SqlParameter("@ProjectId", SqlDbType.UniqueIdentifier)).Value = projectId;

                    adapter.InsertCommand = commandToIsert;
                    commandToIsert.ExecuteNonQuery();
                    logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Objects. Guid записи: [" + objectId.ToString() + "]";

                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Objects, произошла ошибка. " + e.Message;
            }

            return logString;
        }       
    }
}
