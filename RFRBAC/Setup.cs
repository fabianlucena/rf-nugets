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
            eventBus = provider.GetService<IEventBus>();

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
                var rolesId = roles.Select(i => i.Id);
                var permissions = await RolePermissionService.GetPermissionsForRolesIdAsync(rolesId);

                property["roles"] = roles.Select(i => i.Name);
                property["permissions"] = permissions.Select(i => i.Name);
            });

            if (eventBus != null)
            {
                eventBus.AddListener("updated", "User", UpdateUserRoles);
                eventBus.AddListener("created", "User", UpdateUserRoles);
            }
        }

#pragma warning disable IDE0060 // Remove unused parameter
        public static void ConfigureDataRFRBAC(IServiceProvider provider)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            ConfigureRFRBACAsync().Wait();
        }

        public static async Task<bool> UpdateUserRoles(Event evt)
        {
            if (userRoleService == null || evt.Data == null)
                return false;

            if (evt.Data is not DataDictionary bundle)
                return false;

            if (!bundle.TryGet("Data", out DataDictionary data))
                return false;

            if (data == null)
                return false;

            if (!data.TryGetNotNullString("Username", out var username))
                return false;

            if (!data.TryGetNotNullStrings("Roles", out var rolesName))
                return false;

            await userRoleService.UpdateRolesNameForUserNameAsync(rolesName, username);

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
