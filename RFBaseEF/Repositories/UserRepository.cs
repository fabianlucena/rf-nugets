using Microsoft.EntityFrameworkCore;
using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;

namespace RFBaseEF.Repositories
{
    public class UserRepository
        : CommonEntityRepository<User>,
        IUserRepository
    {
        public UserRepository(DbContext context) : base(context) { }

        public override IQueryable<User> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is UserQueryOptions userOptions)
            {
                if (userOptions.Username != null)
                    queryable = queryable.Where(u => u.Username == userOptions.Username);
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
    }
}
