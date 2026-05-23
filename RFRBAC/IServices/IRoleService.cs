using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleService : ICommonEntityService<Role>
    {
        Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, RoleQueryOptions? options = null);
    }
}
