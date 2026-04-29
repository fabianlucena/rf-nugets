using RFBaseIServices.IServices;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIServices.IServices
{
    public interface ISessionCompanyService : INoIdEntityService<SessionCompany>
    {
        Task<Company> GetSingleCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
        Task<Company?> GetSingleOrDefaultCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
    }
}
