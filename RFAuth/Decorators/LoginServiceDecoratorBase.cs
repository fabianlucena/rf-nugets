using RFAuth.Entities;
using RFAuth.DTO;
using RFAuth.IServices;
using RFBase.ILibs;

namespace RFAuth.Decorators
{
    public class LoginServiceDecoratorBase(ILoginService loginService)
        : ILoginService
    {
        public virtual Task<Session> LoginAsync(UserIdAndDeviceIdDTO request, string identityProvider, IDataDictionary? data = null)
            => loginService.LoginAsync(request, identityProvider, data);

        public virtual Task<Session> AutoLoginAsync(AutoLoginRequest request, IDataDictionary? data = null)
            => loginService.AutoLoginAsync(request, data);

        public virtual Task<Session> LoginAsync(LoginRequest request, string identityProvider, IDataDictionary? data = null)
            => loginService.LoginAsync(request, identityProvider, data);
    }
}
