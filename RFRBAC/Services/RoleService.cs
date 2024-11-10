using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RoleService(
        IRepo<Role> repo
    ) : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<Role>, Role>(repo),
            IRoleService
    {
    }
}
