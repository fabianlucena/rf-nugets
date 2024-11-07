using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface IUserService : IService<User>
    {
        Task<User> GetSingleForUsernameAsync(string username);

        Task<User?> GetSingleOrDefaultForUsernameAsync(string username);
    }
}
