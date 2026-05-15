using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IRoleIncludeService : ICommonJoinService<RoleInclude>
    {
        Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);

        Task<IEnumerable<string>> GetAllRoleNamesByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);
    }
}
