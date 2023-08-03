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
                InitializeDataContext();
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
                InitializeDataContext();
            }
        }

        private void InitializeDataContext()
        {
            OrganisationRolesInfoViewModel organisationRolesInfoViewDataContext = new OrganisationRolesInfoViewModel(sessionInfo);
            if (organisationRolesInfoViewDataContext.organisationRolesInfoCollection != null)
            {
                OrganisationRolesInfoView.DataContext = organisationRolesInfoViewDataContext;
                if (organisationRolesInfoViewDataContext.organisationRolesInfoCollection.Count > 0) OrganisationRolesInfoView.Visibility = Visibility.Visible;
            }

            DocumentClassRulesViewModel documentClassRulesViewDataContext = new DocumentClassRulesViewModel(sessionInfo);
            if (documentClassRulesViewDataContext.documentClassRulesCollection != null)
            {
                DocumentClassRulesView.DataContext = documentClassRulesViewDataContext;
                if (documentClassRulesViewDataContext.documentClassRulesCollection.Count > 0) DocumentClassRulesView.Visibility = Visibility.Visible;
            }
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
