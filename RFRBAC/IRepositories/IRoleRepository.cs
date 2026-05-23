using RFIRepositories.IRepositories;
using RFRBAC.Entities;
using RFRBAC.QueryOptions;

namespace RFRBAC.IRepositories
{
    public interface IRoleRepository : ICommonEntityRepository<Role>
    {
        Task<IEnumerable<string>> GetNamesAsync(RoleQueryOptions options);
    }
}