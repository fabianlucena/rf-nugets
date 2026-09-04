using Microsoft.Extensions.DependencyInjection;
using RFAuth.Decorators;
using RFAuth.Entities;
using RFAuth.IServices;
using RFBase.Libs;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.IServices;

namespace RFRGOBAC.Decorators;

[RegisterDecorator]
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
        session.InternalData["Organizations"] = session.Data["Organizations"];
        session.InternalData["CurrentOrganization"] = session.Data["CurrentOrganization"];

        if (orpgData.GroupsId is not null)
            session.Data["GroupsId"] = orpgData.GroupsId;

        if (orpgData.GroupsName is not null)
            session.Data["GroupsName"] = orpgData.GroupsName;

        if (orpgData.RolesId is not null)
            session.Data["RolesId"] = orpgData.RolesId;

        if (orpgData.RolesName is not null)
            session.Data["RolesName"] = orpgData.RolesName;

        if (orpgData.PermissionsName is not null)
            session.Data["PermissionsName"] = orpgData.PermissionsName;
            
        var sessionDataResponse = new ORPGDataResponse(orpgData);
        if (sessionDataResponse == null)
            return session;

        foreach (var item in sessionDataResponse.Data)
            session.ResponseData[item.Key] = item.Value;

        return session;
    }
}
