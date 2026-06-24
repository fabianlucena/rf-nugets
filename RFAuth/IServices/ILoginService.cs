using RFAuth.Entities;
using RFAuth.DTO;

namespace RFAuth.IServices
{
    public interface ILoginService
    {
        Task<Session> LoginAsync(UserIdAndDeviceIdDTO request, SessionData? data = null);
        Task<Session> LoginAsync(LoginRequest request, SessionData? data = null);
        Task<Session> AutoLoginAsync(AutoLoginRequest request, SessionData? data = null);
    }
}
