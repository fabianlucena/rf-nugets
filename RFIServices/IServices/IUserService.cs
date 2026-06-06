using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIServices.IServices
{
    public interface IUserService : ICommonEntityService<User>
    {
        Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null);
        Task<User> GetSingleByUsernameOrCreateAsync(string username, UserQueryOptions? options = null, Func<User, Task<User>>? completeCreateData = null);

        Task<long> GetSingleIdByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<long?> GetSingleIdOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null);

        Task<User> GetSystemUserAsync();

        Task<User> GetCurrentOrSystemUserAsync();

        Task<long> GetCurrentOrSystemUserIdAsync();

        Task UpdateLastLoginAtByUserIdAsync(long userId);

        Task<IEnumerable<string>> GetUsernamesByIdsAsync(IEnumerable<long> userIds, UserQueryOptions? options = null);
    }
}
