using Microsoft.Extensions.DependencyInjection;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFRolesPermissions.IServices;
using RFServices.Attributes;
using RFServices.Interfaces;

namespace RFRGOBAC;

[SeedData(true)]
public class RFRGOBACInitialDataSeeder(
    IRoleService roleService,
    IServiceProvider serviceProvider
) : ISeeder
{
    public async Task Run()
    {
        await roleService.GetOrCreateByNameAsync(
            "admin",
            createFactory: async T => new Role
            {
                Name = "organizationAdmin",
                Title = "Organization Administrator",
                Description = "Administrator for a single organization",
                IsSelectable = true,
                IsTranslatable = true,
                TranslationContext = "rfrgobac",
            }
        );
        
        var addRolePermissionService = serviceProvider.GetService<IAddRolePermissionService>();
        if (addRolePermissionService != null)
        {
            var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                { "admin",  [
                    "organizations.add", "organizations.get", "organizations.update", "organizations.delete", "organizations.restore",
                    "systemUsers.add", "systemUsers.get", "systemUsers.update", "systemUsers.delete", "systemUsers.restore",
                    "organizationUsers.add", "organizationUsers.get", "organizationUsers.update", "organizationUsers.delete", "organizationUsers.restore",
                    "selectableRole.get",
                ] },

                { "organizationAdmin",  [
                    "organizationUsers.add", "organizationUsers.get", "organizationUsers.update", "organizationUsers.delete", "organizationUsers.restore",
                    "selectableRole.get",
                ] },

                { "user",  [
                    "organizations.select",
                ] },
            };

            await addRolePermissionService.AddRolesPermissionsAsync(rolesPermissions);
        }
    }
}
