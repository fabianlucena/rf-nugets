using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFRBAC.DTO;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.IServices;
using RFService.Services;
using System.Text.Json;

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

            if (GetPropertyValue(data, "username") is not string username)
                return false;

            List<object?>? rolesValue = GetPropertyValue(data, "roles");
            if (rolesValue == null)
                return false;

            var rolesName = rolesValue
                .Select(i => i?.ToString() ?? "")
                .Where(i => !string.IsNullOrEmpty(i))
                .ToArray();

            await userRoleService.UpdateRolesNameForUserNameAsync(username, rolesName);

            return true;
        }

        public static dynamic? GetPropertyValue(dynamic obj, string prop)
        {
            var value = obj[prop];
            if (value.GetType() == typeof(JsonElement))
                return GetValue(value);

            return value;
        }

        public static dynamic? GetValue(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.String: return element.GetString();
                case JsonValueKind.Array: {
                    List<object?> result = [];
                    foreach (var item in element.EnumerateArray())
                        result.Add(GetValue(item));

                    return result;
                }
            }

            throw new Exception("Valor no implementado");
        }
    }
}
