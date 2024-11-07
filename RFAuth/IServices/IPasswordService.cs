using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface IPasswordService : IServiceId<Password>
    {
        Task<Password> GetSingleForUserIdAsync(Int64 userId);

        Task<Password?> GetSingleOrDefaultForUserIdAsync(Int64 userId);

        Task<Password> GetSingleForUserAsync(User user);

        Task<Password?> GetSingleOrDefaultForUserAsync(User user);

        string Hash(string password);

        bool Verify(string rawPassword, string hash);

        bool Verify(string rawPassword, Password password);
    }
}
