using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RoleService(
        IRepo<Role> repo,
        IUserRoleService userRoleService
    ) : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<Role>, Role>(repo),
            IRoleService
    {
        public async Task<IEnumerable<Role>> GetListForUserIdAsync(long userId, GetOptions? options)
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);

            options ??= new GetOptions();
            options.Filters["Id"] = allRolesId;
            return await GetListAsync(options);
        }
    }
}
