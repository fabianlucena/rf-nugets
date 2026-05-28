using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;
using RFServices.Attributes;

namespace RFRBAC
{
    public static class SeedData
    {
        [SeedData(true)]
        public static async Task Setup(IServiceProvider provider)
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
