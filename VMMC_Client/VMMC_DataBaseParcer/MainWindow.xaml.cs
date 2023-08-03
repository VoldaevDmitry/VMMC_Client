using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VMMC_DataBaseParcer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public MainWindow(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
            if (sessionInfo == null) this.Close();
            else
            {
                InitializeComponent();
                DataContext = new VMMC_DataBaseParcer.DataBaseParcerViewModel(sessionInfo);
            }
        }
        public MainWindow()
        {
            sessionInfo = new VMMC_Core.SessionInfo();
            VMMC_Core.CommonControls.LoginForm loginForm = new VMMC_Core.CommonControls.LoginForm();
            loginForm.ShowDialog();
            sessionInfo = loginForm.sessionInfo;
            if (sessionInfo == null) this.Close();
            else
            {
                InitializeComponent();
                DataContext = new VMMC_DataBaseParcer.DataBaseParcerViewModel(sessionInfo);
            }
        }

        private void TagRdRels_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataBaseParcerViewModel dataBaseParcerViewModel = new DataBaseParcerViewModel(sessionInfo);
            //dataBaseParcerViewModel.getDbTagRdRelationships();

            Relationships_Query.Text = @"SELECT 
NEWID() as [Id]
,'EC83F27D-1907-EC11-A602-00155D03FA01' as [RelTypeId]
,tags.[Id] as [LeftObjectId]
,documentsRD.[DocumentId] as [RightObjectId]
,IIF(''='', NULL, '') as [RoleId]

FROM [dbo].[Documents] documentsRD
left join [dbo].[Relationships] reldocrd on reldocrd.RightObjectId = documentsRD.DocumentId and reldocrd.RelTypeId = 'A27B1D5C-0A5D-4AD8-A2F0-E52E7E40E67A'
left join [dbo].[Documents] documents3D on reldocrd.LeftObjectId = documents3D.DocumentId
left join [dbo].[Relationships] reldoc3d on reldoc3d.RightObjectId = documents3D.DocumentId and reldoc3d.RelTypeId = '8B617564-F353-4655-B21F-6171504C4274'
left join [dbo].[Tags] tags on reldoc3d.LeftObjectId = tags.Id
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' and tags.[Id] is not null";

            DataContext = dataBaseParcerViewModel;

        }
        private void TreeRdRels_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataBaseParcerViewModel dataBaseParcerViewModel = new DataBaseParcerViewModel(sessionInfo);

            //dataBaseParcerViewModel.getDbTreeRdRelationships();

            Relationships_Query.Text = @"SELECT NEWID() as [Id]
,'70ED50E7-AD56-ED11-A60B-00155D03FA01' as [RelTypeId]
,treeItems.Id as [LeftObjectId]
,documentsRD.[DocumentId] as [RightObjectId]
,IIF(''='', NULL, '') as [RoleId]
FROM [dbo].[Documents] documentsRD
left join [dbo].[Relationships] reldocrd on reldocrd.RightObjectId = documentsRD.DocumentId and reldocrd.RelTypeId = 'A27B1D5C-0A5D-4AD8-A2F0-E52E7E40E67A'
left join [dbo].[Documents] documents3D on reldocrd.LeftObjectId = documents3D.DocumentId
left join [dbo].[Relationships] reldoc3d on reldoc3d.RightObjectId = documents3D.DocumentId and reldoc3d.RelTypeId = '70ED50E7-AD56-ED11-A60B-00155D03FA01'
left join [dbo].[TreeItems] treeItems on reldoc3d.LeftObjectId = treeItems.Id
where documentsRD.[ClassId] = '715574FF-30E5-4911-80E1-44CCB68212A6' and treeItems.Id is not null";

            DataContext = dataBaseParcerViewModel;

        }
        private void TreeIdComplektsRels_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataBaseParcerViewModel dataBaseParcerViewModel = new DataBaseParcerViewModel(sessionInfo);

            //dataBaseParcerViewModel.getDbTreeRdRelationships();

            Relationships_Query.Text = @"SELECT NEWID() as [Id]
      ,'749F630D-08BB-EC11-A608-00155D03FA01' as [RelTypeId] /*Элемент дерева-Комплект*/
      ,ti.Id as [LeftObjectId]
	  ,ti.TreeItemName
      ,c.ComplektId as [RightObjectId]
	  ,c.Code
      ,NULL as [RoleId]
  FROM [dbo].[TreeItems] ti
  left join [InfoModelVMMK].[dbo].[Complekts] c on c.Code like 'ВММК-ИД-'+ti.TreeItemCode+'-%'
  where  [ParentId] = '5B501ACB-F721-4A17-8A3F-23D8B5DDABA2' and (SELECT [Id] FROM [dbo].[Relationships] WHERE [LeftObjectId] in (ti.Id) and [RightObjectId] in (c.ComplektId)) is null 
  order by [TreeItemCode]";

            DataContext = dataBaseParcerViewModel;

        }
        private void IdTree_MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DataBaseParcerViewModel dataBaseParcerViewModel = new DataBaseParcerViewModel(sessionInfo);

            //dataBaseParcerViewModel.getDbIdTree();

            TreeItems_Query.Text = @"SELECT NEWID() as [Id]
,t.TreeViewName as [TreeItemName]
,t.TreeViewCode as [TreeItemCode]
,NULL as [TreeItemDescription]
,'2BA620C0-2853-4423-BBB5-85A680C145BD' as [ClassId]
,'5B501ACB-F721-4A17-8A3F-23D8B5DDABA2' as [ParentId]
FROM (SELECT 
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
    WHERE Code like 'ВММК-' + 'ИД' + '%-%' 
                    ) as tab ) t

                    where t.TreeViewName not in (SELECT [TreeItemName] FROM [dbo].[TreeItems] where [ParentId] = '5B501ACB-F721-4A17-8A3F-23D8B5DDABA2')";


            DataContext = dataBaseParcerViewModel;

        }


        private static string Lats_Char_Found(string position)
        {
            string result = "";
            for (int c = 0; c < position.Length; c++)
            {
                if ((position[c] >= 'А' && position[c] <= 'Я') || (position[c] >= 'а' && position[c] <= 'я') || position[c] == 'Ё' || position[c] == 'ё') //нахождение русских символов А-Я, а-я и отдельно для Ё-ё.
                {
                    result = result + position[c] + ","; // вывод русских символов через запятую
                }
            }
            if (result != "")
            {
                result = result.Substring(0, result.Length - 1); // убрать последний символ (в данном случае запятую). Пример А,Я
            }
            return result;
        }
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
            //VMMC_Import.MainWindow importMainWindow = new VMMC_Import.MainWindow(sessionInfo);
        }
        private void documetnsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VMMC_Core.Document selectedItem = (VMMC_Core.Document)Documents_DataGrid.SelectedItem;
            if (selectedItem != null)
            {
                VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(selectedItem, selectedItem.Revisions[0], null, !selectedItem.IsExistInDB, sessionInfo);
                DocumentViewControl.DataContext = DocumentViewControl_DataContext;
            }
        }

        private void ExecuteQuery_Button_Click(object sender, RoutedEventArgs e)
        {
            DataBaseParcerViewModel dataBaseParcerViewModel = new DataBaseParcerViewModel(sessionInfo);

            if (Complekts_Query.Text != "") dataBaseParcerViewModel.getComplektsFromQuery(Complekts_Query.Text);
            if (Documents_Query.Text != "") dataBaseParcerViewModel.getRelationshipsFromQuery(Documents_Query.Text);
            if (Tags_Query.Text != "") dataBaseParcerViewModel.getRelationshipsFromQuery(Tags_Query.Text);
            if (Relationships_Query.Text != "") dataBaseParcerViewModel.getRelationshipsFromQuery(Relationships_Query.Text);
            if (TreeItems_Query.Text != "") dataBaseParcerViewModel.getTreeItemsFromQuery(TreeItems_Query.Text);
            if (Attributes_Query.Text != "") dataBaseParcerViewModel.getAttributesFromQuery(Attributes_Query.Text);
            

            DataContext = dataBaseParcerViewModel;

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
