using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFService.IServices;
using RFUserEmail.IServices;

namespace RFUserEmail
{
    public static class Setup
    {
        public static void ConfigureRFUserEmail(IServiceProvider provider)
        {
            var userEmailService = provider.GetRequiredService<IUserEmailService>();
            var mapper = provider.GetRequiredService<IMapper>();
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();

            propertiesDecorators.AddDecorator("LoginAttributes", async (data, property, eventName) => {
                var userEmail = await userEmailService.GetSingleOrDefaultForUserIdAsync(((LoginData)data).UserId);
                property["hasEmail"] = userEmail != null;
            });
        }
    }
}
