using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;

namespace RFRGOBAC.DTO;

public class SystemUserRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool CanLogin { get; set; }
    public IEnumerable<Guid> GlobalRolesUuid { get; set; } = [];
    public IEnumerable<OrganizationRolesUuid> OrganizationsRolesUuid { get; set; } = [];

    public async Task<SystemUser> ToSystemUser(IServiceProvider serviceProvider)
    {
        var roleService = serviceProvider.GetRequiredService<IRoleService>();

        return new SystemUser
        {
            DisplayName = DisplayName,
            Username = Username,
            Password = Password,
            IsActive = IsActive,
            CanLogin = CanLogin,
            SystemRolesId = await roleService.GetListIdByUuidAsync(GlobalRolesUuid),
            OrganizationsRolesId = [..OrganizationsRolesUuid.Select(o => o.ToOrganizationRolesId(serviceProvider).GetAwaiter().GetResult())],
        };
    }
}
