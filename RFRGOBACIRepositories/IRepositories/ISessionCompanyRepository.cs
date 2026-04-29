using RFBaseIRepositories.IRepositories;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIRepositories.IRepositories
{
    public interface ISessionOrganizationRepository : INoIdEntityRepository<SessionOrganization>
    {
        Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
        Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
    }
}