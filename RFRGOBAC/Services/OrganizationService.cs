using RFL10n;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.IServices;
using RFServices.Services;

namespace RFRGOBAC.Services;

public class OrganizationService(
    IOrganizationRepository organizationRepository,
    IL10n l10n
) : LocalizableEntityService<Organization>(organizationRepository, l10n),
    IOrganizationService
{
}
