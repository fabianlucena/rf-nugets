using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IRoleXUserService : ICommonJoinService<RoleXUser>
    {
        Task<IEnumerable<long>> GetListRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null);
    }
}
