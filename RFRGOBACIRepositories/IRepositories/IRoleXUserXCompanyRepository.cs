using RFBaseIRepositories.IRepositories;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;

namespace RFRGCBACIRepositories.IRepositories
{
    public interface IRoleXUserXCompanyRepository : ICommonJoinRepository<RoleXUserXCompany>
    {
        Task<IEnumerable<long>> GetListIdByUserIdAndCompanyIdAsync(long userId, long? companyId, RoleXUserXCompanyQueryOptions? options = null);
        Task<IEnumerable<Company>> GetListCompaniesByUserIdAsync(long userId, RoleXUserXCompanyQueryOptions? options = null);
    }
}