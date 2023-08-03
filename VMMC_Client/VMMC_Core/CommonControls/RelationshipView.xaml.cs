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

namespace VMMC_Core.CommonControls
{
    /// <summary>
    /// Логика взаимодействия для RelationshipView.xaml
    /// </summary>
    public partial class RelationshipView : UserControl
    {
        public RelationshipView()
        {
            InitializeComponent();
            CommonControls.RelationshipViewModel relationshipViewModel = new VMMC_Core.CommonControls.RelationshipViewModel(null);
            this.DataContext = relationshipViewModel;
            
        }

      
        private void TreeViewItem_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //VMMC_Core.CommonControls.RelationshipViewModel relationshipViewModel = (VMMC_Core.CommonControls.RelationshipViewModel)this.DataContext;

            //RelatedObjects_Tree.Items.Clear();

            //TreeViewItem docRDTVI = new TreeViewItem();
            //docRDTVI.Header = "Рабочая документация";
            //docRDTVI.Items.Add("пусто");
            //TreeViewItem docIDTVI = new TreeViewItem();
            //docIDTVI.Header = "Исполнительная документация";
            //docIDTVI.Items.Add("пусто");
            //TreeViewItem doc3DTVI = new TreeViewItem();
            //doc3DTVI.Header = "3D-модели";
            //doc3DTVI.Items.Add("пусто");
            //RelatedObjects_Tree.Items.Add(docRDTVI);
            //RelatedObjects_Tree.Items.Add(docIDTVI);
            //RelatedObjects_Tree.Items.Add(doc3DTVI);

        }
    }
}
