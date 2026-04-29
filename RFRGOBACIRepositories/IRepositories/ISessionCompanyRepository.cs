using RFBaseIRepositories.IRepositories;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIRepositories.IRepositories
{
    public interface ISessionCompanyRepository : INoIdEntityRepository<SessionCompany>
    {
        Task<Company?> GetSingleOrDefaultCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
        Task<Company> GetSingleCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null);
    }
}