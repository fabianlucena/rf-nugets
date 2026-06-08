using Microsoft.Extensions.DependencyInjection;
using RFAuthControllers.Exceptions;
using RFHttpAction.IServices;
using RFRolesPermissions.IServices;
using RFServices.Attributes;
using RFUserEmailVerified.IServices;

namespace RFUserEmailVerified
{
    [SeedData(true)]
    public static class SeedInitialData
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
        public static async Task Run(IServiceProvider provider)
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
