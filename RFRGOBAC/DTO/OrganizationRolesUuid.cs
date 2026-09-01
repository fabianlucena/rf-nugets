using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;
using RFRGOBAC.IServices;

namespace RFRGOBAC.DTO;

public class OrganizationRolesUuid
{
    public Guid OrganizationUuid { get; set; }
    public IEnumerable<Guid> RolesUuid { get; set; } = [];

    public async Task<OrganizationRolesId> ToOrganizationRolesId(IServiceProvider serviceProvider)
    {
        var organizationService = serviceProvider.GetRequiredService<IOrganizationService>();
        var roleService = serviceProvider.GetRequiredService<IRoleService>();

        return new OrganizationRolesId
        {
            OrganizationId = await organizationService.GetSingleIdByUuidAsync(OrganizationUuid),
            RolesId = await roleService.GetListIdByUuidAsync(RolesUuid)
        };
    }
}
