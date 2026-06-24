using RFAuth.Entities;
using RFAuth.DTO;
using RFAuth.IServices;

namespace RFAuth.Decorators
{
    public class LoginServiceDecoratorBase(ILoginService loginService)
        : ILoginService
    {
        public virtual Task<Session> LoginAsync(UserIdAndDeviceIdDTO request, SessionData? data = null)
            => loginService.LoginAsync(request, data);

        public virtual Task<Session> AutoLoginAsync(AutoLoginRequest request, SessionData? data = null)
            => loginService.AutoLoginAsync(request, data);

        public virtual Task<Session> LoginAsync(LoginRequest request, SessionData? data = null)
            => loginService.LoginAsync(request, data);
    }
}
