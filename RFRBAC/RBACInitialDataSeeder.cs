using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFServices.Attributes;
using RFServices.Interfaces;
using RFServices.Services;

namespace RFRBAC;

[SeedData(true)]
public class RBACInitialDataSeeder(
    IPermissionXRoleService permissionXRoleService,
    IRoleService roleService,
    IUserService userService,
    IRoleXUserService roleXUserService
) : ISeeder
{
    public async Task Run()
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

        var roleAdminId = await roleService.GetSingleIdByNameAsync("admin");
        var userAdminId = await userService.GetSingleIdByUsernameAsync("admin");
        if (!await roleXUserService.UserIdHasRoleIdAsync(userAdminId, roleAdminId))
        {
            await roleXUserService.CreateAsync(new RoleXUser
            {
                UserId = userAdminId,
                RoleId = roleAdminId,
                CreatedById = await userService.GetCurrentOrSystemUserIdAsync(),
            });
        }
    }
}