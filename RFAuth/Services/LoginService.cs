using RFAuth.DTO;
using RFAuth.Entities;
using RFAuth.Exceptions;
using RFAuth.IServices;
using RFAuth.QueryOptions;
using RFBase.ILibs;
using RFIServices.IServices;
using RFRegisterService.Attributes;

namespace RFAuth.Services;

[RegisterService]
public class LoginService(
    IUserService userService,
    IUserPasswordService userPasswordService,
    ISessionService sessionService,
    IDeviceService deviceService
) : ILoginService
{
    public async Task<Session> LoginAsync(UserIdAndDeviceIdDTO request, IDataDictionary? data = null)
    {
        var session = await sessionService.CreateAsync(request.UserId, request.DeviceId, data);
        session = await sessionService.DecorateAsync(session);
        await userService.UpdateLastLoginAtByUserIdAsync(request.UserId);

        return session;
    }

    public async Task<Session> LoginAsync(LoginRequest request, IDataDictionary? data)
    {
        var user = await userService.GetSingleByUsernameAsync(request.Username)
            ?? throw new UserNotFoundException();

        if (user.DeletedAt.HasValue)
            throw new UserIsDeletedException();

        if (!user.IsActive)
            throw new UserIsNotActiveException();

        if (!user.CanLogin)
            throw new UserIsNotAllowedToLoginException();

        if (!await userPasswordService.CheckPasswordByUserIdAsync(request.Password, user.Id))
            throw new InvalidPasswordException();

        var device = await deviceService.GetSingleByTokenOrCreateAsync(request.DeviceToken);

        var session = await LoginAsync(new UserIdAndDeviceIdDTO { UserId = user.Id, DeviceId = device.Id }, data);

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
            ) ?? throw new SessionNotFoundException();

        if (previousSession.ClosedAt is not null)
            throw new SessionIsClosedException();

        await sessionService.CloseByIdAsync(previousSession.Id);

        var user = previousSession.User
            ?? throw new UserNotFoundException();
        
        if (user.DeletedAt.HasValue)
            throw new UserIsDeletedException();

        if (!user.IsActive)
            throw new UserIsNotActiveException();

        if (!user.CanLogin)
            throw new UserIsNotAllowedToLoginException();

        var device = previousSession.Device
            ?? throw new DeviceNotFoundException();

        if (device.Token != request.DeviceToken)
            throw new DeviceTokenMismatchException();

        var session = await LoginAsync(new UserIdAndDeviceIdDTO { UserId = user.Id, DeviceId = device.Id }, data);

        return session;
    }
}
