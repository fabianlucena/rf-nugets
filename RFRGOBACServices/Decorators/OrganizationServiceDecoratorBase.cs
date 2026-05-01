using RFBaseServices.Decorators;
using RFRGOBACEntities.Entities;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Decorators
{
    public class OrganizationServiceDecoratorBase(IOrganizationService organizationService)
        : NominableEntityServiceDecorator<Organization>(organizationService),
        IOrganizationService
    {
    }
}
