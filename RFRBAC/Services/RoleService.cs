using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IRepo;
using RFService.IServices;
using RFService.Services;

namespace RFRBAC.Services
{
    public class RoleService(
        IRepo<Role> repo,
        IPropertiesDecorators propertiesDecorators
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<Role>, Role>(repo),
            IRoleService,
            IServiceDecorated
    {
        public IPropertiesDecorators PropertiesDecorators { get; } = propertiesDecorators;
    }
}
