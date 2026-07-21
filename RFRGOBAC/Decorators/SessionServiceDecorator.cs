using Microsoft.Extensions.DependencyInjection;
using RFAuth.Decorators;
using RFAuth.Entities;
using RFAuth.IServices;
using RFBase.Libs;
using RFRGOBAC.DTO;
using RFRGOBAC.IServices;

namespace RFRGOBAC.Decorators;

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

        var orpgDataService = serviceProvider.GetRequiredService<IORGPDataService>();
        var orpgData = await orpgDataService.GetSingleOrDefaultBySession(session);
        if (orpgData is null)
            return session;

        session.Data ??= new DataDictionary();
        session.Data["Organizations"] = orpgData.Organizations;
        session.Data["CurrentOrganization"] = orpgData.CurrentOrganization;

        if (orpgData.GroupIds is not null)
            session.Data["GroupIds"] = orpgData.GroupIds;

        if (orpgData.GroupIds is not null)
            session.Data["GroupNames"] = orpgData.GroupNames;

        if (orpgData.RoleIds is not null)
            session.Data["RoleIds"] = orpgData.RoleIds;

        if (orpgData.RoleNames is not null)
            session.Data["RoleNames"] = orpgData.RoleNames;

        if (orpgData.PermissionNames is not null)
            session.Data["PermissionNames"] = orpgData.PermissionNames;

        var sessionDataResponse = new ORPGDataResponse(orpgData);
        if (sessionDataResponse == null)
            return session;

        foreach (var item in sessionDataResponse.Data)
            session.ResponseData[item.Key] = item.Value;

        return session;
    }
}
