using RFEntities.Entities;
using RFIServices.QueryOptions;

namespace RFIRepositories.IRepositories
{
    public interface IUserRepository : ICommonEntityRepository<User>
    {
        Task<IEnumerable<string>> GetUsernamesAsync(UserQueryOptions options);
    }
}
