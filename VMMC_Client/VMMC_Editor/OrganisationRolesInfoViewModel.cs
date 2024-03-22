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
                bool isWDDeveloper = false;
                bool isDesigner = false;
                bool isDeliveryResponsible = false;
                List<OrganizationRole> organizationRoles = new VMMC_Core.OrganizationRole(sessionInfo).getOrganizationRoles(org.OrganizationId.ToString());
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
                        case "Разработчик рабочей документации":
                            isWDDeveloper = true;
                            break;
                        case "Проектировщик":
                            isDesigner = true;
                            break;
                        case "Ответственный по поставке":
                            isDeliveryResponsible = true;
                            break;
                        default:
                            break;
                    }
                }

                OrganisationRolesInfo newOrgInfo = new OrganisationRolesInfo
                {
                    OrganisationId = org.OrganizationId.ToString(),
                    OrganisationName = org.OrganizationName,
                    OrganisationShortName = org.OrganizationShortName,
                    OrganisationINN = org.OrganizationINN,
                    IsOrganization = isOrganization,
                    IsManufacturer = isManufacturer,
                    IsSupplier = isSupplier,
                    IsControl = isControl,
                    IsSMR = isSMR,
                    IsWDDeveloper = isWDDeveloper,
                    IsDesigner = isDesigner,
                    IsDeliveryResponsible = isDeliveryResponsible,
                    IsOrganization_DB = isOrganization,
                    IsManufacturer_DB = isManufacturer,
                    IsSupplier_DB = isSupplier,
                    IsControl_DB = isControl,
                    IsSMR_DB = isSMR,        
                    IsWDDeveloper_DB = isWDDeveloper,
                    IsDesigner_DB = isDesigner,
                    IsDeliveryResponsible_DB = isDeliveryResponsible,
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
                if (changedOrganisationRoleInfo.IsWDDeveloper != changedOrganisationRoleInfo.IsWDDeveloper_DB)
                {
                    if (changedOrganisationRoleInfo.IsWDDeveloper) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Разработчик рабочей документации").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Разработчик рабочей документации").FirstOrDefault().RoleId);
                }
                if (changedOrganisationRoleInfo.IsDesigner != changedOrganisationRoleInfo.IsDesigner_DB)
                {
                    if (changedOrganisationRoleInfo.IsDesigner) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Проектировщик").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Проектировщик").FirstOrDefault().RoleId);
                }
                if (changedOrganisationRoleInfo.IsDeliveryResponsible != changedOrganisationRoleInfo.IsDeliveryResponsible_DB)
                {
                    if (changedOrganisationRoleInfo.IsDeliveryResponsible) new VMMC_Core.OrganizationRole(sessionInfo).AddOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Ответственный по поставке").FirstOrDefault().RoleId);
                    else new VMMC_Core.OrganizationRole(sessionInfo).DeleteOrganisationRole(changedOrganisationRoleInfo.OrganisationId, roles.Where(x => x.RoleName == "Ответственный по поставке").FirstOrDefault().RoleId);
                }
            }
            fillOrgListView();
        }
    }
}
