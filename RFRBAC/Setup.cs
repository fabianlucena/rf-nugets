using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFRBAC.DTO;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IServices;
using RFService.Services;

namespace RFRBAC
{
    public static class Setup
    {
        static IUserRoleService? userRoleService;

        public static void ConfigureRFRBAC(IServiceProvider provider)
        {
            var roleService = provider.GetRequiredService<IRoleService>();
            userRoleService = provider.GetRequiredService<IUserRoleService>();
            var mapper = provider.GetRequiredService<IMapper>();
            var propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();
            var eventBus = provider.GetRequiredService<IEventBus>();

            propertiesDecorators.AddDecorator("UserAttributes", async (data, property, eventType) => {
                var roles = await userRoleService.GetRolesForUserIdAsync(((UserAttributes)data).Id);
                if (eventType == "Result")
                    property["roles"] = roles.Select(mapper.Map<Role, RoleResponse>);
                else
                    property["roles"] = roles;
            });

            eventBus.AddListener("updated", "User", UserUpdated);
        }

        public static async Task<bool> UserUpdated(Event evt)
        {
            if (userRoleService == null)
                return false;

            var data = evt.Data?.Data;
            if (data == null)
                return false;

            if (DataValue.GetPropertyValue(data, "username") is not string username)
                return false;

            List<object?>? rolesValue = DataValue.GetPropertyValue(data, "roles");
            if (rolesValue == null)
                return false;

            var rolesName = rolesValue
                .Select(i => i?.ToString() ?? "")
                .Where(i => !string.IsNullOrEmpty(i))
                .ToArray();

            await userRoleService.UpdateRolesNameForUserNameAsync(username, rolesName);

            return true;
        }
    }
}
