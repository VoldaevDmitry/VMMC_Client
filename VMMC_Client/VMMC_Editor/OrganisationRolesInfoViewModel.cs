using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using VMMC_Core;



namespace VMMC_Editor
{
    public class OrganisationRolesInfoViewModel
    {
        public VMMC_Core.SessionInfo sessionInfo;
        public List<OrganisationRolesInfo> organisationRolesInfoCollection { get; set; }
        public OrganisationRolesInfoViewModel(VMMC_Core.SessionInfo session)
        {
            sessionInfo = session;            
            fillOrgListView();
        }
        
        public void fillOrgListView()
        {
            organisationRolesInfoCollection = new List<OrganisationRolesInfo>();
            List<Organization> organisationsCollection = new VMMC_Core.Organization(sessionInfo).getOrganizations();
            foreach (Organization org in organisationsCollection)
            {
                bool isOrganization = false;
                bool isManufacturer = false;
                bool isSupplier = false;
                bool isControl = false;
                bool isSMR = false;
                List<OrganizationRole> organizationRoles = new VMMC_Core.OrganizationRole(sessionInfo).getOrganizationRoles(org.OrganizationId);
                foreach (OrganizationRole orgRol in organizationRoles)
                {
                    switch (orgRol.RoleName)
                    {
                        case "Организация":
                            isOrganization = true;
                            break;
                        case "Изготовитель":
                            isManufacturer = true;
                            break;
                        case "Поставщик":
                            isSupplier = true;
                            break;
                        case "Стройконтроль":
                            isControl = true;
                            break;
                        case "СМР":
                            isSMR = true;
                            break;
                        default:
                            break;
                    }
                }

                OrganisationRolesInfo newOrgInfo = new OrganisationRolesInfo
                {
                    OrganisationId = org.OrganizationId,
                    OrganisationName = org.OrganizationName,
                    OrganisationShortName = org.OrganizationShortName,
                    OrganisationINN = org.OrganizationINN,
                    IsOrganization = isOrganization,
                    IsManufacturer = isManufacturer,
                    IsSupplier = isSupplier,
                    IsControl = isControl,
                    IsSMR = isSMR,
                    IsOrganization_DB = isOrganization,
                    IsManufacturer_DB = isManufacturer,
                    IsSupplier_DB = isSupplier,
                    IsControl_DB = isControl,
                    IsSMR_DB = isSMR,                
                };
                organisationRolesInfoCollection.Add(newOrgInfo);
            }
            //List<DataGridRow> dgr = new List<DataGridRow>();
            //OrganisationRolesInfoCollection = organisationRolesInfoCollection;
            //OrganisationRolesDataGrid.ItemsSource = OrganisationRolesInfoCollection;
            //OrganisationRolesDataGrid.ItemsSource = organisationRolesInfoCollection;

        }
        public void SaveRow(List<OrganisationRolesInfo> changedOrganisationRolesInfoCollection)
        {
            List<Role> roles = new VMMC_Core.Role(sessionInfo).getRoles();
            foreach (OrganisationRolesInfo changedOrganisationRoleInfo in changedOrganisationRolesInfoCollection)
            {
                if (changedOrganisationRoleInfo.IsOrganization != changedOrganisationRoleInfo.IsOrganization_DB)
                {
                    if (changedOrganisationRoleInfo.IsOrganization) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Организация").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Организация").FirstOrDefault().RoleId);
                }
                if (changedOrganisationRoleInfo.IsManufacturer != changedOrganisationRoleInfo.IsManufacturer_DB)
                {
                    if (changedOrganisationRoleInfo.IsManufacturer) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Изготовитель").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Изготовитель").FirstOrDefault().RoleId);
                }
                if (changedOrganisationRoleInfo.IsSupplier != changedOrganisationRoleInfo.IsSupplier_DB)
                {
                    if (changedOrganisationRoleInfo.IsSupplier) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Поставщик").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Поставщик").FirstOrDefault().RoleId);
                }
                if (changedOrganisationRoleInfo.IsControl != changedOrganisationRoleInfo.IsControl_DB)
                {
                    if (changedOrganisationRoleInfo.IsControl) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Стройконтроль").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Стройконтроль").FirstOrDefault().RoleId);
                }
                if (changedOrganisationRoleInfo.IsSMR != changedOrganisationRoleInfo.IsSMR_DB)
                {
                    if (changedOrganisationRoleInfo.IsSMR) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "СМР").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "СМР").FirstOrDefault().RoleId);
                }
            }
            fillOrgListView();
        }
    }
}
