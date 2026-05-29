using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;
using RFServices.Attributes;

namespace RFRBAC
{
    [SeedData(true)]
    public static class SeedInitialData
    {
        public static async Task Run(IServiceProvider provider)
        {
            var permissionXRoleService = provider.GetService<IPermissionXRoleService>();
            if (permissionXRoleService != null)
            {
                var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                    { "user", [
                        "changePassword",
                    ]},

                    { "admin", [
                        "changePassword",
                        "user.get", "user.add", "user.edit", "user.delete", "user.restore",
                    ]},
                };

                await permissionXRoleService.CreateIfNotExistsAsync(rolesPermissions);
            }
        }
    }
}
