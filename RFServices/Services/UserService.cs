using Microsoft.AspNetCore.Http;
using RFBase.Libs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFServices.Attributes;

namespace RFServices.Services;

[RegisterService]
public class UserService(
    IUserRepository userRepository,
    IHttpContextAccessor contextAccessor
)
    : CommonEntityService<User>(userRepository),
    IUserService
{
    public async Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null)
        => await GetSingleAsync(new UserQueryOptions(options) { Username = username });

    public async Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null)
        => await GetSingleOrDefaultAsync(new UserQueryOptions(options) { Username = username });

    public async Task<long?> GetSingleIdOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null)
        => await GetSingleIdOrDefaultAsync(new UserQueryOptions(options) { Username = username });

    public async Task<User> GetSystemUserAsync()
        => await GetSingleByUsernameAsync("system");

    public async Task<User> GetCurrentOrSystemUserAsync()
    {
        var items = contextAccessor.HttpContext?.Items;
        if (items?.TryGetValue("CurrentUser", out var currentUserData) == true
            && currentUserData is User currentUser
            && currentUser is not null
        )
        {
            return currentUser;
        }

        return await GetSystemUserAsync();
    }

    public async Task<long> GetCurrentOrSystemUserIdAsync()
    {
        var items = contextAccessor.HttpContext?.Items;
        if (items?.TryGetValue("CurrentUserId", out var idCurrentUserData) == true
            && idCurrentUserData is long idCurrentUser
            && idCurrentUser > 0
        )
        {
            return idCurrentUser;
        }
     
        var systemUser = await GetSystemUserAsync();
        return systemUser.Id;
    }

    public async Task UpdateLastLoginAtByUserIdAsync(long userId)
    {
        var data = new DataDictionary
        {
            { "LastLoginAt", DateTime.UtcNow },
            { "UpdatedById", userId },
        };

        await UpdateByIdAsync(userId, data);
    }

    public async Task<long> GetSingleIdByUsernameAsync(string username, UserQueryOptions? options = null)
        => await GetSingleIdAsync(new UserQueryOptions(options) { Username = username });

    public async Task<IEnumerable<string>> GetUsernamesByIdsAsync(IEnumerable<long> userIds, UserQueryOptions? options = null)
    {
        options = (UserQueryOptions?)(options?.Clone() ?? new UserQueryOptions());
        options!.Ids = userIds;
        return await userRepository.GetUsernamesAsync(options);
    }

    public async Task<User> GetSingleByUsernameOrCreateAsync(string username, UserQueryOptions? options = null, Func<User, Task<User>>? completeCreateData = null)
    {
        var user = await GetSingleOrDefaultByUsernameAsync(username, options);
        if (user != null)
            return user;

        user = new User { Username = username };
        if (completeCreateData != null)
            user = await completeCreateData(user);

        user = await CreateAsync(user);

        return user;
    }
}
