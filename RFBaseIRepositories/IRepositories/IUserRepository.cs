using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface IUserRepository : ICommonEntityRepository<User>
    {
        Task<User> GetSingleByUsernameAsync(string username, UserQueryOptions? options = null);
        Task<User?> GetSingleOrDefaultByUsernameAsync(string username, UserQueryOptions? options = null);
    }
}
