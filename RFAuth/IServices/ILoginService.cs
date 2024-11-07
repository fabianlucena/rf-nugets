using RFAuth.DTO;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface ILoginService : IServiceDecorated
    {
        Task<LoginData> LoginAsync(LoginRequest request);

        Task<LoginData> AutoLoginAsync(AutoLoginRequest request);
    }
}
