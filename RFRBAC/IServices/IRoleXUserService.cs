using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleXUserService : ICommonJoinService<RoleXUser>
    {
        Task<IEnumerable<long>> GetRolesIdByUsersIdAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<long>> GetRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetRolesNameByUsersIdAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetRolesNameByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<Role>> GetRolesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<Role>> GetAllRolesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<long> SetAllRolesForUserIdAsync(IEnumerable<string> roles, long userId, RoleXUserQueryOptions? options = null);
        Task<long> SetAllRolesIdForUserIdAsync(IEnumerable<long> rolesId, long userId, RoleXUserQueryOptions? options = null);
        Task<bool> UserIdHasRoleIdAsync(long userId, long roleId, RoleXUserQueryOptions? options = null);

        Task<bool> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> userRoles);
    }
}
