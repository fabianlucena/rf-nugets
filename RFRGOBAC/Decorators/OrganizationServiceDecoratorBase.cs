using RFRGOBAC.Entities;
using RFRGOBAC.IServices;
using RFServices.Decorators;

namespace RFRGOBAC.Decorators;

public class OrganizationServiceDecoratorBase(IOrganizationService organizationService)
    : LocalizableEntityServiceDecorator<Organization>(organizationService),
    IOrganizationService
{
    public IEnumerable<Organization> GetCurrentOrganizations()
        => organizationService.GetCurrentOrganizations();

    public IEnumerable<long> GetCurrentOrganizationsId()
        => organizationService.GetCurrentOrganizationsId();

    public Organization? GetCurrentOrganization()
        => organizationService.GetCurrentOrganization();
}
