using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace VMMC_Core.CommonControls
{
    public class RelationshipViewModel
    {
        public ObservableCollection<VMMC_Core.DbObject> RelatedeObjectsCollection { get; set; }

        public ObservableCollection<VMMC_Core.Complekt> RelatedeComplektObjectsCollection { get; set; }
        public ObservableCollection<VMMC_Core.Document> RelatedeDocumentObjectsCollection { get; set; }
        public ObservableCollection<VMMC_Core.DbObject> RelatedeTagObjectsCollection { get; set; }
        public ObservableCollection<VMMC_Core.DbObject> RelatedeOrganisationObjectsCollection { get; set; }
        public ObservableCollection<VMMC_Core.DbObject> RelatedeOtherObjectsCollection { get; set; }
        public ObservableCollection<VMMC_Core.DbObject> RelatedeObjects { get; set; }
        public ObservableCollection<TreeViewItem> RelatedObjectsTree { get; set; }

        public RelationshipViewModel(ObservableCollection<VMMC_Core.DbObject> relatedeObjectsCollection)
        {
            if (relatedeObjectsCollection != null)
            {
                if (relatedeObjectsCollection.Count() > 0)
                {
                    RelatedeComplektObjectsCollection = new ObservableCollection<VMMC_Core.Complekt>();
                    RelatedeDocumentObjectsCollection = new ObservableCollection<Document>();
                    RelatedeTagObjectsCollection = new ObservableCollection<DbObject>();
                    RelatedeOrganisationObjectsCollection = new ObservableCollection<DbObject>();
                    RelatedeOtherObjectsCollection = new ObservableCollection<DbObject>();

                    RelatedeObjectsCollection = relatedeObjectsCollection;
                    foreach (VMMC_Core.DbObject dbObject in relatedeObjectsCollection)
                    {
                        if (dbObject.SystemTypeId == 5) RelatedeComplektObjectsCollection.Add(new Complekt(dbObject.sessionInfo).GetComplekt(dbObject.ObjectCode));
                        else if (dbObject.SystemTypeId == 2) RelatedeDocumentObjectsCollection.Add(new Document(dbObject.sessionInfo).GetDocument(dbObject.ObjectCode));
                        else if (dbObject.SystemTypeId == 3) RelatedeTagObjectsCollection.Add(dbObject);
                        else if (dbObject.SystemTypeId == 6) RelatedeOrganisationObjectsCollection.Add(dbObject);
                        else RelatedeOtherObjectsCollection.Add(dbObject);

                    }

                   

                }
            }
            //RelatedeComplektObjectsCollection = relatedeObjectsCollection.Where(x => x.SystemTypeId == 5);
            //RelatedeDocumentObjectsCollection = relatedeObjectsCollection.Where(x => x.SystemTypeId == 2);
            //RelatedeTagObjectsCollection = relatedeObjectsCollection.Where(x => x.SystemTypeId == 3);
            //RelatedeOrganisationObjectsCollection = relatedeObjectsCollection.Where(x => x.SystemTypeId == 6);
            //RelatedeOtherObjectsCollection = relatedeObjectsCollection.Where(x => x.SystemTypeId != 5 && x.SystemTypeId == 2 && x.SystemTypeId == 3 && x.SystemTypeId == 6);

            //if(RelatedeComplektObjectsCollection.Count()>0) RelatedeObjects.A
        }

        private void TreeViewItem_DataContextChanged()
        {
            
            RelatedObjectsTree = new ObservableCollection<TreeViewItem>();

            TreeViewItem docRDTVI = new TreeViewItem();
            docRDTVI.Header = "Комплекты";
            docRDTVI.Items.Add("пусто");
            TreeViewItem docIDTVI = new TreeViewItem();
            docIDTVI.Header = "Документы";
            docIDTVI.Items.Add("пусто");
            TreeViewItem doc3DTVI = new TreeViewItem();
            doc3DTVI.Header = "3D-модели";
            doc3DTVI.Items.Add("пусто");
            RelatedObjectsTree.Add(docRDTVI);
            RelatedObjectsTree.Add(docIDTVI);
            RelatedObjectsTree.Add(doc3DTVI);

        }
    }
}
