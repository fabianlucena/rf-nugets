using RFAuth.Decorators;
using RFAuth.Entities;
using RFAuth.IServices;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified.Decorators;

public class LoginServiceDecorator(
    ILoginService _loginService,
    IUserEmailVerifiedService userEmailVerifiedService
)
    : LoginServiceDecoratorBase(_loginService),
    ILoginService
{
    public async Task<Session> DecorateSession(Session session)
    {
        var userEmail = await userEmailVerifiedService.GetSingleOrDefaultByUserIdAsync(session.UserId);
        if (userEmail == null)
            session.ResponseData["hasEmail"] = false;
        else
        {
            session.ResponseData["hasEmail"] = true;
            session.ResponseData["isEmailVerified"] = userEmail.IsVerified;
        }

        return session;
    }
}
