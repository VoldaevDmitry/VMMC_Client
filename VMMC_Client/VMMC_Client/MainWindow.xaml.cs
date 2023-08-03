using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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


using System.ComponentModel;
using System.Data;
//using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using VMMC_Editor;
using System.Collections.ObjectModel;

namespace VMMC_Client
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
                InitializeTreeView();
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
                InitializeTreeView();
            }
        }
        private void InitializeTreeView() 
        {
            treeview1.Items.Clear();

            TreeViewItem docRDTVI = new TreeViewItem();
            docRDTVI.Header = "Рабочая документация";
            docRDTVI.Items.Add("пусто");
            TreeViewItem docIDTVI = new TreeViewItem();
            docIDTVI.Header = "Исполнительная документация";
            docIDTVI.Items.Add("пусто");
            TreeViewItem doc3DTVI = new TreeViewItem();
            doc3DTVI.Header = "3D-модели";
            doc3DTVI.Items.Add("пусто");
            treeview1.Items.Add(docRDTVI);
            treeview1.Items.Add(docIDTVI);
            treeview1.Items.Add(doc3DTVI);
            
        }
        private void TreeViewItem_Lvl0_Expanded(object sender, RoutedEventArgs e)
        { 
            //TreeViewItem tvItem = (TreeViewItem)sender;
            //MessageBox.Show("Lvl0_Узел " + tvItem.Header.ToString() + " раскрыт");
            TreeViewItem tvItem;
            TreeViewItem tvItemParent;
            TreeViewItem tvItemParentParent;
            TreeViewItem lvl0_TreeViewItem;
            TreeViewItem lvl1_TreeViewItem = new TreeViewItem();
            TreeViewItem lvl2_TreeViewItem = new TreeViewItem();
            tvItem = (TreeViewItem)sender;
            if (tvItem.Parent != null & tvItem.Parent.GetType() == typeof(TreeViewItem))
            {
                tvItemParent = (TreeViewItem)tvItem.Parent;
                if (tvItemParent.Parent != null & tvItemParent.Parent.GetType() == typeof(TreeViewItem))
                { 
                    tvItemParentParent = (TreeViewItem)tvItemParent.Parent;

                    lvl0_TreeViewItem = tvItemParentParent;
                    lvl1_TreeViewItem = tvItemParent;
                    lvl2_TreeViewItem = tvItem;

                }
                else
                {
                    lvl0_TreeViewItem = tvItemParent;
                    lvl1_TreeViewItem = tvItem;
                }
            }
            else lvl0_TreeViewItem = tvItem;

            MessageBox.Show(treeview1.Items[0].ToString());

            string MessageBoxText = "Узел " + tvItem.Header.ToString() + " раскрыт";
            MessageBoxText += "\nlvl0: " + lvl0_TreeViewItem.Header.ToString();
            if (lvl1_TreeViewItem.Header != null) MessageBoxText += "\nlvl1: " + lvl1_TreeViewItem.Header.ToString();
            if (lvl2_TreeViewItem.Header != null) MessageBoxText += "\nlvl2: " + lvl2_TreeViewItem.Header.ToString();
            MessageBox.Show(MessageBoxText);
        }

        private void TreeViewItem_Lvl0_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem tvItem = (TreeViewItem)sender;
            MessageBox.Show("Выбран Lvl0_узел: " + tvItem.Header.ToString());
        }

        private void treeViewItem_Expanded(object sender, RoutedEventArgs e) 
        {
            TreeViewItem expandedItem = (TreeViewItem)e.OriginalSource;
            expandedItem.Items.Clear();
            if (expandedItem.Tag == null)
            {
                //List<VMMC_Core.TreeItem> tomFoldersTreeItemList = new List<VMMC_Core.TreeItem>();
                ////if (treeViewItemParent1.Header.ToString() == "Исполнительная документация") markFoldersTreeItemList = new VMMC_Core.TreeItem(SessionInfo).getDbMarkFolderTreeItemList("ИД", expandedItemTag.TreeItemCode.ToString());
                ////else markFoldersTreeItemList = new VMMC_Core.TreeItem(SessionInfo).getDbMarkFolderTreeItemList("РД", expandedItemTag.TreeItemCode.ToString());
                //if (expandedItem.Header.ToString() == "Исполнительная документация") tomFoldersTreeItemList = new VMMC_Core.TreeItem(SessionInfo).getDbFolderTreeItemList("ИД", "", "");
                //else tomFoldersTreeItemList = new VMMC_Core.TreeItem(SessionInfo).getDbFolderTreeItemList("РД", "", "");

                //if (tomFoldersTreeItemList != null)
                //{
                //    foreach (VMMC_Core.TreeItem tomFoldersTreeItem in tomFoldersTreeItemList)
                //    {
                //        TreeViewItem newItem = new TreeViewItem();
                //        if (expandedItem.Header.ToString() == "Исполнительная документация") newItem.Header = tomFoldersTreeItem.TreeItemName;
                //        else newItem.Header = tomFoldersTreeItem.TreeItemCode + " - " + tomFoldersTreeItem.TreeItemName;
                //        //newItem.Header = tomFoldersTreeItem.TreeItemCode;
                //        newItem.Items.Add("*");
                //        newItem.Tag = tomFoldersTreeItem;
                //        expandedItem.Items.Add(newItem);
                //    }

                //}
                //else expandedItem.Items.Add("(пусто)");

                if (expandedItem.Header.ToString() == "Рабочая документация")
                {
                    List<VMMC_Core.TreeItem> buildFoldersTreeItemList = new VMMC_Core.TreeItem(sessionInfo).getDbBuildFolderTreeItemList();

                    if (buildFoldersTreeItemList != null)
                    {
                        foreach (VMMC_Core.TreeItem buildFoldersTreeItem in buildFoldersTreeItemList)
                        {
                            TreeViewItem newItem = new TreeViewItem();
                            newItem.Header = buildFoldersTreeItem.TreeItemCode + " - " + buildFoldersTreeItem.TreeItemName;
                            newItem.Items.Add("*");
                            newItem.Tag = buildFoldersTreeItem;
                            //newItem.Tag = buildFoldersTreeItem.TreeItemDescription;
                            expandedItem.Items.Add(newItem);
                        }

                    }
                    else expandedItem.Items.Add("(пусто)");
                }
                else if (expandedItem.Header.ToString() == "Исполнительная документация")
                {

                    List<VMMC_Core.TreeItem> buildFoldersTreeItemList = new VMMC_Core.TreeItem(sessionInfo).getDbTomFolderTreeItemList();

                    if (buildFoldersTreeItemList != null)
                    {
                        foreach (VMMC_Core.TreeItem tomFoldersTreeItem in buildFoldersTreeItemList)
                        {
                            TreeViewItem newItem = new TreeViewItem();
                            newItem.Header = tomFoldersTreeItem.TreeItemName;
                            newItem.Items.Add("*");
                            newItem.Tag = tomFoldersTreeItem;
                            expandedItem.Items.Add(newItem);
                        }

                    }
                    else expandedItem.Items.Add("(пусто)");
                }
                else if (expandedItem.Header.ToString() == "3D-модели")
                {
                    ObservableCollection<VMMC_Core.Document> dbDocuments = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
                    Guid classId = new VMMC_Core.Class(sessionInfo).getClass("3D-Модель").ClassId;
                    IEnumerable<VMMC_Core.Document> targetDocuments = dbDocuments.Where(x => x.DocumentClassId == classId);

                    if (targetDocuments != null)
                    {
                        foreach (VMMC_Core.Document document in targetDocuments)
                        {
                            document.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(document.DocumentId);
                            TreeViewItem newItem = new TreeViewItem();
                            newItem.Header = document.DocumentCode;
                            newItem.Items.Add("*");
                            newItem.Tag = document;
                            expandedItem.Items.Add(newItem);
                        }

                    }
                    else expandedItem.Items.Add("(пусто)");
                }
                else if (expandedItem.Header.ToString() == "1")
                {
                    ObservableCollection<VMMC_Core.Complekt> dbComplects = new VMMC_Core.Complekt(sessionInfo).GetDbComplektsList();
                    IEnumerable<VMMC_Core.Complekt> targetComplects = dbComplects.Where(x => x.ComplektCode.Contains("ВММК-РД-") || x.ComplektCode.Contains("ВММК-РД4-") || x.ComplektCode.Contains("ВММК-РД3-"));

                    if (targetComplects != null)
                    {
                        foreach (VMMC_Core.Complekt complekt in targetComplects)
                        {
                            TreeViewItem newItem = new TreeViewItem();
                            newItem.Header = complekt.ComplektCode;
                            newItem.Items.Add("*");
                            newItem.Tag = complekt;
                            expandedItem.Items.Add(newItem);
                        }

                    }
                    else expandedItem.Items.Add("(пусто)");
                }
                else if (expandedItem.Header.ToString() == "2")
                {
                    ObservableCollection<VMMC_Core.Complekt> dbComplects = new VMMC_Core.Complekt(sessionInfo).GetDbComplektsList();
                    IEnumerable<VMMC_Core.Complekt> targetComplects = dbComplects.Where(x => x.ComplektCode.Contains("ВММК-ИД-"));

                    if (targetComplects != null)
                    {
                        foreach (VMMC_Core.Complekt complekt in targetComplects)
                        {
                            TreeViewItem newItem = new TreeViewItem();
                            newItem.Header = complekt.ComplektCode;
                            newItem.Items.Add("*");
                            newItem.Tag = complekt;
                            expandedItem.Items.Add(newItem);
                        }

                    }
                    else expandedItem.Items.Add("(пусто)");
                }
                else expandedItem.Items.Add("(пусто)");
            }
            else 
            {
                if (expandedItem.Tag.GetType().Name == "Complekt")
                {
                    //var type = expandedItem.Tag.GetType().Name == "Complekt";
                    VMMC_Core.Complekt expandedComplect = (VMMC_Core.Complekt)expandedItem.Tag;
                    ObservableCollection<VMMC_Core.Relationship> dbRelationships = new VMMC_Core.Relationship(sessionInfo).GetDbRelationshipsList();
                    ObservableCollection<VMMC_Core.Document> dbDocuments = new VMMC_Core.Document(sessionInfo).GetDbDocumentsList();
                    IEnumerable<VMMC_Core.Relationship> targetRelationships = dbRelationships.Where(x => x.LeftObjectId == expandedComplect.ComplektId && x.RelTypeId == new VMMC_Core.Relationship(sessionInfo).GetRelationshipTypeId("Комплект-Документ"));

                    foreach (VMMC_Core.Relationship relationship in targetRelationships)
                    {
                        VMMC_Core.Document targetDocument = dbDocuments.Where(x => x.DocumentId == relationship.RightObjectId).FirstOrDefault();
                        targetDocument.Revisions = new VMMC_Core.Revision(sessionInfo).GetDbDocumentRevisionsList(targetDocument.DocumentId);
                        TreeViewItem newItem = new TreeViewItem();
                        newItem.Header = targetDocument.DocumentCode;
                        newItem.Items.Add("*");
                        newItem.Tag = targetDocument;
                        expandedItem.Items.Add(newItem);
                    }

                }
                else if (expandedItem.Tag.GetType().Name == "TreeItem")
                {
                    VMMC_Core.TreeItem expandedItemTag = (VMMC_Core.TreeItem)expandedItem.Tag;
                    TreeViewItem treeViewItemParent1 = (TreeViewItem)expandedItem.Parent; 
                    if (treeViewItemParent1.Tag == null)
                    {
                        ObservableCollection<VMMC_Core.TreeItem> markFoldersTreeItemList = new ObservableCollection<VMMC_Core.TreeItem>();
                        //if (treeViewItemParent1.Header.ToString() == "Исполнительная документация") markFoldersTreeItemList = new VMMC_Core.TreeItem(SessionInfo).getDbMarkFolderTreeItemList("ИД", expandedItemTag.TreeItemCode.ToString());
                        //else markFoldersTreeItemList = new VMMC_Core.TreeItem(SessionInfo).getDbMarkFolderTreeItemList("РД", expandedItemTag.TreeItemCode.ToString());
                        if (treeViewItemParent1.Header.ToString() == "Исполнительная документация") markFoldersTreeItemList = new VMMC_Core.TreeItem(sessionInfo).getDbFolderTreeItemList("ИД", expandedItemTag.TreeItemCode.ToString(),"");
                        else markFoldersTreeItemList = new VMMC_Core.TreeItem(sessionInfo).getDbFolderTreeItemList("РД", expandedItemTag.TreeItemCode.ToString(), "");

                        if (markFoldersTreeItemList != null)
                        {
                            foreach (VMMC_Core.TreeItem markFoldersTreeItem in markFoldersTreeItemList)
                            {
                                TreeViewItem newItem = new TreeViewItem();
                                newItem.Header = markFoldersTreeItem.TreeItemCode;
                                newItem.Items.Add("*");
                                newItem.Tag = markFoldersTreeItem;
                                expandedItem.Items.Add(newItem);
                            }

                        }
                        else expandedItem.Items.Add("(пусто)");
                    }
                    else
                    {
                        TreeViewItem treeViewItemParent2 = (TreeViewItem)treeViewItemParent1.Parent;
                        VMMC_Core.TreeItem treeViewItemParentTag = (VMMC_Core.TreeItem)treeViewItemParent1.Tag;
                        //List<VMMC_Core.Complekt> dbComplects = new VMMC_Core.Complekt(SessionInfo).GetDbComplektsList();

                        ObservableCollection<VMMC_Core.TreeItem> complectsFoldersTreeItemList = new ObservableCollection<VMMC_Core.TreeItem>();                        
                        if (treeViewItemParent2.Header.ToString() == "Исполнительная документация") complectsFoldersTreeItemList = new VMMC_Core.TreeItem(sessionInfo).getDbFolderTreeItemList("ИД", treeViewItemParentTag.TreeItemCode, expandedItemTag.TreeItemCode.ToString());
                        else complectsFoldersTreeItemList = new VMMC_Core.TreeItem(sessionInfo).getDbFolderTreeItemList("РД", treeViewItemParentTag.TreeItemCode, expandedItemTag.TreeItemCode.ToString());

                        if (complectsFoldersTreeItemList != null)
                        {
                            foreach (VMMC_Core.TreeItem complectsFoldersTreeItem in complectsFoldersTreeItemList)
                            {
                                TreeViewItem newItem = new TreeViewItem();
                                newItem.Header = complectsFoldersTreeItem.TreeItemCode;
                                newItem.Items.Add("*");
                                newItem.Tag = new VMMC_Core.Complekt(sessionInfo).GetComplekt(complectsFoldersTreeItem.TreeItemCode);
                                expandedItem.Items.Add(newItem);
                            }

                        }
                        else expandedItem.Items.Add("(пусто)");

                        //IEnumerable<VMMC_Core.Complekt> targetComplects = dbComplects.Where(x => x.ComplektCode.Contains(expandedItemTag.TreeItemDescription.ToString()));

                        //if (targetComplects != null)
                        //{
                        //    foreach (VMMC_Core.Complekt complekt in targetComplects)
                        //    {
                        //        TreeViewItem newItem = new TreeViewItem();
                        //        newItem.Header = complekt.ComplektCode;
                        //        newItem.Items.Add("*");
                        //        newItem.Tag = complekt;
                        //        expandedItem.Items.Add(newItem);
                        //    }

                        //}
                        //else expandedItem.Items.Add("(пусто)");
                    }


                }
            }
        }
        
        private void trw_Products_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)e.OriginalSource;
            item.Items.Clear();
            DirectoryInfo dir;
            if (item.Tag is DriveInfo)
            {
                DriveInfo drive = (DriveInfo)item.Tag;
                dir = drive.RootDirectory;
            }
            else dir = (DirectoryInfo)item.Tag;


            try
            {
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    TreeViewItem newItem = new TreeViewItem();
                    newItem.Tag = subDir;
                    newItem.Header = subDir.ToString();
                    newItem.Items.Add("*");
                    item.Items.Add(newItem);
                }
            }
            catch
            { }
        }

        private void ShowEditor_Click(object sender, RoutedEventArgs e)
        {
            VMMC_Editor.MainWindow editorMainWindow = new VMMC_Editor.MainWindow(sessionInfo);
            //editorMainWindow.ViewModel = "ViewModel";

            editorMainWindow.Show();

            //editorMainWindow.ShowViewModel();


        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
            //sessionInfo = new VMMC_Core.SessionInfo();
            //VMMC_Core.CommonControls.LoginForm loginForm = new VMMC_Core.CommonControls.LoginForm();
            //loginForm.ShowDialog();
            //sessionInfo = loginForm.sessionInfo;

            //InitializeComponent();
            //InitializeTreeView();
        }

        private void treeViewItem_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)e.OriginalSource;
            if (selectedItem.Tag != null)
            {
                if (selectedItem.Tag.GetType().Name == "Complekt")
                {

                }
                else if (selectedItem.Tag.GetType().Name == "Document")
                {
                    VMMC_Core.Document selectedDocument = (VMMC_Core.Document)selectedItem.Tag;
                    if (selectedItem != null)
                    {
                        VMMC_Core.CommonControls.DocumentViewModel DocumentViewControl_DataContext = new VMMC_Core.CommonControls.DocumentViewModel(selectedDocument, selectedDocument.Revisions[0], null, !selectedDocument.IsExistInDB, sessionInfo);
                        DocumentViewControl.DataContext = DocumentViewControl_DataContext;
                    }
                }
            }
        }

        private void ReportTreeView_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)e.OriginalSource;
            VMMC_Core.CommonControls.Report_PiChartViewModel reportPiChartViewModel = (VMMC_Core.CommonControls.Report_PiChartViewModel)Report_PiChartView.DataContext;
            reportPiChartViewModel.sessionInfo = sessionInfo;
            reportPiChartViewModel.reportName = selectedItem.Header.ToString();
            Report_PiChartView.DataContext = reportPiChartViewModel;
        }
    }
}
