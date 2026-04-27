using RFAuthEntities.Entities;
using RFAuthIServices.DTO;
using RFBaseEntities.ILibs;

namespace RFAuthIServices.IServices
{
    public interface ILoginService
    {
        Task<Session> LoginAsync(LoginRequest request, IDataDictionary? data = null);
    }
}
