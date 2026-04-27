using RFAuthEntities.Entities;
using RFBaseIRepositories.IRepositories;

namespace RFAuthIRepositories.Repositories
{
    public interface IUserPasswordRepository : INoIdEntityRepository<UserPassword>
    {
        Task<UserPassword> GetSingleByUserIdAsync(long userId);
        Task<UserPassword?> GetSingleOrDefaultByUserIdAsync(long userId);
    }
}