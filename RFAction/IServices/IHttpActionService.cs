using RFHttpAction.Entities;
using RFService.IService;

namespace RFHttpAction.IServices
{
    public interface IHttpActionService
        : IServiceTimestamps<HttpAction>,
            IServiceId<HttpAction>
    {
        Task<HttpAction> GetSingleForTokenAsync(string token);

        Task<HttpAction?> GetSingleOrDefaultForTokenAsync(string token);

        string GetUrl(HttpAction action);
    }
}
