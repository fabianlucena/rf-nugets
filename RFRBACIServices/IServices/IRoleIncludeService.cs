using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IRoleIncludeService : ICommonJoinService<RoleInclude>
    {
        Task<IEnumerable<long>> GetAllRolesIdByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null);

        Task<IEnumerable<string>> GetAllRolesNamesByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null);
    }
}
