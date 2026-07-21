using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleXUserService : ICommonJoinService<RoleXUser>
    {
        Task<IEnumerable<long>> GetRoleIdsByUserIdsAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<long>> GetRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetRoleNamesByUserIdsAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<long> SetAllRolesForUserIdAsync(IEnumerable<string> roles, long userId, RoleXUserQueryOptions? options = null);
        Task<bool> UserIdHasRoleIdAsync(long userId, long roleId, RoleXUserQueryOptions? options = null);

        Task<bool> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> userRoles);
    }
}
