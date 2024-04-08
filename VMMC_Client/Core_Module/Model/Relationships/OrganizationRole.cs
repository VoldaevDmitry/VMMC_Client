using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module
{
    public class OrganizationRole
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public string OrganizationRoleId { get; set; }
        public string OrganizationId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public OrganizationRole(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public List<OrganizationRole> getOrganizationRoles(string OrganizationId)
        {
            List<OrganizationRole> organizationRoles = new List<OrganizationRole>();
            organizationRoles.Clear();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //    SqlConnection conn = new SqlConnection(connectionString);  // создаём объект для подключения к БД
                connection.Open();// устанавливаем соединение с БД
                string sql = @"SELECT orgRol.[OrganizationRoleId] as OrganizationRoleId, orgRol.[OrganizationId] as OrganizationId, orgRol.[RoleId] as RoleId, rol.RoleName as RoleName 
FROM [dbo].[OrganizationRoles] orgRol 
left join [dbo].[Roles] rol on rol.RoleId = orgRol.RoleId 
WHERE orgRol.[OrganizationId] = '" + OrganizationId + "' ";

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, connection);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        OrganizationRole newOrganizationRole = new OrganizationRole(sessionInfo)
                        {
                            OrganizationRoleId = dr["OrganizationRoleId"].ToString(),
                            OrganizationId = dr["OrganizationId"].ToString(),
                            RoleId = dr["RoleId"].ToString(),
                            RoleName = dr["RoleName"].ToString(),
                        };
                        organizationRoles.Add(newOrganizationRole);
                    }
                }
                return organizationRoles;
            }
        }

        public void AddOrganisationRole(string organizationId, string roleId)
        {
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertsql =
                "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[OrganizationRoles] ([OrganizationRoleId], [OrganizationId], [RoleId]) " +
                " VALUES ( @OrganizationRoleId, @OrganizationId, @RoleId)";

                // Create the InsertCommand.
                SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                // Add the parameters for the InsertCommand.
                commandToIsert.Parameters.Add(new SqlParameter("@OrganizationRoleId", System.Data.SqlDbType.UniqueIdentifier)).Value = Guid.NewGuid();
                commandToIsert.Parameters.Add(new SqlParameter("@OrganizationId", System.Data.SqlDbType.UniqueIdentifier)).Value = Guid.Parse(organizationId);
                commandToIsert.Parameters.Add(new SqlParameter("@RoleId", System.Data.SqlDbType.UniqueIdentifier)).Value = Guid.Parse(roleId);

                commandToIsert.ExecuteNonQuery();

            }

        }
        public void DeleteOrganisationRole(string organizationId, string roleId)
        {
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertsql =
                "DELETE FROM [dbo].[OrganizationRoles] " +
                "WHERE [OrganizationId] = '" + organizationId + "' and [RoleId] = '" + roleId + "' ";

                // Create the InsertCommand.
                SqlCommand commandToDelete = new SqlCommand(insertsql, connection);

                commandToDelete.ExecuteNonQuery();

            }

        }
    }
}
