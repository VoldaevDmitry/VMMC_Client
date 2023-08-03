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

namespace VMMC_Editor
{
    /// <summary>
    /// Логика взаимодействия для OrganisationRolesInfoView.xaml
    /// </summary>
    public partial class OrganisationRolesInfoView : UserControl
    {
        public OrganisationRolesInfoView()
        {
            InitializeComponent();
            
        }
        public void ClearRowButton_Click(object sender, RoutedEventArgs e)
        {
            string s = e.RoutedEvent.Name;
            Button btn = (Button)e.Source;
            OrganisationRolesInfo src = (OrganisationRolesInfo)btn.DataContext;
            OrganisationRolesInfo crntItem = (OrganisationRolesInfo)OrganisationRolesDataGrid.CurrentItem;
            
            var dgr = OrganisationRolesDataGrid.Items[OrganisationRolesDataGrid.SelectedIndex];

            crntItem.IsOrganization = false;
            crntItem.IsManufacturer = false;
            crntItem.IsSupplier = false;
            crntItem.IsSMR = false;
            crntItem.isSaved = false;
        }
        public void SaveRowButton_Click(object sender, RoutedEventArgs e)
        {
            //List<OrganisationRolesInfo>  changedOrganisationRolesInfoCollection = OrganisationRolesDataGrid.ItemsSource;

            //foreach (OrganisationRolesInfo item in OrganisationRolesDataGrid.ItemsSource)
            //{
            //    //if (!item.IsSaved) changedOrganisationRolesInfoCollection.Add(item);

            //    if (item.IsOrganization != item.IsOrganization_DB) changedOrganisationRolesInfoCollection.Add(item);
            //    if (item.IsManufacturer != item.IsManufacturer_DB) changedOrganisationRolesInfoCollection.Add(item);
            //    if (item.IsSupplier != item.IsSupplier_DB) changedOrganisationRolesInfoCollection.Add(item);
            //    if (item.IsControl != item.IsControl_DB) changedOrganisationRolesInfoCollection.Add(item);
            //    if (item.IsSMR != item.IsSMR_DB) changedOrganisationRolesInfoCollection.Add(item);
            //}
            OrganisationRolesInfoViewModel ttt = (OrganisationRolesInfoViewModel)this.DataContext;
            ttt.SaveRow(ttt.organisationRolesInfoCollection);
            //ttt.SQLServer = "";
            OrganisationRolesDataGrid.ItemsSource = ttt.organisationRolesInfoCollection;
            OrganisationRolesDataGrid.Items.Refresh();

        }

        public void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            OrganisationRolesInfoViewModel ttt = (OrganisationRolesInfoViewModel)this.DataContext;
            ttt.fillOrgListView();
            OrganisationRolesDataGrid.ItemsSource = ttt.organisationRolesInfoCollection;
            OrganisationRolesDataGrid.Items.Refresh();

        }


    }
}
