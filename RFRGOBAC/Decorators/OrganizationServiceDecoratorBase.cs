using RFRGOBAC.Entities;
using RFRGOBAC.IServices;
using RFServices.Decorators;

namespace RFRGOBAC.Decorators;

public class OrganizationServiceDecoratorBase(IOrganizationService organizationService)
    : LocalizableEntityServiceDecorator<Organization>(organizationService),
    IOrganizationService
{
}
