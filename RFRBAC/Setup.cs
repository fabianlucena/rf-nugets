using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFRBAC.DTO;
using RFRBAC.Entities;
using RFRBAC.IServices;
using RFService.Authorization;
using RFService.IServices;
using RFService.Libs;
using RFService.Repo;
using System.Reflection;

namespace RFRBAC
{
    public static class Setup
    {
        static IRoleService? roleService;
        static IPermissionService? permissionService;
        static IUserRoleService? userRoleService;
        static IRolePermissionService? rolePermissionService;
        static IMapper? mapper;
        static IPropertiesDecorators? propertiesDecorators;
        static IEventBus? eventBus;

        static IPermissionService PermissionService => permissionService ?? throw new Exception();
        static IUserRoleService UserRoleService => userRoleService ?? throw new Exception();
        static IRolePermissionService RolePermissionService => rolePermissionService ?? throw new Exception();

        public static void ConfigureRFRBAC(IServiceProvider provider)
        {
            roleService = provider.GetRequiredService<IRoleService>();
            permissionService = provider.GetRequiredService<IPermissionService>();
            userRoleService = provider.GetRequiredService<IUserRoleService>();
            rolePermissionService = provider.GetRequiredService<IRolePermissionService>();
            mapper = provider.GetRequiredService<IMapper>();
            propertiesDecorators = provider.GetRequiredService<IPropertiesDecorators>();
            eventBus = provider.GetRequiredService<IEventBus>();

            propertiesDecorators.AddDecorator("UserAttributes", async (data, property, eventType) => {
                var roles = await UserRoleService.GetRolesForUserIdAsync(((UserAttributes)data).Id);
                if (eventType == "Result")
                    property["roles"] = roles.Select(mapper.Map<Role, RoleResponse>);
                else
                    property["roles"] = roles;
            });

            propertiesDecorators.AddDecorator("LoginAttributes", async (data, property, eventType) => {
                var userId = ((LoginData)data).UserId;
                var roles = await UserRoleService.GetAllRolesForUserIdAsync(userId);
                var rolesId = roles.Select(i =>  i.Id);
                var permissionsName = (await RolePermissionService.GetPermissionsForRolesIdAsync(rolesId))
                    .Select(i => i.Name);
                property["roles"] = roles.Select(i => i.Name);
                property["permissions"] = permissionsName;
            });

            eventBus.AddListener("updated", "User", UpdateUserRoles);
            eventBus.AddListener("created", "User", UpdateUserRoles);

            ConfigureRFRBACAsync().Wait();
        }

        public static async Task<bool> UpdateUserRoles(Event evt)
        {
            if (userRoleService == null)
                return false;

            var data = evt.Data?.Data;
            if (data == null)
                return false;

            if (!DataValue.TryGetPropertyValue(data, "Username", out var usernameValue)
                || usernameValue is not string username
            )
                return false;

            if (!DataValue.TryGetPropertyValue(data, "Roles", out var rolesValue)
                || rolesValue is not List<object?> roles
            )
                return false;

            var rolesName = roles
                .Select(i => i?.ToString() ?? "")
                .Where(i => !string.IsNullOrEmpty(i))
                .ToArray();

            await userRoleService.UpdateRolesNameForUserNameAsync(username, rolesName);

            return true;
        }

        public static async Task ConfigureRFRBACAsync()
        {
            Type attributeType = typeof(PermissionAttribute);
            List<string> permissionsName = [];
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Module module in assembly.GetModules())
                {
                    Type[] types = module.GetTypes();
                    foreach (var type in types)
                    {
                        var classAttributes = type.GetCustomAttributes(attributeType, true);
                        foreach (var attr in classAttributes)
                        {
                            permissionsName
                                .AddRange(
                                    ((PermissionAttribute)attr).Permissions.Where(i => !permissionsName.Contains(i))
                                );
                        }

                        foreach (var method in type.GetMethods())
                        {
                            var methodAttributes = method.GetCustomAttributes(attributeType, true);
                            foreach (var attr in methodAttributes)
                            {
                                permissionsName
                                    .AddRange(
                                        ((PermissionAttribute)attr).Permissions.Where(i => !permissionsName.Contains(i))
                                    );
                            }
                        }

                        foreach (var property in type.GetProperties())
                        {
                            var propertyAttributes = property.GetCustomAttributes(attributeType, true);
                            foreach (var attr in propertyAttributes)
                            {
                                permissionsName
                                    .AddRange(
                                        ((PermissionAttribute)attr).Permissions.Where(i => !permissionsName.Contains(i))
                                    );
                            }
                        }
                    }
                }
            }

            var currentPermissionName = (await PermissionService
                .GetListAsync(new GetOptions { Filters = { { "Name", permissionsName } } }))
                .Select(i => i.Name);
            var newPermissionsName = permissionsName.Except(currentPermissionName);

            foreach (var permissionName in newPermissionsName)
            {
                await PermissionService.CreateAsync(new Permission{
                    Name = permissionName,
                    Title = permissionName,
                });
            }
        }
    }
}
