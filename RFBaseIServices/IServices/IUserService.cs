using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface IUserService : ICommonEntityService<User>
    {
        string HashPassword(string password);
        bool CheckPassword(User user, string password);

        Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<User> GetSystemUserAsync();

        Task<User> GetCurrentOrSystemUserAsync();

        Task<long> GetCurrentOrSystemUserIdAsync();

        Task UpdateLastLoginAtByUserIdAsync(long userId);
    }
}
