using Microsoft.Extensions.DependencyInjection;
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

    public async Task<IEnumerable<long>> GetPermissionIdsByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
        => await permissionXRoleRepository.GetPermissionIdsByRoleIdsAsync(roleIds, options);

    public async Task<IEnumerable<string>> GetPermissionNamesByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null)
    {
        var allPermissionIds = await GetPermissionIdsByRoleIdsAsync(roleIds, options);
        return await PermissionService.GetNamesByIdsAsync(allPermissionIds);
    }

    public async Task<bool> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions)
    {
        foreach (var kvp in rolesPermissions)
        {
            var roleName = kvp.Key;
            var permissionNames = kvp.Value;
            var permissionIds = await PermissionService.GetIdsOrCreateByNamesAsync(permissionNames);
            var roleId = await RoleService.GetSingleIdOrCreateByNameAsync(roleName);

            var existentPermissionIds = await GetPermissionIdsByRoleIdsAsync([roleId]);
            var newPermissionIds = permissionIds.Except(existentPermissionIds);

            await Task.WhenAll(newPermissionIds.Select(permissionId => CreateAsync(new PermissionXRole
            {
                RoleId = roleId,
                PermissionId = permissionId
            })));
        }

        return true;
    }
}
