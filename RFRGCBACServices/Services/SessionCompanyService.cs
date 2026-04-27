using RFBaseServices.Services;
using RFRGCBACEntities.Entities;
using RFRGCBACEntities.QueryOptions;
using RFRGCBACIRepositories.IRepositories;
using RFRGCBACIServices.IServices;

namespace RFRGCBACServices.Services
{
    public class SessionCompanyService(
        ISessionCompanyRepository sessionCompanyRepository
    ) : NoIdEntityService<SessionCompany>(sessionCompanyRepository),
        ISessionCompanyService
    {
        public async Task<Company> GetSingleCompanyBySessionIdAsync(long sessionId, SessionCompanyQueryOptions? options = null)
        {
            var company= await sessionCompanyRepository.GetSingleCompanyBySessionIdAsync(sessionId, options);
            return company;
        }

        public async Task<Company?> GetSingleOrDefaultCompanyBySessionIdAsync(long id, SessionCompanyQueryOptions? options = null)
        {
            var company = await sessionCompanyRepository.GetSingleOrDefaultCompanyBySessionIdAsync(id, options);
            return company;
        }
    }
}
