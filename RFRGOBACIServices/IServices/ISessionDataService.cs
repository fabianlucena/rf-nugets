using RFAuthEntities.Entities;
using RFRGOBACIRepositories.DTO;
using RFRGOBACIServices.QueryOptions;

namespace RFRGOBACIServices.IServices
{
    public interface ISessionDataService
    {
        Task<SessionData?> GetSingleOrDefaultBySession(Session session, SessionDataQueryOptions? options = null);
    }
}
