using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;

namespace RFAuthIRepositories.Repositories
{
    public interface ISessionRepository : ICreatableEntityRepository<Session>
    {
        Task<Session?> GetFirstOrDefaultByTokenAsync(string token, SessionQueryOptions? options = null);
        Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null);
    }
}