using RFAuthEntities.Entities;
using RFRGCBACIRepositories.DTO;
using RFRGCBACIServices.QueryOptions;

namespace RFRGCBACIServices.IServices
{
    public interface ISessionDataService
    {
        Task<SessionData?> GetSingleOrDefaultBySession(Session session, SessionDataQueryOptions? options = null);
    }
}
