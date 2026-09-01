using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFRBAC.Services;

[RegisterService]
public class RoleService(
    IRoleRepository roleRepository,
    IServiceProvider serviceProvider
)
    : LocalizableEntityService<Role>(roleRepository, serviceProvider),
    IRoleService
{
    public override string? GetTranlationContext(Role entity)
        => base.GetTranlationContext(entity) ?? "rfrbac";
    
    public override async Task<Role> Translate(Role role, string? context = null)
    {
        role = await base.Translate(role, context);

        if (role.Description is not null)
            role.Description = await L10n._c(context ?? GetTranlationContext(role) ?? "rfrbac", role.Description); 
        
        return role;
    }
}