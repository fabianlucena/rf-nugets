using Microsoft.Extensions.DependencyInjection;
using RFRBAC.IServices;

namespace RFRGOBAC.DTO;

public class OrganizationUserRequest
{
    public string DisplayName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool CanLogin { get; set; }
    public IEnumerable<Guid> RolesUuid { get; set; } = [];

    public async Task<OrganizationUser> ToOrganizationUser(IServiceProvider serviceProvider)
    {
        var roleService = serviceProvider.GetRequiredService<IRoleService>();

        return new OrganizationUser
        {
            DisplayName = DisplayName,
            Username = Username,
            Password = Password,
            IsActive = IsActive,
            CanLogin = CanLogin,
            RolesId = await roleService.GetListIdByUuidAsync(RolesUuid),
        };
    }
}
