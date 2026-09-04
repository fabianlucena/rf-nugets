using RFIServices.IServices;
using RFRGOBAC.Entities;

namespace RFRGOBAC.IServices;

public interface IOrganizationService : IALocalizableEntityService<Organization>
{
    IEnumerable<Organization> GetCurrentOrganizations();
    IEnumerable<long> GetCurrentOrganizationsId();
    Organization? GetCurrentOrganization();
}
