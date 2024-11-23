using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RoleService(
        IRepo<Role> repo,
        IUserRoleService userRoleService,
        IPropertiesDecorators propertiesDecorators
    ) : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<Role>, Role>(repo),
            IRoleService,
            IServiceDecorated
    {
        public IPropertiesDecorators PropertiesDecorators { get; } = propertiesDecorators;

        public async Task<IEnumerable<Role>> GetListForUserIdAsync(long userId, GetOptions? options)
        {
            var allRolesId = await userRoleService.GetAllRolesIdForUserIdAsync(userId);

            options ??= new GetOptions();
            options.Filters["Id"] = allRolesId;
            return await GetListAsync(options);
        }
    }
}
