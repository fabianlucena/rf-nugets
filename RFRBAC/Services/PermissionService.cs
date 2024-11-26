using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.Services;

namespace RFRBAC.Services
{
    public class PermissionService(
        IRepo<Permission> repo
    ) : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<Permission>,
        Permission>(repo),
        IPermissionService
    {
    }
}
