using RFRBAC.Entities;
using RFService.IServices;
using RFService.Repo;

namespace RFRBAC.IServices
{
    public interface IRoleService
        : IService<Role>, IServiceName<Role>, IServiceDecorated
    {
        Task<IEnumerable<Role>> GetListForUserIdAsync(Int64 userId, GetOptions? options = null);
    }
}
