using RFBaseEntities.QueryOptions;
using RFBaseServices.Decorators;
using RFRGOBACEntities.Entities;
using RFRGOBACIServices.IServices;

namespace RFRGOBACServices.Decorators
{
    public class OrganizationServiceDecoratorBase(IOrganizationService organizationService)
        : LocalizableEntityServiceDecorator<Organization>(organizationService),
        IOrganizationService
    {
    }
}
