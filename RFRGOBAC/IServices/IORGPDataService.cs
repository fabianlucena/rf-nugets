using RFAuth.Entities;
using RFRGOBAC.DTO;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface IORGPDataService
{
    Task<ORGPData?> GetSingleOrDefaultBySession(Session session, ORGPDataQueryOptions? options = null);
}
