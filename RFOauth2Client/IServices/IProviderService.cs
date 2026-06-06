using RFAuth.DTO;
using RFBase.Libs;
using RFOauth2Client.Entities;

namespace RFOauth2Client.IServices
{
    public interface IProviderService
    {
        Task<IEnumerable<Provider>> GetListAsync();

        Task<IEnumerable<Provider>> GetListAuthorizeAsync();

        Task<Provider?> GetSingleOrDefaultByNameAsync(string name);

        Task<SessionResponse?> CallbackAsync(string name, string actionName, DataDictionary? data);

        Task<SessionResponse?> CallbackAuthorizeAsync(Provider provider, DataDictionary? data);
    }
}
