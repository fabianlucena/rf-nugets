using RFIServices.IServices;
using RFPermissions.Entities;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices
{
    public interface IRoleService : INominableEntityService<Role>
    {
        Task<long> GetSingleIdOrCreateByNameAsync(string name, RoleQueryOptions? options = null, Func<Role, Task<Role>>? completeCreateData = null);
    }
}
