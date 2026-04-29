using RFBaseIRepositories.IRepositories;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIRepositories.IRepositories
{
    public interface IRoleXUserXCompanyRepository : ICommonJoinRepository<RoleXUserXCompany>
    {
        Task<IEnumerable<long>> GetListIdByUserIdAndCompanyIdAsync(long userId, long? companyId, RoleXUserXCompanyQueryOptions? options = null);
        Task<IEnumerable<Company>> GetListCompaniesByUserIdAsync(long userId, RoleXUserXCompanyQueryOptions? options = null);
    }
}