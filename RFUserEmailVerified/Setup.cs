using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.Exceptions;
using RFAuth.IServices;
using RFHttpAction.IServices;
using RFService.IServices;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified
{
    public static class Setup
    {
        public static void ConfigureRFUserEmailVerified(IServiceProvider provider)
        {
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();
            var userEmailVerifiedService = provider.GetRequiredService<IUserEmailVerifiedService>();
            propertiesDecorators.AddDecorator("LoginAttributes", async (data, newProperty, property) =>
            {
                var userEmail = await userEmailVerifiedService.GetSingleOrDefaultForUserIdAsync(((LoginData)data).UserId);
                if (userEmail == null)
                    newProperty["hasEmail"] = false;
                else
                {
                    newProperty["hasEmail"] = true;
                    newProperty["isEmailVerified"] = userEmail.IsVerified;
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

        public static void ConfigureDataConfigureRFUserEmailVerified(IServiceProvider provider)
            => ConfigureDataConfigureRFUserEmailVerifiedAsync(provider).Wait();

        public static async Task ConfigureDataConfigureRFUserEmailVerifiedAsync(IServiceProvider provider)
        {
            var addRolePermissionService = provider.GetService<IAddRolePermissionService>();
            if (addRolePermissionService != null)
            {
                var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                    { "user",  [
                        "passwordRecovery",
                    ] },

                    { "admin",  [
                        "passwordRecovery",
                    ] },
                };

                await addRolePermissionService.AddRolesPermissionsAsync(rolesPermissions);
            }
        }
    }
}
