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
    public override string? GetTranslationContext(Role entity)
        => base.GetTranslationContext(entity) ?? "rfrbac";
    
    public override async Task<Role> Translate(Role role, string? context = null)
    {
        if (!role.IsTranslatable)
            return role;

        role = await base.Translate(role, context);

        if (role.Description is not null)
            role.Description = await L10n._c(context ?? GetTranslationContext(role) ?? "rfrbac", role.Description); 
        
        return role;
    }
}