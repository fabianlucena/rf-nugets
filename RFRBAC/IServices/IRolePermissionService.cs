using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IRolePermissionService
        : IServiceTimestamps<RolePermission>,
            IService<RolePermission>
    {
        Task<IEnumerable<Int64>> GetPermissionsIdForRolesIdAsync(IEnumerable<Int64> rolesId, GetOptions? options = null);

        Task<IEnumerable<Permission>> GetPermissionsForRolesIdAsync(IEnumerable<Int64> rolesId, GetOptions? options = null);

        Task<IEnumerable<Permission>> GetPermissionsForRoleIdListAsync(IEnumerable<Int64> rolesId, GetOptions? options = null);

        Task<IEnumerable<Permission>> GetAllPermissionsForUserIdAsync(Int64 userId, GetOptions? options = null);

        Task<IEnumerable<Permission>> GetAllPermissionsForUsersIdAsync(IEnumerable<Int64> usersId, GetOptions? options = null);
    }
}
