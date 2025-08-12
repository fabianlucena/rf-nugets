using RFAuth.DTO;
using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface ILoginService
        : IServiceDecorated
    {
        Task<LoginData> CreateSessionAsync(User user, Device device);

        Task<LoginData> LoginAsync(LoginRequest request);

        Task<LoginData> AutoLoginAsync(AutoLoginRequest request);
    }
}
