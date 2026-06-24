using RFAuth.Decorators;
using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.IServices;
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

    public override async Task<Session> AutoLoginAsync(AutoLoginRequest request, SessionData? data = null)
    {
        var session = await loginService.AutoLoginAsync(request, data);
        session = await rpDataService.DecorateSession(session);
        return session;
    }

    public override async Task<Session> LoginAsync(LoginRequest request, SessionData? data = null)
    {
        var session = await loginService.LoginAsync(request, data);
        session = await rpDataService.DecorateSession(session);
        return session;
    }
}
