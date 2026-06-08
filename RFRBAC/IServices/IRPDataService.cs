using RFAuth.Entities;
using RFRBAC.DTO;
using RFRBAC.QueryOptions;

namespace RFRBAC.IServices;

public interface IRPDataService
{
    Task<RPData?> GetSingleOrDefaultBySession(Session session, RPDataQueryOptions? options = null);
}
