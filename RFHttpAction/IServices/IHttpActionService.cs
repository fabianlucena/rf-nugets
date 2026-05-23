using RFHttpAction.Entities;
using RFHttpAction.QueryOptions;
using RFIServices.IServices;

namespace RFHttpAction.IServices
{
    public interface IHttpActionService : ICreatableEntityService<HttpAction>
    {
        Task<HttpAction> GetSingleByTokenAsync(string token, HttpActionQueryOptions? options = null);

        Task<HttpAction?> GetSingleOrDefaultByTokenAsync(string token, HttpActionQueryOptions? options = null);

        Task CloseForIdAsync(long id);

        string GetUrl(HttpAction action);
    }
}
