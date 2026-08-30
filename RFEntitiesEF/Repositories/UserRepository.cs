using Microsoft.EntityFrameworkCore;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.QueryOptions;
using RFRegisterService.Attributes;

namespace RFEntitiesEF.Repositories;

[RegisterService]
public class UserRepository(DbContext context)
    : CommonEntityRepository<User>(context),
    IUserRepository
{
    public override IQueryable<User> CreateDBSet(BaseQueryOptions? options = null)
    {
        var queryable = base.CreateDBSet(options);

        if (options is UserQueryOptions userOptions)
        {
            if (userOptions.Username != null)
                queryable = queryable.Where(u => u.Username == userOptions.Username);

            if (userOptions.TypeUuid != null)
                queryable = queryable
                    .Include(u => u.Type)
                    .Where(u => u.Type!.Uuid == userOptions.TypeUuid);

            if (userOptions.IncludeType)
                queryable = queryable.Include(u => u.Type);
        }

        return queryable;
    }

    public async Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null)
    {
        return await GetSingleOrDefaultByUsernameAsync(username, options)
            ?? throw new KeyNotFoundException($"User with username '{username}' not found.");
    }

    public async Task<long> GetSingleIdByUsernameAsync(string username, UserQueryOptions? options = null)
    {
        options ??= new UserQueryOptions();
        options.Take = 2;
        var set = GetDBSet(options);

        var list = await set
            .Where(u => u.Username == username)
            .Select(u => u.Id)
            .ToListAsync();

        if (list.Count == 0)
            throw new KeyNotFoundException($"User with username '{username}' not found.");

        if (list.Count > 1)
            throw new InvalidOperationException($"Multiple users with username '{username}' found.");
        
        return list[0];
    }

    public async Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null)
    {
        options ??= new UserQueryOptions();
        options.Take = 2;
        var set = GetDBSet(options);

        var list = await set
            .Where(u => u.Username == username)
            .ToListAsync();

        if (list.Count == 0)
            return null;

        if (list.Count > 1)
        {
            throw new InvalidOperationException($"Multiple users with username '{username}' found.");
        }

        return list[0];
    }

    public async Task<IEnumerable<string>> GetUsernamesAsync(UserQueryOptions options)
    {
        return await GetDBSet(options)
            .Select(u => u.Username)
            .ToListAsync();
    }
}
