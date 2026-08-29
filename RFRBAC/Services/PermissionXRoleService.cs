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

    public async Task<IEnumerable<long>> GetPermissionIdsByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        => await permissionXRoleRepository.GetPermissionIdsByRoleIdsAsync(roleIds, options);

    public async Task<IEnumerable<string>> GetPermissionNamesByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
    {
        var allPermissionIds = await GetPermissionIdsByRoleIdsAsync(roleIds, options);
        return await PermissionService.GetNamesByIdsAsync(allPermissionIds);
    }

    public async Task<int> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions)
    {
        var created = 0;

        var creatorId = await UserService.GetCurrentOrSystemUserIdAsync();
        foreach (var kvp in rolesPermissions)
        {
            var roleName = kvp.Key;
            var permissionNames = kvp.Value;
            var permissionIds = await PermissionService.GetIdsOrCreateByNamesAsync(
                permissionNames,
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

            var existentPermissionIds = await GetPermissionIdsByRoleIdsAsync([roleId]);
            var newPermissionIds = permissionIds.Except(existentPermissionIds);

            foreach (var permissionId in newPermissionIds)
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
