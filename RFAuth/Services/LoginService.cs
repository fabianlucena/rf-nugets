using Microsoft.Extensions.Localization;
using RFAuth.DTO;
using RFAuth.Exceptions;
using RFAuth.IServices;
using RFHttpExceptions.Exceptions;
using RFService.IServices;
using RFService.Services;
using System.Globalization;

namespace RFAuth.Services
{
    public class LoginService(
        IUserService userService,
        IPasswordService passwordService,
        IDeviceService deviceService,
        ISessionService sessionService,
        IPropertiesDecorators propertiesDecorators,
        ILocalizerService localizerService
    )
        : ILoginService, IServiceDecorated
    {
        private readonly IUserService _userService = userService;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly IDeviceService _deviceService = deviceService;
        private readonly ISessionService _sessionService = sessionService;
        private readonly ILocalizerService _localizer = localizerService;

        public IPropertiesDecorators PropertiesDecorators { get; } = propertiesDecorators;

        public async Task<LoginData> LoginAsync(LoginRequest request)
        {
            CultureInfo.CurrentCulture = new CultureInfo("es");
            CultureInfo.CurrentUICulture = new CultureInfo("es");


            if (string.IsNullOrWhiteSpace(request.Username))
                throw new HttpArgumentNullOrEmptyException(paramName: nameof(request.Username));

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new HttpArgumentNullOrEmptyException(nameof(request.Password));

            var user = await _userService.GetSingleOrDefaultForUsernameAsync(request.Username.Trim())
                ?? throw new UnknownUserException(_localizer["Unknown username"]);

            var password = await _passwordService.GetSingleForUserAsync(user);
            var check = _passwordService.Verify(request.Password.Trim(), password);
            if (!check)
                throw new BadPasswordException(_localizer["Bad password"]);

            var device = await _deviceService.GetSingleForTokenOrCreateAsync(request.DeviceToken);
            var session = await _sessionService.CreateForUserAndDeviceAsync(user, device);
            
            return new LoginData
            {
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

            var device = await _deviceService.GetSingleOrDefaultForTokenAsync(request.DeviceToken)
                ?? throw new UnknownDeviceException();

            var session = await _sessionService.CreateForAutoLoginTokenAndDeviceAsync(request.AutoLoginToken, device);

            return new LoginData
            {
                AuthorizationToken = session.Token,
                AutoLoginToken = session.AutoLoginToken,
                DeviceToken = device.Token,
                UserId = session.UserId
            };
        }
    }
}
