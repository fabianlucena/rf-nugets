using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.Exceptions;
using RFHttpAction.IServices;
using RFService.IService;
using RFUserEmail.IServices;

namespace RFUserEmail
{
    public static class Setup
    {
        public static void ConfigureRFUserEmail(IServiceProvider provider)
        {
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();
            var userEmailService = provider.GetRequiredService<IUserEmailService>();
            propertiesDecorators.AddDecorator("LoginAttributes", async (data, property) => {
                var userEmail = await userEmailService.GetSingleOrDefaultForUserIdAsync(((LoginData)data).UserId);
                if (userEmail == null)
                    property["hasEmail"] = false;
                else
                {
                    property["hasEmail"] = true;
                    property["isEmailVerified"] = userEmail.IsVerified;
                }
            });

            var actionListeners = provider.GetRequiredService<IHttpActionListeners>();
            actionListeners.AddListener("userEmail.verify", async token => {
                if (string.IsNullOrEmpty(token.Data))
                    throw new NoAuthorizationHeaderException();

                var userEmailId = Int64.Parse(token.Data);
                if (userEmailId == 0)
                    throw new NoAuthorizationHeaderException();

                await userEmailService.SetIsVerifiedForIdAsync(true, userEmailId);
            });
        }
    }
}
