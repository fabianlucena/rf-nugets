using RFAuth.Decorators;
using RFAuth.Entities;
using RFAuth.IServices;
using RFRBAC.IServices;

namespace RFRBAC.Decorators;

public class SessionServiceDecorator(
    ISessionService _sessionService,
    IRPDataService rpDataService
)
    : SessionServiceDecoratorBase(_sessionService),
    ISessionService
{
    private readonly ISessionService sessionService = _sessionService;

    public override async Task<Session> DecorateAsync(Session session)
    {
        session = await sessionService.DecorateAsync(session);
        session = await rpDataService.DecorateSession(session);
        return session;
    }
}
