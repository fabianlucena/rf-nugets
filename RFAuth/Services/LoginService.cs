using RFAuth.DTO;
using RFAuth.Exceptions;
using RFAuth.IServices;
using RFHttpExceptions.Exceptions;
using RFLocalizer.IServices;
using RFService.IServices;
using System.Globalization;

namespace RFAuth.Services
{
    public class LoginService(
        IUserService userService,
        IPasswordService passwordService,
        IDeviceService deviceService,
        ISessionService sessionService,
        IPropertiesDecorators propertiesDecorators,
        ILocalizerContextService localizerService
    )
        : ILoginService,
            IServiceDecorated
    {
        public IPropertiesDecorators PropertiesDecorators { get; } = propertiesDecorators;

        public async Task<LoginData> LoginAsync(LoginRequest request)
        {
            CultureInfo.CurrentCulture = new CultureInfo("es");
            CultureInfo.CurrentUICulture = new CultureInfo("es");

            if (string.IsNullOrWhiteSpace(request.Username))
                throw new HttpArgumentNullOrEmptyException(paramName: nameof(request.Username));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new HttpArgumentNullOrEmptyException(nameof(request.Password));

            var user = await userService.GetSingleOrDefaultForUsernameAsync(request.Username.Trim())
                ?? throw new UnknownUserException(localizerService["loginException"]["Unknown username"]);

            var password = await passwordService.GetSingleForUserAsync(user);
            var check = passwordService.Verify(request.Password.Trim(), password);
            if (!check)
                throw new BadPasswordException(localizerService["passwordException"]["Bad password"]);

            var device = await deviceService.GetSingleForTokenOrCreateAsync(request.DeviceToken);
            var session = await sessionService.CreateForUserAndDeviceAsync(user, device);

            return new LoginData
            {
                Username = user.Username,
                AuthorizationToken = session.Token,
                AutoLoginToken = session.AutoLoginToken,
                DeviceToken = device.Token,
                UserId = session.UserId,
            };
        }

        public async Task<LoginData> AutoLoginAsync(AutoLoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AutoLoginToken))
                throw new HttpArgumentNullOrEmptyException(nameof(request.AutoLoginToken));

            if (string.IsNullOrWhiteSpace(request.DeviceToken))
                throw new HttpArgumentNullOrEmptyException(nameof(request.DeviceToken));

            var device = await deviceService.GetSingleOrDefaultForTokenAsync(request.DeviceToken)
                ?? throw new UnknownDeviceException();

            var session = await sessionService.CreateForAutoLoginTokenAndDeviceAsync(request.AutoLoginToken, device);
            var user = await userService.GetSingleForIdAsync(session.UserId);

            return new LoginData
            {
                Username = user.Username,
                AuthorizationToken = session.Token,
                AutoLoginToken = session.AutoLoginToken,
                DeviceToken = device.Token,
                UserId = session.UserId
            };
        }
    }
}
