using RFBaseServices.Services;
using RFL10n;
using RFRGOBACEntities.Entities;
using RFRGOBACIRepositories.IRepositories;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Services
{
    public class OrganizationService(
        IOrganizationRepository organizationRepository,
        IL10n l10n
    ) : LocalizableEntityService<Organization>(organizationRepository, l10n),
        IOrganizationService
    {
    }
}
