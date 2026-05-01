using Microsoft.Extensions.DependencyInjection;
using RFAuthEntities.Entities;
using RFAuthIServices.IServices;
using RFAuthServices.Decorators;
using RFBaseEntities.Libs;
using RFRGOBACIServices.DTO;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Decorators
{
    public class SessionServiceDecorator(
        ISessionService _sessionService,
        IServiceProvider serviceProvider
    )
        : SessionServiceDecoratorBase(_sessionService),
        ISessionService
    {
        private readonly ISessionService sessionService = _sessionService;

        public override async Task<Session> DecorateAsync(Session session)
        {
            session = await sessionService.DecorateAsync(session);
            session = await DecorateSession(session);
            return session;
        }

        public async Task<Session> DecorateSession(Session session)
        {
            session = new Session(session);

            var sessionDataService = serviceProvider.GetRequiredService<ISessionDataService>();
            var sessionData = await sessionDataService.GetSingleOrDefaultBySession(session);
            if (sessionData is null)
                return session;

            var sessionDataResponse = new SessionDataResponse(sessionData);
            if (sessionDataResponse == null)
                return session;

            session.DataResponse ??= [];
            foreach (var item in sessionDataResponse.Data)
                session.DataResponse[item.Key] = item.Value;

            return session;
        }
    }
}
