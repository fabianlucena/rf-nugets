using RFBaseIServices.IServices;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;

namespace RFRGCBACIServices.IServices
{
    public interface ISessionCompanyService : INoIdEntityService<SessionCompany>
    {
        Task<Company> GetSingleCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
        Task<Company?> GetSingleOrDefaultCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
    }
}
