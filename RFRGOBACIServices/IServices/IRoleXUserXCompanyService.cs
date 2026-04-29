using RFBaseIServices.IServices;
using RFRBACIServices.IServices;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;

namespace RFRGCBACIServices.IServices
{
    public interface IRoleXUserXCompanyService : ICommonJoinService<RoleXUserXCompany>
    {
        Task<IEnumerable<long>> GetRolesIdByUserIdCompanyIdAsync(long userId, long companyId, RoleXUserXCompanyQueryOptions? options = null);
        Task<IEnumerable<long>> GetAllRolesIdByUserIdAndCompanyIdAsync(long userId, long companyId, RoleXUserXCompanyQueryOptions? options = null);
        Task<IEnumerable<Company>> GetListCompaniesByUserIdAsync(long userId, RoleXUserXCompanyQueryOptions? options = null);
    }
}
