using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIServices.DTO;
using RFAuthIServices.IServices;
using RFBaseEntities.ILibs;
using RFBaseIServices.IServices;

namespace RFAuthServices.Services
{
    public class LoginService(
        IUserService userService,
        IUserPasswordService userPasswordService,
        ISessionService sessionService,
        IDeviceService deviceService
    ) : ILoginService
    {
        public async Task<Session> LoginAsync(LoginRequest request, IDataDictionary? data)
        {
            var user = await userService.GetSingleByUsernameAsync(request.Username)
                ?? throw new Exception("User not found.");

            if (user.DeletedAt.HasValue)
            {
                throw new Exception("User is deleted.");
            }

            if (!user.IsActive)
            {
                throw new Exception("User is not active.");
            }

            if (!user.CanLogin)
            {
                throw new Exception("User is not allowed to login.");
            }

            if (!await userPasswordService.CheckPasswordByUserIdAsync(request.Password, user.Id))
            {
                throw new Exception("Invalid password.");
            }

            var device = await deviceService.GetFirstOrCreateByTokenAsync(request.DeviceToken);

            var session = await sessionService.CreateAsync(user.Id, device.Id, data);
            session.User = user;
            session.Device = device;

            await userService.UpdateLastLoginAtByUserIdAsync(user.Id);

            return session;
        }

        public async Task<Session> AutoLoginAsync(AutoLoginRequest request, IDataDictionary? data = null)
        {
            var previousSession = await sessionService.GetFirstOrDefaultByAutoLoginTokenAsync(
                    request.AutoLoginToken,
                    new SessionQueryOptions
                    {
                        IncludeUser = true,
                        IncludeDevice = true,
                    }
                ) ?? throw new Exception("Session not found.");

            if (previousSession.ClosedAt is not null)
            {
                throw new Exception("Session is closed.");
            }

            await sessionService.CloseByIdAsync(previousSession.Id);

            var user = previousSession.User
                ?? throw new Exception("User not found.");
            
            if (user.DeletedAt.HasValue)
            {
                throw new Exception("User is deleted.");
            }

            if (!user.IsActive)
            {
                throw new Exception("User is not active.");
            }

            if (!user.CanLogin)
            {
                throw new Exception("User is not allowed to login.");
            }

            var device = previousSession.Device
                ?? throw new Exception("Device not found.");

            if (device.Token != request.DeviceToken)
            {
                throw new Exception("Device token mismatch.");
            }

            var session = await sessionService.CreateAsync(user.Id, device.Id, data);
            session.User = user;
            session.Device = device;

            await userService.UpdateLastLoginAtByUserIdAsync(user.Id);

            return session;
        }
    }
}
