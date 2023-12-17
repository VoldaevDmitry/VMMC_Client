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
    public class Relationship
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public Guid RelationshipId { get; set; }
        public Guid RelTypeId { get; set; }
        public VMMC_Core.DbObject LeftObject { get; set; }
        public Guid LeftObjectId { get; set; }
        public VMMC_Core.DbObject RightObject { get; set; }
        public Guid RightObjectId { get; set; }
        public Guid RoleId { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }
        public bool? LeftIsParent { get; set; }

        public Relationship(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }


        public VMMC_Core.Relationship GetRelationship(Guid leftObjectId, Guid rightObjectId)
        {
            VMMC_Core.Relationship relationship = new VMMC_Core.Relationship(sessionInfo);

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [RelTypeId], [LeftObjectId], [RightObjectId], [RoleId], [LeftIsParent] FROM [dbo].[Relationships] WHERE [LeftObjectId] = '" + leftObjectId + "' and [RightObjectId] = '" + rightObjectId + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        relationship.RelationshipId = Guid.Parse(dr["Id"].ToString());
                        relationship.RelTypeId = Guid.Parse(dr["RelTypeId"].ToString());
                        relationship.LeftObjectId = Guid.Parse(dr["LeftObjectId"].ToString());
                        relationship.RightObjectId = Guid.Parse(dr["RightObjectId"].ToString());
                        if(dr["RoleId"].ToString()!="") relationship.RoleId = Guid.Parse(dr["RoleId"].ToString());
                        if(dr["LeftIsParent"].ToString()=="True") relationship.LeftIsParent = true;
                        else if (dr["LeftIsParent"].ToString() == "False") relationship.LeftIsParent = false;
                        
                    }
                }

                return relationship;
            }
        }
        public ObservableCollection<VMMC_Core.Relationship> GetDbRelationshipsList()
        {
            ObservableCollection<VMMC_Core.Relationship> relationshipList = new ObservableCollection<VMMC_Core.Relationship>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [RelTypeId], [LeftObjectId], [RightObjectId], [RoleId], [LeftIsParent] FROM [dbo].[Relationships] ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Relationship newRelationship = new VMMC_Core.Relationship(sessionInfo);


                        if (dr["Id"].ToString()!="") newRelationship.RelationshipId = Guid.Parse(dr["Id"].ToString());
                        if (dr["RelTypeId"].ToString() != "") newRelationship.RelTypeId = Guid.Parse(dr["RelTypeId"].ToString());
                        if (dr["LeftObjectId"].ToString() != "") newRelationship.LeftObjectId = Guid.Parse(dr["LeftObjectId"].ToString());
                        if (dr["RightObjectId"].ToString() != "") newRelationship.RightObjectId = Guid.Parse(dr["RightObjectId"].ToString());
                        if (dr["RoleId"].ToString() != "") newRelationship.RoleId = Guid.Parse(dr["RoleId"].ToString());
                        if(dr["LeftIsParent"].ToString()=="True") newRelationship.LeftIsParent = true;
                        else if (dr["LeftIsParent"].ToString() == "False") newRelationship.LeftIsParent = false;


                        relationshipList.Add(newRelationship);
                    }
                }

                return relationshipList;
            }
        }
        public ObservableCollection<VMMC_Core.Relationship> GetDbObjectRelationshipsList(Guid relObjectId)
        {
            ObservableCollection<VMMC_Core.Relationship> relationshipList = new ObservableCollection<VMMC_Core.Relationship>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = "SELECT [Id], [RelTypeId], [LeftObjectId], [RightObjectId], [RoleId], [LeftIsParent] FROM [dbo].[Relationships] WHERE [LeftObjectId] = '" + relObjectId .ToString()+ "' OR [RightObjectId]= '" + relObjectId.ToString() + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.Relationship newRelationship = new VMMC_Core.Relationship(sessionInfo);


                        if (dr["Id"].ToString() != "") newRelationship.RelationshipId = Guid.Parse(dr["Id"].ToString());
                        if (dr["RelTypeId"].ToString() != "") newRelationship.RelTypeId = Guid.Parse(dr["RelTypeId"].ToString());
                        if (dr["LeftObjectId"].ToString() != "") newRelationship.LeftObjectId = Guid.Parse(dr["LeftObjectId"].ToString());
                        if (dr["RightObjectId"].ToString() != "") newRelationship.RightObjectId = Guid.Parse(dr["RightObjectId"].ToString());
                        if (dr["RoleId"].ToString() != "") newRelationship.RoleId = Guid.Parse(dr["RoleId"].ToString());
                        if(dr["LeftIsParent"].ToString()=="True") newRelationship.LeftIsParent = true;
                        else if(dr["LeftIsParent"].ToString() == "False") newRelationship.LeftIsParent = false;


                        relationshipList.Add(newRelationship);
                    }
                }

                return relationshipList;
            }
        }
        public Guid GetRelationshipTypeId(string relationshipCode)
        {
            Guid relTypeId = new Guid();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT TOP 1 [Id],[Code],[Name],[Description],[LeftClassId],[RightClassId] FROM [dbo].[RelationshipTypes] WHERE [Code] = '"+ relationshipCode + "' ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        relTypeId = Guid.Parse(dr["Id"].ToString());
                    }
                }

                return relTypeId;
            }
        }
        public ObservableCollection<VMMC_Core.Relationship> GetRelationshipsFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.Relationship> relationshipList = new ObservableCollection<VMMC_Core.Relationship>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            try
            {
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
                            VMMC_Core.Relationship newRelationship = new VMMC_Core.Relationship(sessionInfo);


                            if (dr["Id"].ToString() != "") newRelationship.RelationshipId = Guid.Parse(dr["Id"].ToString());
                            if (dr["RelTypeId"].ToString() != "") newRelationship.RelTypeId = Guid.Parse(dr["RelTypeId"].ToString());
                            if (dr["LeftObjectId"].ToString() != "")
                            {
                                newRelationship.LeftObjectId = Guid.Parse(dr["LeftObjectId"].ToString());
                                newRelationship.LeftObject = new VMMC_Core.DbObject(sessionInfo).GetObject(newRelationship.LeftObjectId);
                            }
                            if (dr["RightObjectId"].ToString() != "")
                            {
                                newRelationship.RightObjectId = Guid.Parse(dr["RightObjectId"].ToString());
                                newRelationship.RightObject = new VMMC_Core.DbObject(sessionInfo).GetObject(newRelationship.RightObjectId);
                            }
                            if (dr["RoleId"].ToString() != "") newRelationship.RoleId = Guid.Parse(dr["RoleId"].ToString());
                            if(dr["LeftIsParent"].ToString()=="True") newRelationship.LeftIsParent = true;
                            else if (dr["LeftIsParent"].ToString()=="False") newRelationship.LeftIsParent = false;

                            relationshipList.Add(newRelationship);
                        }
                    }
                    return relationshipList;
                }
            }
            catch (Exception e)
            {
                return null;
            }


        }
        public string CreateDBRelationship()
        {
            int systemTypeId = 2; // для документов всегда 2
            Guid projectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519");//ВММК

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            if (LeftObjectId == Guid.Parse("00000000-0000-0000-0000-000000000000")) LeftObjectId = LeftObject.ObjectId;
            if (RightObjectId == Guid.Parse("00000000-0000-0000-0000-000000000000")) RightObjectId = RightObject.ObjectId;

            try
            {
                VMMC_Core.Relationship existRelationship = GetRelationship(LeftObjectId, RightObjectId);
                if (existRelationship.RelationshipId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        string sql = "SELECT [Id], [RelTypeId], [LeftObjectId], [RightObjectId], [RoleId], [LeftIsParent] FROM [" + sessionInfo.DataBaseName + "].[dbo].[Relationships] ";
                        string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Relationships] ([Id], [RelTypeId], [LeftObjectId], [RightObjectId]";
                        if (RoleId != Guid.Parse("00000000-0000-0000-0000-000000000000")) insertsql += ", [RoleId]";
                        if (LeftIsParent!=null) insertsql += ", [LeftIsParent]";
                        insertsql += ") ";
                        string valuessql = "VALUES ( @RelationshipId, @RelTypeId, @LeftObjectId, @RightObjectId";
                        if (RoleId != Guid.Parse("00000000-0000-0000-0000-000000000000")) valuessql += ", @RoleId";
                        if (LeftIsParent != null) valuessql += ", @LeftIsParent";
                        valuessql += ") ";
                        insertsql = insertsql + valuessql;
                        //if (RoleId == Guid.Parse("00000000-0000-0000-0000-000000000000")) insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[Relationships] ([Id], [RelTypeId], [LeftObjectId], [RightObjectId]) VALUES ( @RelationshipId, @RelTypeId, @LeftObjectId, @RightObjectId )";
                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                        // Add the parameters for the InsertCommand.
                        commandToIsert.Parameters.Add(new SqlParameter("@RelationshipId", SqlDbType.UniqueIdentifier)).Value = RelationshipId;
                        commandToIsert.Parameters.Add(new SqlParameter("@RelTypeId", SqlDbType.UniqueIdentifier)).Value = RelTypeId;
                        commandToIsert.Parameters.Add(new SqlParameter("@LeftObjectId", SqlDbType.UniqueIdentifier)).Value = LeftObjectId;
                        commandToIsert.Parameters.Add(new SqlParameter("@RightObjectId", SqlDbType.UniqueIdentifier)).Value = RightObjectId;
                        if (RoleId != Guid.Parse("00000000-0000-0000-0000-000000000000")) commandToIsert.Parameters.Add(new SqlParameter("@RoleId", SqlDbType.UniqueIdentifier)).Value = RoleId;
                        if (LeftIsParent!=null) commandToIsert.Parameters.Add(new SqlParameter("@LeftIsParent", SqlDbType.Bit)).Value = LeftIsParent;



                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();
                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу Relationships. Guid записи: [" + RelationshipId.ToString() + "]";
                        Status = "Ok";
                        StatusInfo = logString;
                    }

                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Relationships, произошла ошибка. Отношение между указанными объектами уже существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу Relationships, произошла ошибка. " + e.Message;
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
