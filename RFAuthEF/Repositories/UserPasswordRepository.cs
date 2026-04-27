using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;

namespace RFAuthEF.Repositories
{
    public class UserPasswordRepository
        : NoIdEntityRepository<UserPassword>,
        IUserPasswordRepository
    {
        public UserPasswordRepository(DbContext context) : base(context) { }

        public async Task<UserPassword> GetSingleByUserIdAsync(long userId)
        {
            var table = context.Set<UserPassword>();
            var list = await table
                .Where(up => up.UserId == userId)
                .Take(2)
                .ToListAsync();

            if (list == null)
                throw new KeyNotFoundException($"UserPassword with userId '{userId}' not found.");

            if (list.Count > 1)
                throw new InvalidOperationException($"Multiple UserPassword entries found for userId '{userId}'.");

            if (list.Count == 0)
                throw new KeyNotFoundException($"UserPassword with userId '{userId}' not found.");

            return list[0];
        }

        public async Task<UserPassword?> GetSingleOrDefaultByUserIdAsync(long userId)
        {
            var table = context.Set<UserPassword>();
            var list = await table
                .Where(up => up.UserId == userId)
                .Take(2)
                .ToListAsync();

            if (list == null || list.Count == 0)
                return null;

            if (list.Count > 1)
                throw new InvalidOperationException($"Multiple UserPassword entries found for userId '{userId}'.");

            return list[0];
        }
    }
}
