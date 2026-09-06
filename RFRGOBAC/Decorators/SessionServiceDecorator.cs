using Microsoft.Extensions.DependencyInjection;
using RFAuth.Decorators;
using RFAuth.Entities;
using RFAuth.IServices;
using RFBase.Libs;
using RFRegisterService.Attributes;
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
        session.InternalData["Organizations"] = orpgData.Organizations;
        session.ResponseData["organizations"] = orpgData.Organizations;
        session.InternalData["CurrentOrganization"] = orpgData.CurrentOrganization;
        session.ResponseData["currentOrganization"] = orpgData.CurrentOrganization;

        if (orpgData.GroupsId is not null)
            session.InternalData["GroupsId"] = new List<long>([.. (session.InternalData.GetInt64List("GroupsId")), .. orpgData.GroupsId]);

        if (orpgData.RolesId is not null)
            session.InternalData["RolesId"] = new List<long>([.. (session.InternalData.GetInt64List("RolesId")), .. orpgData.RolesId]);

        if (orpgData.GroupsName is not null)
        {
            session.InternalData["GroupsName"] = new List<string>([.. (session.InternalData.GetNotNullOrEmptyStrings("GroupsName")), .. orpgData.GroupsName]);
            session.ResponseData["groups"] = new List<string>([.. (session.ResponseData.GetNotNullOrEmptyStrings("GroupsName")), .. orpgData.GroupsName]);
        }

        if (orpgData.RolesName is not null)
        {
            session.InternalData["RolesName"] = new List<string>([.. (session.InternalData.GetNotNullOrEmptyStrings("RolesName")), .. orpgData.RolesName]);
            session.ResponseData["roles"] = new List<string>([.. (session.ResponseData.GetNotNullOrEmptyStrings("RolesName")), .. orpgData.RolesName]);
        }

        if (orpgData.PermissionsName is not null)
        {
            session.InternalData["PermissionsName"] = new List<string>([.. (session.InternalData.GetNotNullOrEmptyStrings("PermissionsName")), .. orpgData.PermissionsName]);
            session.ResponseData["permissions"] = new List<string>([.. (session.ResponseData.GetNotNullOrEmptyStrings("PermissionsName")), .. orpgData.PermissionsName]);
        }

        return session;
    }
}
