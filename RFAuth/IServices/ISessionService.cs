using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface ISessionService
        : IService<Session>
    {
        Task<Session> CreateForUserIdAndDeviceIdAsync(Int64 userId, Int64 deviceId, string? jsonData = null);

        Task<Session> CreateForUserAndDeviceAsync(User user, Device device, string? jsonData = null);

        Task<Session> CreateForAutoLoginTokenAndDeviceAsync(string autoLoginToken, Device device, string? jsonData = null);

        Task<bool> CloseForIdAsync(Int64 id);

        Task<Session?> GetForTokenOrDefaultAsync(string token);
    }
}
