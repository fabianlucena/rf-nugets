using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleXUserService : ICommonJoinService<RoleXUser>
    {
        Task<IEnumerable<long>> GetListRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<string>> GetListRoleNamesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<long> SetAllRolesForUserIdAsync(IEnumerable<string> roles, long userId, RoleXUserQueryOptions? options = null);
        Task<bool> UserIdHasRoleIdAsync(long userId, long roleId, RoleXUserQueryOptions? options = null);
    }
}
