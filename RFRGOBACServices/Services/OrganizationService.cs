using RFBaseServices.Services;
using RFRGOBACEntities.Entities;
using RFRGOBACIRepositories.IRepositories;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Services
{
    public class OrganizationService(
        IOrganizationRepository organizationRepository
    ) : LocalizableEntityService<Organization>(organizationRepository),
        IOrganizationService
    {
    }
}
