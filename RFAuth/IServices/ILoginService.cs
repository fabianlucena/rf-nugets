using RFAuth.Entities;
using RFAuth.DTO;
using RFBase.ILibs;

namespace RFAuth.IServices
{
    public interface ILoginService
    {
        Task<Session> LoginAsync(LoginRequest request, IDataDictionary? data = null);
        Task<Session> AutoLoginAsync(AutoLoginRequest request, IDataDictionary? data = null);
    }
}
