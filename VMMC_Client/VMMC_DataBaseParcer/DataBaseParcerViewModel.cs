using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace VMMC_DataBaseParcer
{
    public class DataBaseParcerViewModel : INotifyPropertyChanged
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel> AttributeObjectValueViewModelCollection { get; set; }


        private ObservableCollection<VMMC_Core.Complekt> complektsCollection;
        public ObservableCollection<VMMC_Core.Complekt> ComplektsCollection
        {
            get { return complektsCollection; }
            set
            {
                complektsCollection = value;
                OnDataBaseParcerPropertyChanged("ComplektsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.Document> documentsCollection;
        public ObservableCollection<VMMC_Core.Document> DocumentsCollection
        {
            get { return documentsCollection; }
            set
            {
                documentsCollection = value;
                OnDataBaseParcerPropertyChanged("DocumentsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.Tag> tagsCollection;
        public ObservableCollection<VMMC_Core.Tag> TagsCollection
        {
            get { return tagsCollection; }
            set
            {
                tagsCollection = value;
                OnDataBaseParcerPropertyChanged("TagsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.TreeItem> treeItemsCollection;
        public ObservableCollection<VMMC_Core.TreeItem> TreeItemsCollection
        {
            get { return treeItemsCollection; }
            set
            {
                treeItemsCollection = value;
                OnDataBaseParcerPropertyChanged("TreeItemsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.Relationship> relationshipsCollection;
        public ObservableCollection<VMMC_Core.Relationship> RelationshipsCollection
        {
            get { return relationshipsCollection; }
            set
            {
                relationshipsCollection = value;
                OnDataBaseParcerPropertyChanged("RelationshipsCollection");
            }
        }


        private ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesCollection;
        public ObservableCollection<VMMC_Core.AttributeObjectValue> AttributeObjectValuesCollection
        {
            get { return attributeObjectValuesCollection; }
            set
            {
                attributeObjectValuesCollection = value;
                OnDataBaseParcerPropertyChanged("AttributeObjectValuesCollection");
            }
        }

        private bool silentMode;
        public bool SilentMode
        {
            get { return silentMode; }
            set
            {
                silentMode = value;
                OnDataBaseParcerPropertyChanged("SilentMode");
            }
        }


        private bool hasParentWindow;
        public bool HasParentWindow
        {
            get { return hasParentWindow; }
            set
            {
                hasParentWindow = value;
                OnDataBaseParcerPropertyChanged("HasParentWindow");
            }
        }


        public DataBaseParcerViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;
        }


        public void getDbTagRdRelationships()
        {            
            string sql = @"SELECT 
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

            ObservableCollection<VMMC_Core.Relationship> relationshipList = new VMMC_Core.Relationship(sessionInfo).GetRelationshipsFromQuery(sql);
            if (relationshipList != null) RelationshipsCollection = relationshipList;
        }
        public void getDbTreeRdRelationships()
        {
            string sql = @"SELECT NEWID() as [Id]
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

            ObservableCollection<VMMC_Core.Relationship> relationshipList = new VMMC_Core.Relationship(sessionInfo).GetRelationshipsFromQuery(sql);
            if (relationshipList != null) RelationshipsCollection = relationshipList;
        }        
        public void getRelationshipsFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.Relationship> relationshipList = new VMMC_Core.Relationship(sessionInfo).GetRelationshipsFromQuery(sql);
            if (relationshipList!=null) RelationshipsCollection = relationshipList;
        }
        public void getTreeItemsFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.TreeItem> treeItemsList = new VMMC_Core.TreeItem(sessionInfo).getDbTreeItemListFromQuery(sql);
            if (treeItemsList != null) TreeItemsCollection = treeItemsList;
        }
        public void getComplektsFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.Complekt> complektList = new VMMC_Core.Complekt(sessionInfo).GetComplektsListFromQuery(sql);
            AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();
            if (complektList != null) ComplektsCollection = complektList;
            
        }
        public void getAttributesFromQuery(string sql)
        {
            ObservableCollection<VMMC_Core.AttributeObjectValue> attributeObjectValuesList = new VMMC_Core.AttributeObjectValue(sessionInfo).GetAttributesValuesListFromQuery(sql);
            AttributeObjectValueViewModelCollection = new ObservableCollection<VMMC_Core.CommonControls.AttributeObjectValueViewModel>();
            if (attributeObjectValuesList != null) AttributeObjectValuesCollection = attributeObjectValuesList;

            foreach (VMMC_Core.AttributeObjectValue aov in AttributeObjectValuesCollection)
            {
                AttributeObjectValueViewModelCollection.Add(new VMMC_Core.CommonControls.AttributeObjectValueViewModel() { AttributeObjectValue = aov });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnDataBaseParcerPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


    }
}
