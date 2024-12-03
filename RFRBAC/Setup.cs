using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.IServices;
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
        static IUserService? userService;
        static IRoleService? roleService;
        static IUserRoleService? userRoleService;
        static IPermissionService? permissionService;
        static IRolePermissionService? rolePermissionService;
        static IMapper? mapper;
        static IPropertiesDecorators? propertiesDecorators;
        static IEventBus? eventBus;

        static IUserService UserService => userService ?? throw new Exception();
        static IRoleService RoleService => roleService ?? throw new Exception();
        static IUserRoleService UserRoleService => userRoleService ?? throw new Exception();
        static IPermissionService PermissionService => permissionService ?? throw new Exception();
        static IRolePermissionService RolePermissionService => rolePermissionService ?? throw new Exception();

        public static void ConfigureRFRBAC(IServiceProvider provider)
        {
            userService = provider.GetRequiredService<IUserService>();
            roleService = provider.GetRequiredService<IRoleService>();
            userRoleService = provider.GetRequiredService<IUserRoleService>();
            permissionService = provider.GetRequiredService<IPermissionService>();
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
                var permissions = await RolePermissionService.GetPermissionsForRolesIdAsync(rolesId);
                
                property["roles"] = roles.Select(i => i.Name);
                property["permissions"] = permissions.Select(i => i.Name);
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
            var role = await RoleService.GetSingleOrDefaultForNameAsync("admin")
                ?? await RoleService.CreateAsync(new Role
                {
                    Name = "admin",
                    Title = "Administrator",
                    IsSelectable = true,
                    IsTranslatable = true,
                });

            var roleAdminId = role.Id;
            var userAdminId = await UserService.GetSingleIdForUsernameAsync("admin");
            if (!await UserRoleService.UserIdHasRoleIdAsync(userAdminId, roleAdminId))
                await UserRoleService.CreateAsync(new UserRole{
                    UserId = userAdminId,
                    RoleId = roleAdminId,
                });

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
