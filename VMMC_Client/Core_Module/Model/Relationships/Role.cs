using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core_Module
{
    public class Role
    {
        public Core_Module.SessionInfo sessionInfo;
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }

        public Role(Core_Module.SessionInfo session)
        {
            sessionInfo = session;

        }
        public List<Role> getRoles()
        {
            List<Role> roles = new List<Role>();
            roles.Clear();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;


            SqlConnection conn = new SqlConnection(connectionString);  // создаём объект для подключения к БД
            conn.Open();// устанавливаем соединение с БД
            string sql = "SELECT [RoleId], [RoleName], [RoleDescription] FROM [dbo].[Roles]";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Role newRole = new Role(sessionInfo)
                    {
                        RoleId = dr["RoleId"].ToString(),
                        RoleName = dr["RoleName"].ToString(),
                        RoleDescription = dr["RoleDescription"].ToString(),
                    };
                    roles.Add(newRole);
                }
            }
            return roles;
        }
    }
}
