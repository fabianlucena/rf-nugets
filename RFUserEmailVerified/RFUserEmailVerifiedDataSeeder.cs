using Microsoft.Extensions.DependencyInjection;
using RFRolesPermissions.IServices;
using RFServices.Attributes;
using RFServices.Interfaces;

namespace RFUserEmailVerified;

[SeedData(true)]
public class RFUserEmailVerifiedDataSeeder(
    IServiceProvider serviceProvider
) : ISeeder
{
    public async Task Run()
    {
        var addRolePermissionService = serviceProvider.GetService<IAddRolePermissionService>();
        if (addRolePermissionService != null)
        {
            var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                { "user",  [
                    "passwordRecovery",
                ] },

                { "admin",  [
                    "passwordRecovery",
                ] },
            };

            await addRolePermissionService.AddRolesPermissionsAsync(rolesPermissions);
        }
    }
}
