using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;

namespace RFRBAC
{
    public static class SeedSystemData
    {
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
