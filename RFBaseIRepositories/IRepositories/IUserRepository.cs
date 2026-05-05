using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIRepositories.IRepositories
{
    public interface IUserRepository : ICommonEntityRepository<User>
    {
        Task<IEnumerable<string>> GetUsernamesAsync(UserQueryOptions options);
    }
}
