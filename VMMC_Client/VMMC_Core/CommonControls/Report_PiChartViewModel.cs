using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMMC_Core.CommonControls
{
    public class Report_PiChartViewModel
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public string reportName;
        public List<VMMC_Core.Document> DocumentsCollection { get; set; }
        public List<VMMC_Core.ReportItem> ReportItemsCollection { get; set; }
        public List<PieSeries> LegendSeriesCollection { get; set; }
        public Func<ChartPoint, string> PointLabel { get; set; }

        public Report_PiChartViewModel( VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }
        public List<PieSeries> GetRelRDTagReport()
        {
            List<PieSeries> pieSeries = new List<PieSeries>();
            
            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"select distinct IIF(resTab.TargetCount=0, 'N', 'Y') as isHavingRel, COUNT(resTab.DocumentId) as DocCount
from(
SELECT documentsRD.[DocumentId], documentsRD.[Code], COUNT(tags.[Id]) as TargetCount
FROM [dbo].[Documents] documentsRD
left join [dbo].[Relationships] reldocTag on reldocTag.RightObjectId = documentsRD.DocumentId and reldocTag.RelTypeId = 'EC83F27D-1907-EC11-A602-00155D03FA01'
left join [dbo].[Tags] tags on reldocTag.LeftObjectId = tags.Id
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' --and tags.Id is not null
group by documentsRD.[DocumentId], documentsRD.[Name], documentsRD.[Code], documentsRD.[ClassId]
)resTab
group by IIF(resTab.TargetCount=0, 'N', 'Y')";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PieSeries newPieSeries = new PieSeries();
                        newPieSeries.Title = dr["isHavingRel"].ToString();
                        newPieSeries.Values = new ChartValues<int> { int.Parse(dr["DocCount"].ToString()) };
                        //newPieSeries.DataLabels = true;
                        newPieSeries.Tag = true;
                        pieSeries.Add(newPieSeries);
                    }
                }
            }
            return pieSeries;
        }
        public List<VMMC_Core.ReportItem> GetRelRDTagCountDetailsReport(string searchStr)
        {
            List<VMMC_Core.ReportItem> result = new List<VMMC_Core.ReportItem>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string havingStr = "having COUNT(tags.[Id]) > 0";
                if (searchStr == "N") havingStr = "having COUNT(tags.[Id]) = 0";
                if (searchStr == "") havingStr = "";

                string sql = @"SELECT documentsRD.[DocumentId], documentsRD.[Code], documentsRD.[Name], classes.[Id] as ClassId, classes.[ClassName], COUNT(tags.[Id]) as TargetCount
FROM [dbo].[Documents] documentsRD
left join [dbo].[Classes] classes on classes.Id = documentsRD.ClassId
left join [dbo].[Relationships] reldocTag on reldocTag.RightObjectId = documentsRD.DocumentId and reldocTag.RelTypeId = 'EC83F27D-1907-EC11-A602-00155D03FA01'
left join [dbo].[Tags] tags on reldocTag.LeftObjectId = tags.Id
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6'
group by documentsRD.[DocumentId], documentsRD.[Name], documentsRD.[Code], classes.[Id], classes.[ClassName] " + havingStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ////VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo).GetDocument(dr["Code"].ToString());
                        //VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo);
                        //newDocument.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        //newDocument.DocumentCode = dr["Code"].ToString();
                        //newDocument.DocumentName = dr["Name"].ToString();
                        //newDocument.StatusInfo = dr["TargetCount"].ToString();
                        //newDocument.IsExistInDB = true;
                        //result.Add(newDocument);


                        VMMC_Core.ReportItem newReportItem = new VMMC_Core.ReportItem();
                        //newReportItem.Item = new DbObject(sessionInfo).GetObject(Guid.Parse(dr["DocumentId"].ToString()));
                        newReportItem.Item = new DbObject(sessionInfo);
                        newReportItem.Item.ObjectId = Guid.Parse(dr["DocumentId"].ToString());
                        newReportItem.Item.ObjectCode = dr["Code"].ToString();
                        newReportItem.Item.ObjectName = dr["Name"].ToString();
                        newReportItem.Item.ObjectClass = new Class(sessionInfo);
                        newReportItem.Item.ObjectClass.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        newReportItem.Item.ObjectClass.ClassName = dr["ClassName"].ToString();

                        newReportItem.CountValue = int.Parse(dr["TargetCount"].ToString());
                        result.Add(newReportItem);
                    }
                }
            }
            return result;
        }
        public List<PieSeries> GetRelRD3DReport()
        {
            List<PieSeries> pieSeries = new List<PieSeries>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"select distinct IIF(resTab.TargetCount=0, 'N', 'Y') as isHavingRel, COUNT(resTab.DocumentId) as DocCount
from(
SELECT documentsRD.[DocumentId], documentsRD.[Code], COUNT(documents3D.DocumentId) as TargetCount
FROM [dbo].[Documents] documentsRD
left join [dbo].[Relationships] reldoc3d on reldoc3d.RightObjectId = documentsRD.DocumentId and reldoc3d.RelTypeId = 'A27B1D5C-0A5D-4AD8-A2F0-E52E7E40E67A'
left join [dbo].[Documents] documents3D on reldoc3d.LeftObjectId = documents3D.DocumentId
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' 
group by documentsRD.[DocumentId], documentsRD.[Name], documentsRD.[Code], documentsRD.[ClassId]
)resTab
group by  IIF(resTab.TargetCount=0, 'N', 'Y')";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PieSeries newPieSeries = new PieSeries();
                        newPieSeries.Title = dr["isHavingRel"].ToString();
                        newPieSeries.Values = new ChartValues<int> { int.Parse(dr["DocCount"].ToString()) };
                        //newPieSeries.DataLabels = true;

                        pieSeries.Add(newPieSeries);
                    }
                }
            }
            return pieSeries;
        }
        public List<VMMC_Core.ReportItem> GetRelRD3DCountDetailsReport(string searchStr)
        {
            List<VMMC_Core.ReportItem> result = new List<VMMC_Core.ReportItem>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string havingStr = "having COUNT(documents3D.DocumentId) > 0";
                if (searchStr =="N") havingStr = "having COUNT(documents3D.DocumentId) = 0";
                if (searchStr == "") havingStr = "";

                string sql = @"SELECT documentsRD.[DocumentId], documentsRD.[Code], documentsRD.[Name], classes.[Id] as ClassId, classes.[ClassName], COUNT(documents3D.DocumentId) as TargetCount
FROM [dbo].[Documents] documentsRD
left join [dbo].[Classes] classes on classes.Id = documentsRD.ClassId
left join [dbo].[Relationships] reldoc3d on reldoc3d.RightObjectId = documentsRD.DocumentId and reldoc3d.RelTypeId = 'A27B1D5C-0A5D-4AD8-A2F0-E52E7E40E67A'
left join [dbo].[Documents] documents3D on reldoc3d.LeftObjectId = documents3D.DocumentId
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' 
group by documentsRD.[DocumentId], documentsRD.[Name], documentsRD.[Code], classes.[Id], classes.[ClassName] " + havingStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ////VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo).GetDocument(dr["Code"].ToString());
                        //VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo);
                        //newDocument.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        //newDocument.DocumentCode = dr["Code"].ToString();
                        //newDocument.DocumentName = dr["Name"].ToString();
                        //newDocument.StatusInfo = dr["TargetCount"].ToString();
                        //result.Add(newDocument);


                        VMMC_Core.ReportItem newReportItem = new VMMC_Core.ReportItem();
                        //newReportItem.Item = new DbObject(sessionInfo).GetObject(Guid.Parse(dr["DocumentId"].ToString()));
                        newReportItem.Item = new DbObject(sessionInfo);
                        newReportItem.Item.ObjectId = Guid.Parse(dr["DocumentId"].ToString());
                        newReportItem.Item.ObjectCode = dr["Code"].ToString();
                        newReportItem.Item.ObjectName = dr["Name"].ToString();
                        newReportItem.Item.ObjectClass = new Class(sessionInfo);
                        newReportItem.Item.ObjectClass.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        newReportItem.Item.ObjectClass.ClassName = dr["ClassName"].ToString();
                        newReportItem.CountValue = int.Parse(dr["TargetCount"].ToString());
                        result.Add(newReportItem);
                    }
                }
            }
            return result;
        }
        public List<PieSeries> GetRelRDTagCountReport()
        {
            List<PieSeries> pieSeries = new List<PieSeries>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"select distinct resTab.TargetCount, COUNT(resTab.DocumentId) as DocCount
from(
SELECT documentsRD.[DocumentId], documentsRD.[Code], COUNT(tags.[Id]) as TargetCount
FROM [dbo].[Documents] documentsRD
left join [dbo].[Relationships] reldocTag on reldocTag.RightObjectId = documentsRD.DocumentId and reldocTag.RelTypeId = 'EC83F27D-1907-EC11-A602-00155D03FA01'
left join [dbo].[Tags] tags on reldocTag.LeftObjectId = tags.Id
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' and tags.Id is not null
group by documentsRD.[DocumentId], documentsRD.[Name], documentsRD.[Code], documentsRD.[ClassId]
)resTab
group by resTab.TargetCount
order by resTab.TargetCount --COUNT(resTab.DocumentId)";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PieSeries newPieSeries = new PieSeries();
                        newPieSeries.Title = dr["TargetCount"].ToString();
                        newPieSeries.Values = new ChartValues<int> { int.Parse(dr["DocCount"].ToString()) };

                        pieSeries.Add(newPieSeries);
                    }
                }
            }
            return pieSeries;
        }
        public List<VMMC_Core.ReportItem> GetRelRDTagCountDetailsReport(int countTags)
        {
            List<VMMC_Core.ReportItem> result = new List<VMMC_Core.ReportItem>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string havingStr = "having COUNT(tags.[Id]) = "+ countTags.ToString();
                if(countTags<0)havingStr = "";

                string sql = @"SELECT documentsRD.[DocumentId], documentsRD.[Code], documentsRD.[Name], classes.[Id] as ClassId, classes.[ClassName], COUNT(tags.[Id]) as TargetCount
FROM [dbo].[Documents] documentsRD
left join [dbo].[Classes] classes on classes.Id = documentsRD.ClassId
left join [dbo].[Relationships] reldocTag on reldocTag.RightObjectId = documentsRD.DocumentId and reldocTag.RelTypeId = 'EC83F27D-1907-EC11-A602-00155D03FA01'
left join [dbo].[Tags] tags on reldocTag.LeftObjectId = tags.Id
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' and tags.Id is not null
group by documentsRD.[DocumentId], documentsRD.[Name], documentsRD.[Code], classes.[Id], classes.[ClassName] " + havingStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ////VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo).GetDocument(dr["Code"].ToString());
                        //VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo);
                        //newDocument.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        //newDocument.DocumentCode = dr["Code"].ToString();
                        //newDocument.DocumentName = dr["Name"].ToString();
                        ////newDocument.StatusInfo = dr["ClassName"].ToString();

                        //result.Add(newDocument);


                        VMMC_Core.ReportItem newReportItem = new VMMC_Core.ReportItem();
                        //newReportItem.Item = new DbObject(sessionInfo).GetObject(Guid.Parse(dr["DocumentId"].ToString()));
                        newReportItem.Item = new DbObject(sessionInfo);
                        newReportItem.Item.ObjectId = Guid.Parse(dr["DocumentId"].ToString());
                        newReportItem.Item.ObjectCode = dr["Code"].ToString();
                        newReportItem.Item.ObjectName = dr["Name"].ToString();
                        newReportItem.Item.ObjectClass = new Class(sessionInfo);
                        newReportItem.Item.ObjectClass.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        newReportItem.Item.ObjectClass.ClassName = dr["ClassName"].ToString();
                        result.Add(newReportItem);
                    }
                }
            }
            return result;
        }


        public List<PieSeries> GetRelDocTypeCountReport()
        {
            List<PieSeries> pieSeries = new List<PieSeries>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT distinct
domainClasses.ClassName
,count(documents.[DocumentId]) as CountDoc
FROM [dbo].[Documents] documents
left join [dbo].[Classes] classes on classes.Id = documents.ClassId
left join [dbo].[Classes] domainClasses on domainClasses.Id = IIF((SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId])='47A89024-872A-EC11-A602-00155D03FA01', documents.[ClassId], (SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId]))
group by domainClasses.ClassName
Order by domainClasses.ClassName";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PieSeries newPieSeries = new PieSeries();
                        newPieSeries.Title = dr["ClassName"].ToString();
                        newPieSeries.Values = new ChartValues<int> { int.Parse(dr["CountDoc"].ToString()) };
                        //newPieSeries.DataLabels = true;

                        pieSeries.Add(newPieSeries);
                    }
                }
            }
            return pieSeries;
        }
        public List<VMMC_Core.ReportItem> GetRelDocTypeCountDetailsReport(string className)
        {
            List<VMMC_Core.ReportItem> result = new List<VMMC_Core.ReportItem>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string serchStr = "WHERE domainClasses.ClassName = '"+ className + "' Order by domainClasses.ClassName ";
                if (className == "") serchStr = "Order by domainClasses.ClassName ";

                string sql = @"SELECT distinct
documents.[DocumentId]
,documents.[Code]
,documents.[Name]
,domainClasses.ClassName as DocType
,classes.Id as ClassId
,classes.ClassName

FROM [dbo].[Documents] documents
left join [dbo].[Classes] classes on classes.Id = documents.ClassId
left join [dbo].[Classes] domainClasses on domainClasses.Id = IIF((SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId])='47A89024-872A-EC11-A602-00155D03FA01', documents.[ClassId], (SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId])) " + serchStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ////VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo).GetDocument(dr["Code"].ToString());
                        //VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo);
                        //newDocument.DocumentId = Guid.Parse(dr["DocumentId"].ToString());
                        //newDocument.DocumentCode = dr["Code"].ToString();
                        //newDocument.DocumentName = dr["Name"].ToString();
                        //newDocument.StatusInfo = dr["ClassName"].ToString();

                        //result.Add(newDocument);

                        VMMC_Core.ReportItem newReportItem = new VMMC_Core.ReportItem();
                        //newReportItem.Item = new DbObject(sessionInfo).GetObject(Guid.Parse(dr["DocumentId"].ToString()));
                        newReportItem.Item = new DbObject(sessionInfo);
                        newReportItem.Item.ObjectId = Guid.Parse(dr["DocumentId"].ToString());
                        newReportItem.Item.ObjectCode = dr["Code"].ToString();
                        newReportItem.Item.ObjectName = dr["Name"].ToString();
                        newReportItem.Item.ObjectClass = new Class(sessionInfo);
                        newReportItem.Item.ObjectClass.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        newReportItem.Item.ObjectClass.ClassName = dr["ClassName"].ToString();
                        result.Add(newReportItem);
                    }
                }
            }
            return result;
        }
        public List<PieSeries> GetRelDocClassCountReport(string docType)
        {
            List<PieSeries> pieSeries = new List<PieSeries>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT distinct
domainClasses.ClassName as DocType
,classes.ClassName
,count(documents.[DocumentId]) as CountDoc
FROM [dbo].[Documents] documents
left join [dbo].[Classes] classes on classes.Id = documents.ClassId
left join [dbo].[Classes] domainClasses on domainClasses.Id = IIF((SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId])='47A89024-872A-EC11-A602-00155D03FA01', documents.[ClassId], (SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId]))
group by domainClasses.ClassName,classes.ClassName
having domainClasses.ClassName = '" + docType + "' Order by classes.ClassName";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        PieSeries newPieSeries = new PieSeries();
                        newPieSeries.Title = dr["ClassName"].ToString();
                        newPieSeries.Values = new ChartValues<int> { int.Parse(dr["CountDoc"].ToString()) };
                        //newPieSeries.DataLabels = true;

                        pieSeries.Add(newPieSeries);
                    }
                }
            }
            return pieSeries;
        }
        public List<VMMC_Core.ReportItem> GetRelDocClassCountDetailsReport(string className)
        {
            List<VMMC_Core.ReportItem> result = new List<VMMC_Core.ReportItem>();

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;
            //string connectionString = @"Server=server-db;Integrated security=SSPI;database=InfoModelVMMC";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string serchStr = "WHERE classes.ClassName = '" + className + "' ";
                if (className == "") serchStr = "";

                string sql = @"SELECT distinct
documents.[DocumentId]
,documents.[Code]
,documents.[Name]
,domainClasses.ClassName as DocType
,classes.Id as ClassId
,classes.ClassName

FROM [dbo].[Documents] documents
left join [dbo].[Classes] classes on classes.Id = documents.ClassId
left join [dbo].[Classes] domainClasses on domainClasses.Id = IIF((SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId])='47A89024-872A-EC11-A602-00155D03FA01', documents.[ClassId], (SELECT [ParentId] FROM [dbo].[Classes] WHERE [Id] = documents.[ClassId])) " + serchStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //VMMC_Core.Document newDocument = new VMMC_Core.Document(sessionInfo).GetDocument(dr["Code"].ToString());
                        VMMC_Core.ReportItem newReportItem = new VMMC_Core.ReportItem();
                        //newReportItem.Item = new DbObject(sessionInfo).GetObject(Guid.Parse(dr["DocumentId"].ToString()));
                        newReportItem.Item = new DbObject(sessionInfo);
                        newReportItem.Item.ObjectId = Guid.Parse(dr["DocumentId"].ToString());
                        newReportItem.Item.ObjectCode = dr["Code"].ToString();
                        newReportItem.Item.ObjectName = dr["Name"].ToString();
                        newReportItem.Item.ObjectClass = new Class(sessionInfo);
                        newReportItem.Item.ObjectClass.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        newReportItem.Item.ObjectClass.ClassName = dr["ClassName"].ToString();
                        result.Add(newReportItem);
                    }
                }
            }
            return result;
        }
    }
}
