using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface IUserTypeService : IService<UserType>, IServiceName<UserType>
    {
    }
}
