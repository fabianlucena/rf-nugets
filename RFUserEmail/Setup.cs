using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.Exceptions;
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
                property["hasEmail"] = userEmail != null;
            });
        }
    }
}
