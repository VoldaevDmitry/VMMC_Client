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
    public class TreeItem
    {
        VMMC_Core.SessionInfo sessionInfo;
        public Guid TreeItemId { get; set; }
        public VMMC_Core.DbObject Object { get; set; }
        public string TreeItemName { get; set; }
        public string TreeItemCode { get; set; }
        public string TreeItemDescription { get; set; }
        public DbObject TreeObject { get; set; }
        public VMMC_Core.Class Class { get; set; }
        public TreeItem Parent { get; set; }
        public string Status { get; set; }
        public string StatusInfo { get; set; }
        public bool IsExistInDB { get; set; }

        public TreeItem(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }

        public VMMC_Core.TreeItem getTreeItem(string treeItemCode, string parentId)
        {
            VMMC_Core.TreeItem treeItem = new VMMC_Core.TreeItem(sessionInfo);

            string searchStr = "WHERE ([TreeItemCode] = '" + treeItemCode + "' or [TreeItemDescription] = '" + treeItemCode + "') ";
            if (parentId != null) searchStr += "AND ([ParentId] = '" + parentId + "' OR [ClassId] = '" + parentId + "') ";

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [TreeItemName], [TreeItemCode], [TreeItemDescription], [ClassId], [ParentId] FROM [dbo].[TreeItems] " + searchStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        treeItem.TreeItemId = Guid.Parse(dr["Id"].ToString());
                        treeItem.TreeItemName = dr["TreeItemName"].ToString();
                        treeItem.TreeItemCode = dr["TreeItemCode"].ToString();
                        treeItem.TreeItemDescription = dr["TreeItemDescription"].ToString();
                        treeItem.Class = new Class(sessionInfo);
                        treeItem.Class.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        if (dr["ParentId"].ToString() != "") treeItem.Parent = new TreeItem(sessionInfo) { TreeItemId = Guid.Parse(dr["ParentId"].ToString()) };
                        treeItem.IsExistInDB = true;
                    }
                    return treeItem;
                }
                else return null;

            }
        }
        public VMMC_Core.TreeItem getTreeItem(Guid treeItemId)
        {
            VMMC_Core.TreeItem treeItem = new VMMC_Core.TreeItem(sessionInfo);

            string searchStr = "WHERE [Id] = '" + treeItemId.ToString() + "' ";

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [TreeItemName], [TreeItemCode], [TreeItemDescription], [ClassId], [ParentId] FROM [dbo].[TreeItems] " + searchStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        treeItem.TreeItemId = Guid.Parse(dr["Id"].ToString());
                        treeItem.TreeItemName = dr["TreeItemName"].ToString();
                        treeItem.TreeItemCode = dr["TreeItemCode"].ToString();
                        treeItem.TreeItemDescription = dr["TreeItemDescription"].ToString();
                        treeItem.Class = new Class(sessionInfo);
                        treeItem.Class.ClassId = Guid.Parse(dr["ClassId"].ToString());
                        if (dr["ParentId"].ToString() != "") treeItem.Parent = new TreeItem(sessionInfo) { TreeItemId = Guid.Parse(dr["ParentId"].ToString()) };
                        treeItem.IsExistInDB = true;
                    }
                    return treeItem;
                }
                else return null;

            }
        }
        public ObservableCollection<VMMC_Core.TreeItem> getTreeItemCildrens ()
        {
            ObservableCollection<VMMC_Core.TreeItem> treeItemList = new ObservableCollection<VMMC_Core.TreeItem>();

            string searchStr = "WHERE [ParentId] = '" + TreeItemId.ToString() + "' ";

            // строка подключения к БД
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [TreeItemName], [TreeItemCode], [TreeItemDescription], [ClassId], [ParentId] FROM [dbo].[TreeItems] " + searchStr;
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.Parse(dr["Id"].ToString()),
                            TreeItemName = dr["TreeItemName"].ToString(),
                            TreeItemCode = dr["TreeItemCode"].ToString(),
                            TreeItemDescription = dr["TreeItemDescription"].ToString(),
                            Class = new Class(sessionInfo) { ClassId = Guid.Parse(dr["ClassId"].ToString()) },
                            Parent = new TreeItem(sessionInfo) { TreeItemId = Guid.Parse(dr["ParentId"].ToString()) },
                            IsExistInDB = true
                        };
                        treeItemList.Add(newTreeItem);
                    }
                    return treeItemList;
                }
                else return null;

            }
        }
        public List<VMMC_Core.TreeItem> getDbTreeItemList()
        {
            List<VMMC_Core.TreeItem> treeItemList = new List<VMMC_Core.TreeItem>();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT [Id], [TreeItemName], [TreeItemCode], [TreeItemDescription], [ClassId], [ParentId] FROM [dbo].[TreeItems] ";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.Parse(dr["Id"].ToString()),
                            TreeItemName = dr["TreeItemName"].ToString(),
                            TreeItemCode = dr["TreeItemCode"].ToString(),
                            TreeItemDescription = dr["TreeItemDescription"].ToString(),
                            Class = new Class(sessionInfo) {ClassId = Guid.Parse(dr["ClassId"].ToString())},
                            Parent = new TreeItem(sessionInfo) { TreeItemId = Guid.Parse(dr["ParentId"].ToString()) },
                            IsExistInDB = true
                        };
                        treeItemList.Add(newTreeItem);
                    }
                }

                return treeItemList;
            }
        }
        public List<VMMC_Core.TreeItem> getDbBuildFolderTreeItemList()
        {
            List<VMMC_Core.TreeItem> treeItemList = new List<VMMC_Core.TreeItem>();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT 
    tab.TreeViewCode 
    ,IIF(tab.TypeDocument = 'РД', 
        CASE 
            WHEN TreeViewCode = '00' THEN 'Генеральный план' 
            WHEN TreeViewCode = '04' THEN 'Многопрофильный медицинский центр с посадочной площадкой на кровле' 
            WHEN TreeViewCode = '05' THEN 'Онкологический центр' 
            WHEN TreeViewCode = '08' THEN 'Подземная парковка' 
            WHEN TreeViewCode = '09' THEN 'Посадочная площадка' 
            WHEN TreeViewCode = '7.19' THEN 'Здания морга' 
            WHEN TreeViewCode = '7.20' THEN 'Здание по обращению с отходами' 
            WHEN TreeViewCode = '7.21' THEN 'Кухня и Аптечный склад' 
            WHEN TreeViewCode = '7.23' THEN 'Дизельная генераторная установка' 
            WHEN TreeViewCode = '7.24' THEN 'РТП-2' 
            WHEN TreeViewCode = '7.25' THEN 'Котельная' 
            WHEN TreeViewCode = '7.26' THEN 'Подстанция скорой помощи' 
            WHEN TreeViewCode = '7.27' THEN 'Кислородная станция' 
            WHEN TreeViewCode = '7.28' THEN 'Гараж' 
            WHEN TreeViewCode = '7.29' THEN 'Подземный склад' 
            WHEN TreeViewCode = '7.33' THEN 'Авиационный ангар' 
            WHEN TreeViewCode = '7.34' THEN 'Коммуникационный коллектор' 
            WHEN TreeViewCode = '7.36' THEN 'Подземная галерея'  
            WHEN TreeViewCode = '10.1' THEN 'КПП 1' 
            WHEN TreeViewCode = '10.2' THEN 'КПП 2' 
            WHEN TreeViewCode = '10.3' THEN 'КПП 3' 
            WHEN TreeViewCode = '10.4' THEN 'КПП 4' 
            WHEN TreeViewCode = '7.19_7.20_7.21' THEN 'Прифундаментный дренаж Кухня и аптечный скл' 
            WHEN TreeViewCode = 'ВОС' THEN 'Комплекс хозяйственно-питьевого водоснабжения (ВОС)' 
            WHEN TreeViewCode = 'КОС, ЛОС' THEN 'Комплекс очистных сооружений хозяйственно-бытовых (КОС) и поверхностных сточных вод (ЛОС)' 
            ELSE 'н/д' 
        END, 
        IIF(tab.TypeDocument = 'ИД', 'Том ' + TreeViewCode, 'н/д') 
    ) as TreeViewName 
FROM( 
    SELECT DISTINCT 
        CASE 
            WHEN Code like 'ВММК-РД-00-%' THEN '00' 
            WHEN Code like 'ВММК-РД-04-%' THEN '04' 
            WHEN Code like 'ВММК-РД-05-%' THEN '05' 
            WHEN Code like 'ВММК-РД-08-%' THEN '08' 
            WHEN Code like 'ВММК-РД-09-%' THEN '09' 
            WHEN Code like 'ВММК-РД-7.19-%' THEN '7.19' 
            WHEN Code like 'ВММК-РД-7.20-%' THEN '7.20' 
            WHEN Code like 'ВММК-РД-7.21-%' THEN '7.21' 
            WHEN Code like 'ВММК-РД-7.23-%' THEN '7.23' 
            WHEN Code like 'ВММК-РД-7.24-%' THEN '7.24' 
            WHEN Code like 'ВММК-РД-7.25-%' THEN '7.25' 
            WHEN Code like 'ВММК-РД-7.26-%' THEN '7.26' 
            WHEN Code like 'ВММК-РД-7.27-%' THEN '7.27' 
            WHEN Code like 'ВММК-РД-7.28-%' THEN '7.28' 
            WHEN Code like 'ВММК-РД-7.29-%' THEN '7.29' 
            WHEN Code like 'ВММК-РД-7.33-%' THEN '7.33' 
            WHEN Code like 'ВММК-РД-7.34-%' THEN '7.34' 
            WHEN Code like 'ВММК-РД-7.36-%' THEN '7.36' 
            WHEN Code like 'ВММК-РД-10.1-%' THEN '10.1' 
            WHEN Code like 'ВММК-РД-10.2-%' THEN '10.2' 
            WHEN Code like 'ВММК-РД-10.3-%' THEN '10.3' 
            WHEN Code like 'ВММК-РД-10.4-%' THEN '10.4' 
            WHEN Code like 'ВММК-РД-7.19_7.20_7.21-%' THEN '7.19_7.20_7.21' 
            WHEN Code like 'ВММК-Р%Д4-%' THEN 'ВОС' 
            WHEN Code like 'ВММК-РД-13-СС%' THEN 'ВОС' 
            WHEN Code like 'ВММК-Р%Д3-%' THEN 'КОС, ЛОС' 
            WHEN Code like 'ВММК-ИД-%' THEN SUBSTRING(Code, 9, (CHARINDEX('-', Code, 9) - 9)) 
            ELSE 'н/д' 
        END as TreeViewCode
        , IIF(Code like 'ВММК-РД-%' or Code like 'ВММК-Р%Д4-%' or Code like 'ВММК-Р%Д3-%', 'РД', IIF(Code like 'ВММК-ИД-%', 'ИД', NULL)) as TypeDocument 
    FROM[dbo].[Complekts] 
    where Code like 'ВММК-РД-%' or Code like 'ВММК-Р%Д%-%' 
) as tab 
order by tab.TreeViewCode";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.NewGuid(),
                            TreeItemName = dr["TreeViewName"].ToString(),
                            TreeItemCode = dr["TreeViewCode"].ToString(),
                            TreeItemDescription = "ВММК-РД-" + dr["TreeViewCode"].ToString()
                        };
                        treeItemList.Add(newTreeItem);
                    }
                }

                return treeItemList;
            }
        }
        public List<VMMC_Core.TreeItem> getDbTomFolderTreeItemList()
        {
            List<VMMC_Core.TreeItem> treeItemList = new List<VMMC_Core.TreeItem>();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();// устанавливаем соединение с БД
                string sql = @"SELECT 
    tab.TreeViewCode 
    ,IIF(tab.TypeDocument = 'РД', 
        CASE 
            WHEN TreeViewCode = '00' THEN 'Генеральный план' 
            WHEN TreeViewCode = '04' THEN 'Многопрофильный медицинский центр с посадочной площадкой на кровле' 
            WHEN TreeViewCode = '05' THEN 'Онкологический центр' 
            WHEN TreeViewCode = '08' THEN 'Подземная парковка' 
            WHEN TreeViewCode = '09' THEN 'Посадочная площадка' 
            WHEN TreeViewCode = '7.19' THEN 'Здания морга' 
            WHEN TreeViewCode = '7.20' THEN 'Здание по обращению с отходами' 
            WHEN TreeViewCode = '7.21' THEN 'Кухня и Аптечный склад' 
            WHEN TreeViewCode = '7.23' THEN 'Дизельная генераторная установка' 
            WHEN TreeViewCode = '7.24' THEN 'РТП-2' 
            WHEN TreeViewCode = '7.25' THEN 'Котельная' 
            WHEN TreeViewCode = '7.26' THEN 'Подстанция скорой помощи' 
            WHEN TreeViewCode = '7.27' THEN 'Кислородная станция' 
            WHEN TreeViewCode = '7.28' THEN 'Гараж' 
            WHEN TreeViewCode = '7.29' THEN 'Подземный склад' 
            WHEN TreeViewCode = '7.33' THEN 'Авиационный ангар' 
            WHEN TreeViewCode = '7.34' THEN 'Коммуникационный коллектор' 
            WHEN TreeViewCode = '7.36' THEN 'Подземная галерея'  
            WHEN TreeViewCode = '10.1' THEN 'КПП 1' 
            WHEN TreeViewCode = '10.2' THEN 'КПП 2' 
            WHEN TreeViewCode = '10.3' THEN 'КПП 3' 
            WHEN TreeViewCode = '10.4' THEN 'КПП 4' 
            WHEN TreeViewCode = '7.19_7.20_7.21' THEN 'Прифундаментный дренаж Кухня и аптечный скл' 
            WHEN TreeViewCode = 'ВОС' THEN 'Комплекс хозяйственно-питьевого водоснабжения (ВОС)' 
            WHEN TreeViewCode = 'КОС, ЛОС' THEN 'Комплекс очистных сооружений хозяйственно-бытовых (КОС) и поверхностных сточных вод (ЛОС)' 
            ELSE 'н/д' 
        END, 
        IIF(tab.TypeDocument = 'ИД', 'Том ' + TreeViewCode, 'н/д') 
    ) as TreeViewName 
FROM( 
    SELECT DISTINCT 
        CASE 
            WHEN Code like 'ВММК-РД-00-%' THEN '00' 
            WHEN Code like 'ВММК-РД-04-%' THEN '04' 
            WHEN Code like 'ВММК-РД-05-%' THEN '05' 
            WHEN Code like 'ВММК-РД-08-%' THEN '08' 
            WHEN Code like 'ВММК-РД-09-%' THEN '09' 
            WHEN Code like 'ВММК-РД-7.19-%' THEN '7.19' 
            WHEN Code like 'ВММК-РД-7.20-%' THEN '7.20' 
            WHEN Code like 'ВММК-РД-7.21-%' THEN '7.21' 
            WHEN Code like 'ВММК-РД-7.23-%' THEN '7.23' 
            WHEN Code like 'ВММК-РД-7.24-%' THEN '7.24' 
            WHEN Code like 'ВММК-РД-7.25-%' THEN '7.25' 
            WHEN Code like 'ВММК-РД-7.26-%' THEN '7.26' 
            WHEN Code like 'ВММК-РД-7.27-%' THEN '7.27' 
            WHEN Code like 'ВММК-РД-7.28-%' THEN '7.28' 
            WHEN Code like 'ВММК-РД-7.29-%' THEN '7.29' 
            WHEN Code like 'ВММК-РД-7.33-%' THEN '7.33' 
            WHEN Code like 'ВММК-РД-7.34-%' THEN '7.34' 
            WHEN Code like 'ВММК-РД-7.36-%' THEN '7.36' 
            WHEN Code like 'ВММК-РД-10.1-%' THEN '10.1' 
            WHEN Code like 'ВММК-РД-10.2-%' THEN '10.2' 
            WHEN Code like 'ВММК-РД-10.3-%' THEN '10.3' 
            WHEN Code like 'ВММК-РД-10.4-%' THEN '10.4' 
            WHEN Code like 'ВММК-РД-7.19_7.20_7.21-%' THEN '7.19_7.20_7.21' 
            WHEN Code like 'ВММК-Р%Д4-%' THEN 'ВОС' 
            WHEN Code like 'ВММК-РД-13-СС%' THEN 'ВОС' 
            WHEN Code like 'ВММК-Р%Д3-%' THEN 'КОС, ЛОС' 
            WHEN Code like 'ВММК-ИД-%' THEN SUBSTRING(Code, 9, (CHARINDEX('-', Code, 9) - 9)) 
            ELSE 'н/д' 
        END as TreeViewCode
        , IIF(Code like 'ВММК-РД-%' or Code like 'ВММК-Р%Д4-%' or Code like 'ВММК-Р%Д3-%', 'РД', IIF(Code like 'ВММК-ИД-%', 'ИД', NULL)) as TypeDocument 
    FROM[dbo].[Complekts] 
    where Code like 'ВММК-ИД-%'
) as tab 
order by tab.TreeViewCode";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.NewGuid(),
                            TreeItemName = dr["TreeViewName"].ToString(),
                            TreeItemCode = dr["TreeViewCode"].ToString(),
                            TreeItemDescription = "ВММК-ИД-" + dr["TreeViewCode"].ToString()
                        };
                        treeItemList.Add(newTreeItem);
                    }
                }

                return treeItemList;
            }
        }
        public ObservableCollection<VMMC_Core.TreeItem> getDbMarkFolderTreeItemList(string docType, string buildortom)
        {
            ObservableCollection<VMMC_Core.TreeItem> treeItemList = new ObservableCollection<VMMC_Core.TreeItem>();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string searchStr = "";
                if (buildortom == "ВОС")
                {
                    searchStr = "where Code like 'ВММК-Р%Д4-%' or  Code like 'ВММК-РД-13-%'";
                }
                else if (buildortom == "КОС, ЛОС")
                {
                    searchStr = "where Code like 'ВММК-Р%Д3-%' ";
                }
                //else searchStr = "where Code like 'ВММК-' + '" + TreeViewItemLevel1 + "' + '-' + '" + TreeViewItemLevel2 + "' + '-%' ";
                else searchStr = "where Code like 'ВММК-'+'-"+ docType + "-'+'"+ buildortom + "' + '-%' ";

                conn.Open();// устанавливаем соединение с БД
                string sql = @"select distinct 
tab.TreeViewCode 
,tab.ParentTreeViewCode
from
( 
SELECT 
	distinct Code
	, SUBSTRING(Code, CHARINDEX('-', Code, 11) + 1, IIF(CHARINDEX('-', Code, CHARINDEX('-', Code, 11) + 1) > 0, CHARINDEX('-', Code, CHARINDEX('-', Code, 11) + 1) - CHARINDEX('-', Code, 11) - 1, LEN(Code) - CHARINDEX('-', Code, 11))) as TreeViewCode 
	, IIF(Code like 'ВММК-РД-%', 'РД', IIF(Code like 'ВММК-ИД-%', 'ИД', NULL)) as TypeDocument 
	,CASE
	WHEN Code like 'ВММК-РД-00-%' THEN 'ВММК-РД-00-'
	WHEN Code like 'ВММК-РД-04-%' THEN 'ВММК-РД-04-'
	WHEN Code like 'ВММК-РД-05-%' THEN 'ВММК-РД-05-'
	WHEN Code like 'ВММК-РД-08-%' THEN 'ВММК-РД-08-'
	WHEN Code like 'ВММК-РД-09-%' THEN 'ВММК-РД-09-'
	WHEN Code like 'ВММК-РД-7.19-%' THEN 'ВММК-РД-7.19-'
	WHEN Code like 'ВММК-РД-7.20-%' THEN 'ВММК-РД-7.20-'
	WHEN Code like 'ВММК-РД-7.21-%' THEN 'ВММК-РД-7.21-'
	WHEN Code like 'ВММК-РД-7.23-%' THEN 'ВММК-РД-7.23'
	WHEN Code like 'ВММК-РД-7.24-%' THEN 'ВММК-РД-7.24'
	WHEN Code like 'ВММК-РД-7.25-%' THEN 'ВММК-РД-7.25'
	WHEN Code like 'ВММК-РД-7.26-%' THEN 'ВММК-РД-7.26'
	WHEN Code like 'ВММК-РД-7.27-%' THEN 'ВММК-РД-7.27'
	WHEN Code like 'ВММК-РД-7.28-%' THEN 'ВММК-РД-7.28' 
	WHEN Code like 'ВММК-РД-7.29-%' THEN 'ВММК-РД-7.29' 
	WHEN Code like 'ВММК-РД-7.33-%' THEN 'ВММК-РД-7.33'
	WHEN Code like 'ВММК-РД-7.34-%' THEN 'ВММК-РД-7.34' 
	WHEN Code like 'ВММК-РД-7.36-%' THEN 'ВММК-РД-7.36'
	WHEN Code like 'ВММК-РД-10.1-%' THEN 'ВММК-РД-10.1'
	WHEN Code like 'ВММК-РД-10.2-%' THEN 'ВММК-РД-10.2'
	WHEN Code like 'ВММК-РД-10.3-%' THEN 'ВММК-РД-10.3'
	WHEN Code like 'ВММК-РД-10.4-%' THEN 'ВММК-РД-10.4'
	WHEN Code like 'ВММК-РД-7.19_7.20_7.21-%' THEN 'ВММК-РД-7.19_7.20_7.21' 
	WHEN Code like 'ВММК-РД4-%' THEN 'ВММК-РД4-'
	WHEN Code like 'ВММК-РКД4-%' THEN 'ВММК-РКД4-'
	WHEN Code like 'ВММК-РД-13-СС%' THEN 'ВММК-РД-13-СС'
	WHEN Code like 'ВММК-РД3-%' THEN 'ВММК-РД3-' 
	WHEN Code like 'ВММК-РКД3-%' THEN 'ВММК-РКД3-' 
	WHEN Code like 'ВММК-ИД-%' THEN 'ВММК-ИД-'+SUBSTRING(Code, 9, (CHARINDEX('-', Code, 9) - 9))+'-'
    ELSE 'н/д'
	END as ParentTreeViewCode

FROM[dbo].[Complekts] 
" + searchStr + ") as tab";
                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.NewGuid(),
                            //TreeItemName = dr["TreeViewName"].ToString(),
                            TreeItemCode = dr["TreeViewCode"].ToString(),
                            TreeItemDescription = dr["ParentTreeViewCode"].ToString() + dr["TreeViewCode"].ToString()
                        };
                        treeItemList.Add(newTreeItem);
                    }
                }

                return treeItemList;
            }
        }
        public ObservableCollection<VMMC_Core.TreeItem> getDbFolderTreeItemList(string docType, string tomCode, string markCode)
        {
            ObservableCollection<VMMC_Core.TreeItem> treeItemList = new ObservableCollection<VMMC_Core.TreeItem>();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string selectStr = "";
                string whereStr = "";
                string orderStr = "";

                if (docType != "")
                {
                    if (tomCode != "")
                    {
                        if (markCode != "")
                        {
                            selectStr = "select distinct tab.TypeDocument, tab.TomCode, tab.MarkCode, tab.Code as TreeViewCode ";
                            whereStr = "where tab.TypeDocument = '" + docType + "' and tab.TomCode = '" + tomCode + "' and tab.MarkCode = '" + markCode + "' ";
                            orderStr = "order by tab.TypeDocument, tab.TomCode, tab.MarkCode, tab.Code ";
                        }
                        else
                        {
                            selectStr = "select distinct tab.TypeDocument, tab.TomCode, tab.MarkCode as TreeViewCode ";
                            whereStr = "where tab.TypeDocument = '" + docType + "' and tab.TomCode = '" + tomCode + "' ";
                            orderStr = "order by tab.TypeDocument, tab.TomCode, tab.MarkCode ";
                        }
                    }
                    else
                    {
                        selectStr = "select distinct tab.TypeDocument, tab.TomCode as TreeViewCode ";
                        whereStr = "where tab.TypeDocument = '" + docType + "' ";
                        orderStr = "order by tab.TypeDocument, tab.TomCode ";
                    }
                }
                else
                {
                    selectStr = "select distinct tab.TypeDocument as TreeViewCode ";
                    orderStr = "order by tab.TypeDocument ";
                }

               

                conn.Open();// устанавливаем соединение с БД
                string sql = selectStr+@"from
( 
SELECT 
	distinct Code
	, SUBSTRING(Code, CHARINDEX('-', Code, 11) + 1, IIF(CHARINDEX('-', Code, CHARINDEX('-', Code, 11) + 1) > 0, CHARINDEX('-', Code, CHARINDEX('-', Code, 11) + 1) - CHARINDEX('-', Code, 11) - 1, LEN(Code) - CHARINDEX('-', Code, 11))) as MarkCode 
	, IIF(Code like 'ВММК-Р%Д%-%', 'РД', IIF(Code like 'ВММК-ИД-%', 'ИД', NULL)) as TypeDocument 
	,CASE
	WHEN Code like 'ВММК-РД-00-%' THEN '00'
	WHEN Code like 'ВММК-РД-04-%' THEN '04'
	WHEN Code like 'ВММК-РД-05-%' THEN '05'
	WHEN Code like 'ВММК-РД-08-%' THEN '08'
	WHEN Code like 'ВММК-РД-09-%' THEN '09'
	WHEN Code like 'ВММК-РД-7.19-%' THEN '7.19'
	WHEN Code like 'ВММК-РД-7.20-%' THEN '7.20'
	WHEN Code like 'ВММК-РД-7.21-%' THEN '7.21'
	WHEN Code like 'ВММК-РД-7.23-%' THEN '7.23'
	WHEN Code like 'ВММК-РД-7.24-%' THEN '7.24'
	WHEN Code like 'ВММК-РД-7.25-%' THEN '7.25'
	WHEN Code like 'ВММК-РД-7.26-%' THEN '7.26'
	WHEN Code like 'ВММК-РД-7.27-%' THEN '7.27'
	WHEN Code like 'ВММК-РД-7.28-%' THEN '7.28' 
	WHEN Code like 'ВММК-РД-7.29-%' THEN '7.29' 
	WHEN Code like 'ВММК-РД-7.33-%' THEN '7.33'
	WHEN Code like 'ВММК-РД-7.34-%' THEN '7.34' 
	WHEN Code like 'ВММК-РД-7.36-%' THEN '7.36'
	WHEN Code like 'ВММК-РД-10.1-%' THEN '10.1'
	WHEN Code like 'ВММК-РД-10.2-%' THEN '10.2'
	WHEN Code like 'ВММК-РД-10.3-%' THEN '10.3'
	WHEN Code like 'ВММК-РД-10.4-%' THEN '10.4'
	WHEN Code like 'ВММК-РД-7.19_7.20_7.21-%' THEN '7.19_7.20_7.21' 
	WHEN Code like 'ВММК-Р%Д4-%' THEN 'ВОС'
	WHEN Code like 'ВММК-РД-13-СС%' THEN 'ВОС'
	WHEN Code like 'ВММК-Р%Д3-%' THEN 'КОС, ЛОС' 
	WHEN Code like 'ВММК-ИД-%' THEN SUBSTRING(Code, 9, (CHARINDEX('-', Code, 9) - 9))
    ELSE 'н/д'
	END as TomCode
FROM[dbo].[Complekts] 
) as tab " + whereStr + orderStr;

                // Создать объект Command.
                SqlCommand cmd = new SqlCommand(sql, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.NewGuid(),
                            //TreeItemName = dr["TreeViewName"].ToString(),
                            TreeItemCode = dr["TreeViewCode"].ToString(),
                            //TreeItemDescription = dr["ParentTreeViewCode"].ToString() + dr["TreeViewCode"].ToString()
                        };
                        treeItemList.Add(newTreeItem);
                    }
                }

                return treeItemList;
            }
        }
        public ObservableCollection<VMMC_Core.TreeItem> getDbTreeItemListFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.TreeItem> treeItemList = new ObservableCollection<VMMC_Core.TreeItem>();

            // строка подключения к БД
            //string connectionString = @"Server=" + SQLServer + ";Integrated security=SSPI;database=" + SQLDataBase;
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
                        VMMC_Core.TreeItem newTreeItem = new VMMC_Core.TreeItem(sessionInfo)
                        {
                            TreeItemId = Guid.Parse(dr["Id"].ToString()),
                            TreeItemName = dr["TreeItemName"].ToString(),
                            TreeItemCode = dr["TreeItemCode"].ToString(),
                            Class = new VMMC_Core.Class(sessionInfo) {  ClassId = Guid.Parse(dr["ClassId"].ToString()) },
                            Parent = new VMMC_Core.TreeItem(sessionInfo) { TreeItemId = Guid.Parse(dr["ParentId"].ToString()) }
                            //TreeItemDescription = dr["ParentTreeViewCode"].ToString() + dr["TreeViewCode"].ToString()
                        };
                        treeItemList.Add(newTreeItem);
                    }
                }

                return treeItemList;
            }
        }
        public string CreateDBTreeItem()
        {
            int systemTypeId = 1; // для дерева всегда 1
            Guid projectId = Guid.Parse("FCE6ABA9-54A7-EB11-A1A0-00155D036519");//ВММК
            projectId = sessionInfo.ProjectId;

            string connectionString = @"Server=" + sessionInfo.ServerName + ";Integrated security=SSPI;database=" + sessionInfo.DataBaseName;

            string logString = "";
            string innerException = "";
            string stackTrace = "";
            string errorType = "";

            try
            {
                VMMC_Core.TreeItem existTreeItem = getTreeItem(TreeItemCode, Parent.TreeItemId.ToString());
                if (existTreeItem == null)
                {
                    string createDbObjectResult = new VMMC_Core.DbObject(sessionInfo).CreateDbObject(TreeItemId, Class.ClassId, systemTypeId, projectId);



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "SELECT * FROM [" + sessionInfo.DataBaseName + "].[dbo].[TreeItems]";
                        string colNames = "[Id]";
                        string colValues = "@TreeItemId";
                        if (TreeItemName != null)
                        {
                            colNames += ", [TreeItemName]";
                            colValues += ", @TreeItemName";
                        }
                        if (TreeItemCode != null)
                        {
                            colNames += ", [TreeItemCode]";
                            colValues += ", @TreeItemCode";
                        }
                        if (TreeItemDescription != null)
                        {
                            colNames += ", [TreeItemDescription]";
                            colValues += ", @TreeItemDescription";
                        }
                        if (Class.ClassId != null)
                        {
                            colNames += ", [ClassId]";
                            colValues += ", @ClassId";
                        }
                        if (Parent.TreeItemId != null)
                        {
                            colNames += ", [ParentId]";
                            colValues += ", @ParentId";
                        }
                       
                        string insertsql = "INSERT INTO [" + sessionInfo.DataBaseName + "].[dbo].[TreeItems] ( "+ colNames + " ) VALUES ( "+ colValues + " )";

                        SqlDataAdapter adapter = new SqlDataAdapter(sql, connection);
                        SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);

                        // Create the InsertCommand.
                        SqlCommand commandToIsert = new SqlCommand(insertsql, connection);

                        // Add the parameters for the InsertCommand.
                        if (TreeItemId != null) commandToIsert.Parameters.Add(new SqlParameter("@TreeItemId", SqlDbType.UniqueIdentifier)).Value = TreeItemId;
                        if (TreeItemName != null) commandToIsert.Parameters.Add(new SqlParameter("@TreeItemName", SqlDbType.NVarChar)).Value = TreeItemName;
                        if (TreeItemCode != null) commandToIsert.Parameters.Add(new SqlParameter("@TreeItemCode", SqlDbType.NVarChar)).Value = TreeItemCode;
                        if (TreeItemDescription != null) commandToIsert.Parameters.Add(new SqlParameter("@TreeItemDescription", SqlDbType.NVarChar)).Value = TreeItemDescription;
                        if (Class.ClassId != null) commandToIsert.Parameters.Add(new SqlParameter("@ClassId", SqlDbType.UniqueIdentifier)).Value = Class.ClassId;
                        if (Parent.TreeItemId != null) commandToIsert.Parameters.Add(new SqlParameter("@ParentId", SqlDbType.UniqueIdentifier)).Value = Parent.TreeItemId;

                        adapter.InsertCommand = commandToIsert;
                        commandToIsert.ExecuteNonQuery();
                        logString = "Пользователь " + sessionInfo.UserName + " добавил новую запись в таблицу TreeItems. Guid записи: [" + TreeItemId.ToString() + "]";
                        Status = "Ok";
                        StatusInfo = logString;
                    }

                }
                else
                {
                    logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу TreeItems, произошла ошибка. Документ с таким же кодом существует в БД";
                    Status = "Error";
                    StatusInfo = logString;
                }
            }
            catch (Exception e)
            {
                logString = "При добавлении новой записи пользователем " + sessionInfo.UserName + " в таблицу TreeItems, произошла ошибка. " + e.Message;
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
