using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.IServices;
using RFService.IServices;
using RFService.Libs;

namespace RFAuth
{
    public static class Setup
    {
        static ILoginService? loginService;
        static IUserTypeService? userTypeService;
        static IUserService? userService;
        static IDeviceService? deviceService;
        static IMapper? mapper;
        static IEventBus? eventBus;
        
        public static void ConfigureRFAuth(IServiceProvider provider)
        {
            loginService = provider.GetService<ILoginService>() ??
                throw new Exception("Can't get ILoginService.");

            userTypeService = provider.GetService<IUserTypeService>() ??
                throw new Exception("Can't get IUserTypeService.");

            userService = provider.GetService<IUserService>() ??
                throw new Exception("Can't get IUserService.");

            deviceService = provider.GetService<IDeviceService>() ??
                throw new Exception("Can't get IDeviceService.");

            mapper = provider.GetService<IMapper>() ??
                throw new Exception("Can't get IMapper.");

            eventBus = provider.GetService<IEventBus>();

            eventBus = provider.GetService<IEventBus>();
            eventBus?.AddListener("login", LoginHandler);
        }

        public static void ConfigureDataRFAuth(IServiceProvider provider)
            => ConfigureDataRFAuthAsync(provider).Wait();

        public static async Task ConfigureDataRFAuthAsync(IServiceProvider provider)
        {
            var userType = await userTypeService!.GetOrCreateAsync(new UserType
            {
                Name = "user",
                Title = "User",
                IsTranslatable = true,
            });

            var user = await userService!.GetOrCreateAsync(new User
            {
                TypeId = userType.Id,
                Username = "admin",
                FullName = "Administrador",
            });

            var passwordService = provider.GetService<IPasswordService>() ??
                throw new Exception("Can't get IPasswordService.");

            await passwordService.CreateIfNotExistsAsync(new Password
            {
                UserId = user.Id,
                Hash = "$2a$11$fRe./FCGyNjS9Vao3IIBlOiVCx3C05NRBNFrHhVk32Qdw75Ia.Y5S",
            });

            var addRolePermissionService = provider.GetService<IAddRolePermissionService>();
            if (addRolePermissionService != null)
            {
                var rolesPermissions = new Dictionary<string, IEnumerable<string>>{
                    { "user",  [
                        "changePassword",
                    ] },

                    { "admin",  [
                        "changePassword",
                        "user.get", "user.add", "user.edit", "user.delete", "user.restore",
                    ] },
                };

                await addRolePermissionService.AddRolesPermissionsAsync(rolesPermissions);
            }
        }

        public static async Task<bool> LoginHandler(Event evt)
        {
            if (evt.Data is not DataDictionary bundle)
                return false;

            if (!bundle.TryGet("Data", out DataDictionary data))
                return false;

            if (data == null)
                return false;

            if (!data.TryGetNotNullString("Username", out var username))
                return false;

            data.TryGetNotNullString("DeviceToken", out var deviceToken);

            var user = await userService!.GetSingleOrDefaultForUsernameAsync(username);
            if (user == null)
            {
                if (!(data.GetBool("SelfServiceRegistration") ?? false))
                    return false;

                user = await userService.CreateAsync(new User
                {
                    Username = username,
                    FullName = data.GetString("FullName") ?? username,
                    TypeId = (await userTypeService!.GetOrCreateAsync(new UserType
                    {
                        Name = "user",
                        Title = "User",
                        IsTranslatable = true,
                    })).Id,
                });
            }

            var device = await deviceService!.GetSingleForTokenOrCreateAsync(deviceToken);
            if (device == null)
                return false;

            var session = await loginService!.CreateSessionAsync(user, device);

            bundle["Session"] = session;
            bundle["Response"] = mapper!.Map<LoginData, LoginResponse>(session);

            return true;
        }
    }
}
