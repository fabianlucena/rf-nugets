using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.Exceptions;
using RFHttpAction.IServices;
using RFService.IService;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified
{
    public static class Setup
    {
        public static void ConfigureRFUserEmailVerified(IServiceProvider provider)
        {
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();
            var userEmailVerifiedService = provider.GetRequiredService<IUserEmailVerifiedService>();
            propertiesDecorators.AddDecorator("LoginAttributes", async (data, property) =>
            {
                var userEmail = await userEmailVerifiedService.GetSingleOrDefaultForUserIdAsync(((LoginData)data).UserId);
                if (userEmail == null)
                    property["hasEmail"] = false;
                else
                {
                    property["hasEmail"] = true;
                    property["isEmailVerified"] = userEmail.IsVerified;
                }
            });

            var actionListeners = provider.GetRequiredService<IHttpActionListeners>();
            actionListeners.AddListener("userEmail.verify", async token =>
            {
                if (string.IsNullOrEmpty(token.Data))
                    throw new NoAuthorizationHeaderException();

                var userEmailId = long.Parse(token.Data);
                if (userEmailId == 0)
                    throw new NoAuthorizationHeaderException();

                await userEmailVerifiedService.SetIsVerifiedForIdAsync(true, userEmailId);
            });
        }
    }
}
