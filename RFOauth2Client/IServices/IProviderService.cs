using RFOauth2Client.DTO;
using RFService.Libs;

namespace RFOauth2Client.IServices
{
    public interface IProviderService
    {
        Task<IEnumerable<Provider>> GetListAsync();

        Task<IEnumerable<AuthorizeProvider>> GetListAuthorizeAsync();

        Task<Provider?> GetSingleOrDefaultByNameAsync(string name);

        Task<object?> CallbackAsync(string name, string actionName, DataDictionary? data);
    }
}
