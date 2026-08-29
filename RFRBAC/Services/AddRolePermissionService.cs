using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFRolesPermissions.IServices;

namespace RFRBAC.Services;

[RegisterService]
public class AddRolePermissionService(
        IPermissionXRoleService permissionXRoleService
    )
    : IAddRolePermissionService
{
    public async Task<int> AddRolesPermissionsAsync(Dictionary<string, IEnumerable<string>> rolesPermissions)
        => await permissionXRoleService.CreateIfNotExistsAsync(rolesPermissions);
}
