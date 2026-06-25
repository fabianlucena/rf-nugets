using RFAuth.Entities;
using RFRBAC.DTO;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices;

public interface IRPDataService
{
    Task<RPData?> GetSingleOrDefaultBySession(Session session, RPDataQueryOptions? options = null);
    Task<RPData> GetSingleBySession(Session session, RPDataQueryOptions? options = null);
    Task<Session> DecorateSession(Session session, RPDataQueryOptions? options = null);
}
