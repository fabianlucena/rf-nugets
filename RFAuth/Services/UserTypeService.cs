using RFAuth.Entities;
using RFAuth.IServices;
using RFService.Services;
using RFService.IRepo;

namespace RFAuth.Services
{
    public class UserTypeService(IRepo<UserType> repo)
        : ServiceTimestampsIdUuidEnabledNameTitleTranslatable<IRepo<UserType>, UserType>(repo),
            IUserTypeService
    {
    }
}
