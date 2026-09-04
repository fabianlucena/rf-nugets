using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices;

public interface IPermissionXRoleService : ICommonJoinService<PermissionXRole>
{                       
    Task<IEnumerable<long>> GetPermissionsIdByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null);
    Task<IEnumerable<string>> GetPermissionsNameByRolesIdAsync(IEnumerable<long> rolesId, PermissionXRoleQueryOptions? options = null);

    Task<int> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> rolesPermissions);
}
