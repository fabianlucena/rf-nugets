using RFService.Services;
using RFService.IRepo;
using RFHttpAction.IServices;
using RFHttpAction.Entities;
using RFService.Repo;
using RFHttpAction.Util;

namespace RFHttpAction.Services
{
    public class HttpActionService(IRepo<HttpAction> repo)
        : ServiceTimestampsIdUuid<IRepo<HttpAction>, HttpAction>(repo),
            IHttpActionService
    {
        public override async Task<HttpAction> ValidateForCreationAsync(HttpAction data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrEmpty(data.Token))
            {
                data.Token = Token.GetString(64);
            };

            return data;
        }

        public async Task<HttpAction?> GetSingleOrDefaultForTokenAsync(string token)
        {
            return await repo.GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = { { "Token", token } }
            });
        }

        public Task<HttpAction> GetSingleForTokenAsync(string token)
        {
            return GetSingleAsync(new GetOptions { Filters = { { "Token", token } } });
        }

        public string GetUrl(HttpAction action)
        {
            return "v1/action/" + action.Token;
        }
    }
}
