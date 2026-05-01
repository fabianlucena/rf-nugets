using RFAuthEntities.Entities;
using RFAuthIServices.DTO;
using RFAuthIServices.IServices;
using RFBaseEntities.ILibs;

namespace RFAuthServices.Decorators
{
    public class LoginServiceDecoratorBase(ILoginService loginService)
        : ILoginService
    {
        public virtual Task<Session> AutoLoginAsync(AutoLoginRequest request, IDataDictionary? data = null)
            => loginService.AutoLoginAsync(request, data);

        public virtual Task<Session> LoginAsync(LoginRequest request, IDataDictionary? data = null)
            => loginService.LoginAsync(request, data);
    }
}
