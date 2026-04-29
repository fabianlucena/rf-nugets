using RFBaseIServices.IServices;
using RFRGOBACEntities.Entities;
using RFRGOBACEntities.QueryOptions;

namespace RFRGOBACIServices.IServices
{
    public interface ISessionOrganizationService : INoIdEntityService<SessionOrganization>
    {
        Task<Organization> GetSingleOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
        Task<Organization?> GetSingleOrDefaultOrganizationBySessionIdAsync(long sessionId, SessionOrganizationQueryOptions? options = null);
    }
}
