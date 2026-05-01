using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFBaseEntities.ILibs;
using RFBaseIServices.IServices;

namespace RFAuthIServices.IServices
{
    public interface ISessionService : ICreatableEntityService<Session>
    {
        Task<Session> CreateAsync(long userId, long deviceId, IDataDictionary? data = null);
        Task<Session?> GetFirstOrDefaultByTokenAsync(string token, SessionQueryOptions? options = null);
        Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null);
        Task UpdateLastUsageAsync(long sessionId);
        Task AddDataByIdAsync(long sessionId, string key, object value);
        Task CloseByIdAsync(long sessionId);
        Task<Session> DecorateAsync(Session session);
    }
}
