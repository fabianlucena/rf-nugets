using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices
{
    public interface IUserService : IACommonEntityService<User>
    {
        Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null);
        Task<User> GetOrCreateByUsernameAsync(string username, UserQueryOptions? options = null, Func<User, Task<User>>? createFactory = null);

        Task<long> GetSingleIdByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<long?> GetSingleIdOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<User> GetSystemUserAsync();

        Task<long> GetSystemUserIdAsync();

        Task<User> GetCurrentUserAsync();

        Task<long> GetCurrentUserIdAsync();

        Task<User> GetCurrentOrSystemUserAsync();

        Task<long> GetCurrentOrSystemUserIdAsync();

        Task UpdateLastLoginAtByUserIdAsync(long userId);

        Task<IEnumerable<string>> GetUsernamesByIdsAsync(IEnumerable<long> userIds, UserQueryOptions? options = null);
    }
}
