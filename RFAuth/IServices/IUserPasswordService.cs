using RFAuth.Entities;
using RFAuth.QueryOptions;
using RFBase.ILibs;
using RFEntities.Entities;

namespace RFAuth.IServices
{
    public interface IUserPasswordService
    {
        Task<UserPassword> GetSingleByUserIdAsync(long userId);
        Task<UserPassword?> GetSingleOrDefaultByUserIdAsync(long userId);
        Task<UserPassword> GetSingleByUserAsync(User user);
        Task<UserPassword?> GetSingleOrDefaultByUserAsync(User user);
        Task<int> UpdateByUserIdAsync(IDataDictionary data, long userId, UserPasswordQueryOptions? options = null);
        Task<bool> CreateIfNotExistsByUsernameAsync(string password, string username);
        Task<bool> CreateOrUpdateByUserIdAsync(string password, long userId);
        Task<bool> CreateOrUpdateByUsernameAsync(string password, string username);
        Task<bool> CreateOrUpdateByUserUuidAsync(string password, Guid userUuid);
        Task<bool> CheckPasswordByUserIdAsync(string password, long userId);
        Task<bool> ChangePasswordByUserIdAsync(string currentPassword, string newPassword, long userId);
    }
}
