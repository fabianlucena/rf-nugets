using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.IServices;
using RFServices.Services;

namespace RFRGOBAC.Services;

public class OrganizationService(
    IOrganizationRepository organizationRepository,
    IServiceProvider serviceProvider
) : LocalizableEntityService<Organization>(organizationRepository, serviceProvider),
    IOrganizationService
{
}
