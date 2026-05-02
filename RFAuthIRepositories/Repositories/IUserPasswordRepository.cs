using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;

namespace RFAuthIRepositories.Repositories
{
    public interface IUserPasswordRepository : INoIdEntityRepository<UserPassword>
    {
    }
}