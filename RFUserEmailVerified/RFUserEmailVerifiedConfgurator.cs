using Microsoft.Extensions.DependencyInjection;
using RFAuthControllers.Exceptions;
using RFHttpAction.IServices;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified;

public static class RFUserEmailVerifiedConfigurator
{
    public static void ConfigureRFUserEmailVerified(IServiceProvider provider)
    {
        var userEmailVerifiedService = provider.GetRequiredService<IUserEmailVerifiedService>();
        
        var actionListeners = provider.GetRequiredService<IHttpActionListeners>();
        actionListeners.AddListener("userEmail.verify", async token =>
        {
            if (string.IsNullOrEmpty(token.Data))
                throw new NoAuthorizationHeaderException();

            var userEmailId = long.Parse(token.Data);
            if (userEmailId == 0)
                throw new NoAuthorizationHeaderException();

            await userEmailVerifiedService.SetIsVerifiedByIdAsync(true, userEmailId);
        });
    }
}
