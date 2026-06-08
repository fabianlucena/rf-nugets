using Microsoft.Extensions.DependencyInjection;
using RFAuth.Decorators;
using RFAuth.Entities;
using RFAuth.IServices;
using RFBase.Libs;
using RFRBAC.DTO;
using RFRBAC.IServices;

namespace RFRBAC.Decorators;

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

        var rpDataService = serviceProvider.GetRequiredService<IRPDataService>();
        var rpData = await rpDataService.GetSingleOrDefaultBySession(session);
        if (rpData is null)
            return session;

        session.Data ??= new DataDictionary();

        if (rpData.RoleIds is not null)
            session.Data["RoleIds"] = rpData.RoleIds;

        if (rpData.RoleNames is not null)
            session.Data["RoleNames"] = rpData.RoleNames;

        if (rpData.PermissionNames is not null)
            session.Data["PermissionNames"] = rpData.PermissionNames;

        var rpDataResponse = new RPDataResponse(rpData);
        if (rpDataResponse == null)
            return session;

        session.DataResponse ??= [];
        foreach (var item in rpDataResponse.Data)
            session.DataResponse[item.Key] = item.Value;

        return session;
    }
}
