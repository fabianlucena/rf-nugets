using RFAuth.Entities;
using RFRGOBAC.DTO;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface ISessionDataService
{
    Task<SessionData?> GetSingleOrDefaultBySession(Session session, SessionDataQueryOptions? options = null);
}
