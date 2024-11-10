using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface IUserService : IService<User>
    {
        Task<User> GetSingleForUsernameAsync(string username);

        Task<User?> GetSingleOrDefaultForUsernameAsync(string username);
    }
}
