using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;

namespace RFRGOBAC.DTO;

public class SystemUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public IEnumerable<Guid> GlobalRolesUuid { get; set; } = [];
    public IEnumerable<OrganizationRolesUuid> OrganizationsRolesUuid { get; set; } = [];

    public async Task<SystemUser> ToSystemUser(IServiceProvider serviceProvider)
    {
        var roleService = serviceProvider.GetRequiredService<IRoleService>();

        return new SystemUser
        {
            Username = Username,
            DisplayName = DisplayName,
            IsActive = IsActive,
            GlobalRolesId = await roleService.GetIdsByUuidsAsync(GlobalRolesUuid),
            OrganizationsRolesId = [..await Task.WhenAll(OrganizationsRolesUuid.Select(o => o.ToOrganizationRolesId(serviceProvider)))],
        };
    }
}
