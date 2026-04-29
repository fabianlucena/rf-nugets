using RFBaseIRepositories.IRepositories;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;

namespace RFRGCBACIRepositories.IRepositories
{
    public interface ISessionCompanyRepository : INoIdEntityRepository<SessionCompany>
    {
        Task<Company?> GetSingleOrDefaultCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
        Task<Company> GetSingleCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
    }
}