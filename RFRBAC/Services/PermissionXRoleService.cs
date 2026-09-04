using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFPermissions.IServices;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFRBAC.Services;

[RegisterService]
public class PermissionXRoleService(
    IPermissionXRoleRepository permissionXRoleRepository,
    IServiceProvider serviceProvider
)
    : CommonJoinService<PermissionXRole>(permissionXRoleRepository, serviceProvider),
    IPermissionXRoleService
{

    public IPermissionService PermissionService { get => ServiceProvider.GetRequiredService<IPermissionService>(); }
    public IRoleService RoleService { get => ServiceProvider.GetRequiredService<IRoleService>(); }
    public IUserService UserService { get => ServiceProvider.GetRequiredService<IUserService>(); }

    public async Task<IEnumerable<long>> GetPermissionsIdByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null)
        => await permissionXRoleRepository.GetPermissionsIdByRolesIdAsync(rolesId, options);

    public async Task<IEnumerable<string>> GetPermissionsNameByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null)
    {
        var allPermissionsId = await GetPermissionsIdByRolesIdAsync(rolesId, options);
        return await PermissionService.GetNamesByIdsAsync(allPermissionsId);
    }

    public async Task<int> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions)
    {
        var created = 0;

        var creatorId = await UserService.GetCurrentOrSystemUserIdAsync();
        foreach (var kvp in rolesPermissions)
        {
            var roleName = kvp.Key;
            var permissionsName = kvp.Value;
            var permissionsId = await PermissionService.GetIdsOrCreateByNamesAsync(
                permissionsName,
                createFactory: async permission =>
                {
                    permission.CreatedById = creatorId;
                    return permission;
                });
            var roleId = await RoleService.GetIdOrCreateByNameAsync(
                roleName,
                createFactory: async role =>
                {
                    role.CreatedById = creatorId;
                    role.UpdatedById = creatorId;
                    return role;
                });

            var existentPermissionsId = await GetPermissionsIdByRolesIdAsync([roleId]);
            var newPermissionsId = permissionsId.Except(existentPermissionsId);

            foreach (var permissionId in newPermissionsId)
            {
                await CreateAsync(new PermissionXRole
                {
                    RoleId = roleId,
                    PermissionId = permissionId,
                    CreatedById = creatorId,
                });
                created++;
            }
        }

        return created;
    }
}
