using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface IUserTypeService
        : IService<UserType>,
            IServiceId<UserType>,
            IServiceUuid<UserType>,
            IServiceIdUuid<UserType>,
            IServiceName<UserType>,
            IServiceIdUuidName<UserType>
    {
    }
}
