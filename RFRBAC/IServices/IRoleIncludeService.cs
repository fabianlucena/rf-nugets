using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleIncludeService : ICommonJoinService<RoleInclude>
    {
        Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);

        Task<IEnumerable<string>> GetAllRoleNamesByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);
    }
}
