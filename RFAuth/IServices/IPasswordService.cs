using RFAuth.Entities;
using RFService.ILibs;
using RFService.IServices;
using RFService.Repo;

namespace RFAuth.IServices
{
    public interface IPasswordService
        : IServiceId<Password>
    {
        Task<Password> GetSingleForUserIdAsync(Int64 userId);

        Task<Password?> GetSingleOrDefaultForUserIdAsync(Int64 userId);

        Task<Password> GetSingleForUserAsync(User user);

        Task<Password?> GetSingleOrDefaultForUserAsync(User user);

        string Hash(string password);

        bool Verify(string rawPassword, string hash);

        bool Verify(string rawPassword, Password password);

        Task<int> UpdateForUserIdAsync(IDataDictionary data, Int64 userId, QueryOptions? options = null);

        Task<bool> CreateOrUpdateForUserIdAsync(string password, Int64 userId);

        Task<bool> CreateOrUpdateForUsernameAsync(string password, string username);
    }
}
