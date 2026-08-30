using Microsoft.Extensions.DependencyInjection;
using RFAuth.IServices;
using RFEntities.Entities;
using RFIServices.IServices;
using RFRolesPermissions.IServices;
using RFServices.Attributes;
using RFServices.Interfaces;

namespace RFRGOBAC;

[SeedData(true)]
public class RFRGOBACInitialDataSeeder(
    IServiceProvider serviceProvider
) : ISeeder
{
    public async Task Run()
    {
        var addRolePermissionService = serviceProvider.GetService<IAddRolePermissionService>();
        if (addRolePermissionService != null)
        {
            var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                { "admin",  [
                    "organizations.add", "organizations.get", "organizations.update", "organizations.delete", "organizations.restore",
                    "organizationUsers.add", "organizationUsers.get", "organizationUsers.update", "organizationUsers.delete", "organizationUsers.restore",
                ] },

                { "user",  [
                    "organizations.select",
                ] },
            };

            await addRolePermissionService.AddRolesPermissionsAsync(rolesPermissions);
        }
    }
}
