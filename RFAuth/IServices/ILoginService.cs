using RFAuth.Entities;
using RFAuth.DTO;
using RFBase.ILibs;

namespace RFAuth.IServices
{
    public interface ILoginService
    {
        Task<Session> LoginAsync(UserIdAndDeviceIdDTO request, string identityProvider, IDataDictionary? data = null);
        Task<Session> LoginAsync(LoginRequest request, string identityProvider, IDataDictionary? data = null);
        Task<Session> AutoLoginAsync(AutoLoginRequest request, IDataDictionary? data = null);
    }
}
