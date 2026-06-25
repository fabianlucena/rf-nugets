using RFAuth.Entities;
using RFAuth.QueryOptions;
using RFBase.ILibs;
using RFIServices.IServices;

namespace RFAuth.IServices
{
    public interface ISessionService : ICreatableEntityService<Session>
    {
        Task<Session> CreateAsync(long userId, long deviceId, IDataDictionary? data = null);
        Task<Session?> GetFirstOrDefaultByAuthorizationTokenAsync(string token, SessionQueryOptions? options = null);
        Task<Session?> GetSingleOrDefaultByAuthorizationTokenAsync(string token, SessionQueryOptions? options = null);
        Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null);
        Task UpdateLastUsageAsync(long sessionId);
        Task AddDataByIdAsync(long sessionId, string key, object value);
        Task CloseByIdAsync(long sessionId);
        Task<Session> DecorateAsync(Session session);
    }
}
