using RFRegisterService.Attributes;
using RFRGOBAC.Entities;
using RFRGOBAC.IRepositories;
using RFRGOBAC.IServices;
using RFServices.Services;

namespace RFRGOBAC.Services;

[RegisterService]
public class OrganizationService(
    IOrganizationRepository organizationRepository,
    IServiceProvider serviceProvider
) : LocalizableEntityService<Organization>(organizationRepository, serviceProvider),
    IOrganizationService
{
}
