using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core
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
                }

                return attribute;
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
        

    }
}
