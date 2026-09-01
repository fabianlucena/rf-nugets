using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleIncludeService : ICommonJoinService<RoleInclude>
    {
        Task<IEnumerable<long>> GetAllRolesIdByRolesIdAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);

        Task<IEnumerable<string>> GetAllRolesNameByRolesIdAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null);
    }
}
