using RFBaseServices.Services;
using RFRBACIServices.IServices;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;
using RFRGCBACIRepositories.IRepositories;
using RFRGCBACIServices.IServices;

namespace RFRGCBACServices.Services
{
    public class RoleXUserXCompanyService(
        IRoleXUserXCompanyRepository roleXUserXCompanyRepository,
        IRoleIncludeService roleIncludeService
    ) : CommonJoinService<RoleXUserXCompany>(roleXUserXCompanyRepository),
        IRoleXUserXCompanyService
    {

        public async Task<IEnumerable<long>> GetRolesIdByUserIdCompanyIdAsync(long userId, long companyId, RoleXUserXCompanyQueryOptions? options = null)
        {
            return await roleXUserXCompanyRepository.GetListIdByUserIdAndCompanyIdAsync(userId, companyId, options);
        }

        public async Task<IEnumerable<long>> GetAllRolesIdByUserIdAndCompanyIdAsync(long userId, long companyId, RoleXUserXCompanyQueryOptions? options = null)
        {
            var rolesId = await GetRolesIdByUserIdCompanyIdAsync(userId, companyId, options);
            var allRolesId = await roleIncludeService.GetAllRolesIdByRolesIdAsync(rolesId);
            return allRolesId;
        }

        public async Task<IEnumerable<Company>> GetListCompaniesByUserIdAsync(long userId, RoleXUserXCompanyQueryOptions? options = null)
        {
            var companiesList = await roleXUserXCompanyRepository.GetListCompaniesByUserIdAsync(userId, options);
            return companiesList;
        }
    }
}
