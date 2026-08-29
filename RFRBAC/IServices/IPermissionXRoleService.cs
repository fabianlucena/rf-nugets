using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices;

public interface IPermissionXRoleService : ICommonJoinService<PermissionXRole>
{                       
    Task<IEnumerable<long>> GetPermissionIdsByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);
    Task<IEnumerable<string>> GetPermissionNamesByRoleIdsAsync(IEnumerable<long> roleIds, PermissionXRoleQueryOptions? options = null);

    Task<int> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions);
}
