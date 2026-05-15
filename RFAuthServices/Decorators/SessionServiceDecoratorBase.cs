using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIServices.IServices;
using RFBaseEntities.ILibs;
using RFBaseServices.Decorators;

namespace RFAuthServices.Decorators
{
    public class SessionServiceDecoratorBase(ISessionService sessionService)
        : CreatableEntityServiceDecorator<Session>(sessionService),
        ISessionService
    {
        public virtual Task AddDataByIdAsync(long sessionId, string key, object value)
            => sessionService.AddDataByIdAsync(sessionId, key, value);

        public virtual Task CloseByIdAsync(long sessionId)
            => sessionService.CloseByIdAsync(sessionId);

        public virtual Task<Session> CreateAsync(long userId, long deviceId, IDataDictionary? data = null)
            => sessionService.CreateAsync(userId, deviceId, data);

        public virtual Task<Session> DecorateAsync(Session session)
            => sessionService.DecorateAsync(session);

        public virtual Task<Session?> GetFirstOrDefaultByAutoLoginTokenAsync(string autoLoginToken, SessionQueryOptions? options = null)
            => sessionService.GetFirstOrDefaultByAutoLoginTokenAsync(autoLoginToken, options);

        public virtual Task<Session?> GetFirstOrDefaultByTokenAsync(string token, SessionQueryOptions? options = null)
            => sessionService.GetFirstOrDefaultByTokenAsync(token, options);

        public virtual Task UpdateLastUsageAsync(long sessionId)
            => sessionService.UpdateLastUsageAsync(sessionId);
    }
}
