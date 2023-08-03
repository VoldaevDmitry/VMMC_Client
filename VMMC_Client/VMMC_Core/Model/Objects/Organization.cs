using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core
{
    public class Organization
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public string OrganizationId { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public string OrganizationCode { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationDescription { get; set; }
        public string OrganizationShortName { get; set; }
        public string OrganizationINN { get; set; }
        public Organization(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;

        }
        public List<Organization> getOrganizations()
        {
            List<Organization> organizations = new List<Organization>();
            organizations.Clear();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            SqlConnection conn = new SqlConnection(connectionString);  // создаём объект для подключения к БД
            conn.Open();// устанавливаем соединение с БД
            string sql = /*"USE [Rakushka] " +*/
                @"SELECT [Id], [Code], [Name], [Description], [ShortName], [INN] FROM [dbo].[Organizations]";

            // Создать объект Command.
            SqlCommand cmd = new SqlCommand(sql, conn);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Organization newOrganization = new Organization(sessionInfo)
                    {
                        OrganizationId = dr["Id"].ToString(),
                        OrganizationCode = dr["Code"].ToString(),
                        OrganizationName = dr["Name"].ToString(),
                        OrganizationDescription = dr["Description"].ToString(),
                        OrganizationShortName = dr["ShortName"].ToString(),
                        OrganizationINN = dr["INN"].ToString()
                    };
                    organizations.Add(newOrganization);
                }
            }
            return organizations;
        }
    }


}
