using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface IUserTypeService
        : IService<UserType>,
            IServiceName<UserType>
    {
    }
}
