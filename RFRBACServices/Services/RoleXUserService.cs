using RFBaseServices.Services;
using RFRBACEntities.Entities;
using RFRBACEntities.QueryOptions;
using RFRBACIRepositories.IRepositories;
using RFRBACIServices.IServices;
using System.ComponentModel.Design;

namespace RFRBACServices.Services
{
    public class RoleXUserService(
        IRoleXUserRepository roleXUserRepository,
        IRoleIncludeService roleIncludeService
    ) : CommonJoinService<RoleXUser>(roleXUserRepository),
        IRoleXUserService
    {
        public async Task<IEnumerable<long>> GetListRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            return await roleXUserRepository.GetListRolesIdByUserIdAsync(userId, options);
        }

        public async Task<IEnumerable<long>> GetAllRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        {
            var rolesId = await GetListRolesIdByUserIdAsync(userId, options);
            var allRolesId = await roleIncludeService.GetAllRolesIdByRolesIdAsync(rolesId);
            return allRolesId;
        }
    }
}
