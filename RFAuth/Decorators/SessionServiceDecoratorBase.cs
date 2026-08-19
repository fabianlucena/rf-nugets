using RFAuth.Entities;
using RFAuth.IServices;
using RFAuth.QueryOptions;
using RFBase.ILibs;
using RFServices.Decorators;

namespace RFAuth.Decorators
{
    public class SessionServiceDecoratorBase(ISessionService sessionService)
        : CreatableEntityServiceDecorator<Session>(sessionService),
        ISessionService
    {
        public virtual Task AddDataByIdAsync(long sessionId, string key, object value)
            => sessionService.AddDataByIdAsync(sessionId, key, value);

        public virtual Task CloseByIdAsync(long sessionId)
            => sessionService.CloseByIdAsync(sessionId);

        public virtual Task<Session> CreateAsync(long userId, long deviceId, string identityProvider, IDataDictionary? data = null)
            => sessionService.CreateAsync(userId, deviceId, identityProvider, data);

        public virtual Task<Session> DecorateAsync(Session session)
            => sessionService.DecorateAsync(session);

        public virtual Task<Session?> GetFirstOrDefaultByAuthorizationTokenAsync(string token, SessionQueryOptions? options = null)
            => sessionService.GetFirstOrDefaultByAuthorizationTokenAsync(token, options);

        public virtual Task<Session?> GetSingleOrDefaultByAuthorizationTokenAsync(string token, SessionQueryOptions? options = null)
            => sessionService.GetSingleOrDefaultByAuthorizationTokenAsync(token, options);

        public virtual Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null)
            => sessionService.GetFirstOrDefaultByAutoLoginTokenAsync(autoLoginToken, options);

        public virtual Task UpdateLastUsageAsync(long sessionId)
            => sessionService.UpdateLastUsageAsync(sessionId);
    }
}
