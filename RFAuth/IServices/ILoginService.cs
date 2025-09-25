using RFAuth.DTO;
using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface ILoginService
        : IServiceDecorated
    {
        Task<LoginData> CreateSessionAsync(User user, Device device, string? jsonData = null);

        Task<LoginData> LoginAsync(LoginRequest request, string? jsonData = null);

        Task<LoginData> AutoLoginAsync(AutoLoginRequest request, string? jsonData = null);
    }
}
