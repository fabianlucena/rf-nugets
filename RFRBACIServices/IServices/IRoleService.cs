using RFBaseIServices.IServices;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;

namespace RFRBACIServices.IServices
{
    public interface IRoleService : ICommonEntityService<Role>
    {
        Task<IEnumerable<string>> GetListNamesByIdAsync(IEnumerable<long> ids, RoleQueryOptions? options = null);
    }
}
