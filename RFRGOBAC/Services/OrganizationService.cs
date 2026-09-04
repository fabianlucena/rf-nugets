using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
) : ALocalizableEntityService<Organization>(organizationRepository, serviceProvider),
    IOrganizationService
{
    private IEnumerable<Organization>? _currentOrganizations = null;

    public IEnumerable<Organization> CurrentOrganizations
    {
        get
        {
            if (_currentOrganizations is null)
            {
                var contextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var items = contextAccessor.HttpContext?.Items;
                if (items?.TryGetValue("Organizations", out var organizationsRaw) == true
                    && organizationsRaw is IEnumerable<Organization> organizations
                )
                    _currentOrganizations = organizations;
                else
                    _currentOrganizations = [];
            }

            return _currentOrganizations;
        }
    }

    public IEnumerable<Organization> GetCurrentOrganizations()
        => CurrentOrganizations;

    public IEnumerable<long> GetCurrentOrganizationsId()
        => GetCurrentOrganizations().Select(o => o.Id);
}
