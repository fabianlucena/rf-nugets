using RFAuth.Decorators;
using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.IServices;
using RFBase.ILibs;
using RFBase.Libs;
using RFRGOBAC.DTO;
using RFRGOBAC.IServices;

namespace RFRGOBAC.Decorators;

public class LoginServiceDecorator(
    ILoginService _loginService,
    IORGPDataService orpgDataService
)
    : LoginServiceDecoratorBase(_loginService),
    ILoginService
{
    private readonly ILoginService loginService = _loginService;

    public override async Task<Session> AutoLoginAsync(AutoLoginRequest request, IDataDictionary? data = null)
    {
        var session = await loginService.AutoLoginAsync(request, data);
        session = await DecorateSession(session);
        return session;
    }

    public override async Task<Session> LoginAsync(LoginRequest request, IDataDictionary? data = null)
    {
        var session = await loginService.LoginAsync(request, data);
        session = await DecorateSession(session);
        return session;
    }

    public async Task<Session> DecorateSession(Session session)
    {
        session = new Session(session);

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

        session.DataResponse ??= [];
        foreach (var item in sessionDataResponse.Data)
            session.DataResponse[item.Key] = item.Value;

        return session;
    }
}
