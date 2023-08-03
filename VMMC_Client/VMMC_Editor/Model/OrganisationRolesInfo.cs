using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;


namespace VMMC_Editor
{
    public class OrganisationRolesInfo : INotifyPropertyChanged
    {
        public string organisationId;
        public string organisationName;
        public string organisationShortName;
        public string organisationINN;
        public bool isOrganization;
        public bool isManufacturer;
        public bool isSupplier;
        public bool isControl;
        public bool isSMR;

        public bool isOrganization_DB;
        public bool isManufacturer_DB;
        public bool isSupplier_DB;
        public bool isControl_DB;
        public bool isSMR_DB;

        public bool isSaved;

        public string OrganisationId
        {
            get { return organisationId; }
            set
            {
                organisationId = value;
                OnOrganisationRolesInfoPropertyChanged("OrganisationId");
            }
        }
        public string OrganisationName
        {
            get { return organisationName; }
            set
            {
                organisationName = value;
                OnOrganisationRolesInfoPropertyChanged("OrganisationName");
            }
        }
        public string OrganisationShortName
        {
            get { return organisationShortName; }
            set
            {
                organisationShortName = value;
                OnOrganisationRolesInfoPropertyChanged("OrganisationShortName");
            }
        }
        public string OrganisationINN
        {
            get { return organisationINN; }
            set
            {
                organisationINN = value;
                OnOrganisationRolesInfoPropertyChanged("OrganisationINN");
            }
        }
        public bool IsOrganization
        {
            get { return isOrganization; }
            set
            {
                isOrganization = value;
                OnOrganisationRolesInfoPropertyChanged("IsOrganization");
            }
        }
        public bool IsManufacturer
        {
            get { return isManufacturer; }
            set
            {
                isManufacturer = value;
                OnOrganisationRolesInfoPropertyChanged("IsManufacturer");
            }
        }
        public bool IsSupplier
        {
            get { return isSupplier; }
            set
            {
                isSupplier = value;
                OnOrganisationRolesInfoPropertyChanged("IsSupplier");
            }
        }
        public bool IsControl
        {
            get { return isControl; }
            set
            {
                isControl = value;
                OnOrganisationRolesInfoPropertyChanged("IsControl");
            }
        }
        public bool IsSMR
        {
            get { return isSMR; }
            set
            {
                isSMR = value;
                OnOrganisationRolesInfoPropertyChanged("IsSMR");
            }
        }
        public bool IsOrganization_DB
        {
            get { return isOrganization_DB; }
            set
            {
                isOrganization_DB = value;
                OnOrganisationRolesInfoPropertyChanged("IsOrganization_DB");
            }
        }
        public bool IsManufacturer_DB
        {
            get { return isManufacturer_DB; }
            set
            {
                isManufacturer_DB = value;
                OnOrganisationRolesInfoPropertyChanged("IsManufacturer_DB");
            }
        }
        public bool IsSupplier_DB
        {
            get { return isSupplier_DB; }
            set
            {
                isSupplier_DB = value;
                OnOrganisationRolesInfoPropertyChanged("IsSupplier_DB");
            }
        }
        public bool IsControl_DB
        {
            get { return isControl_DB; }
            set
            {
                isControl_DB = value;
                OnOrganisationRolesInfoPropertyChanged("IsControl_DB");
            }
        }
        public bool IsSMR_DB
        {
            get { return isSMR_DB; }
            set
            {
                isSMR_DB = value;
                OnOrganisationRolesInfoPropertyChanged("IsSMR_DB");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnOrganisationRolesInfoPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
