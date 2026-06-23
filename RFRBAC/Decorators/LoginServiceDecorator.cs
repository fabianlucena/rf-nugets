using RFAuth.Decorators;
using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.IServices;
using RFBase.ILibs;
using RFBase.Libs;
using RFRBAC.DTO;
using RFRBAC.IServices;
using RFRegisterService.Attributes;

namespace RFRBAC.Decorators;

[RegisterDecorator]
public class LoginServiceDecorator(
    ILoginService _loginService,
    IRPDataService rpDataService
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

        var sessionData = await rpDataService.GetSingleOrDefaultBySession(session);
        if (sessionData is null)
            return session;

        session.Data ??= new DataDictionary();

        if (sessionData.RoleIds is not null)
            session.Data["RoleIds"] = sessionData.RoleIds;

        if (sessionData.RoleNames is not null)
            session.Data["RoleNames"] = sessionData.RoleNames;

        if (sessionData.PermissionNames is not null)
            session.Data["PermissionNames"] = sessionData.PermissionNames;

        var sessionDataResponse = new RPDataResponse(sessionData);
        if (sessionDataResponse == null)
            return session;

        session.DataResponse ??= [];
        foreach (var item in sessionDataResponse.Data)
            session.DataResponse[item.Key] = item.Value;

        return session;
    }
}
