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
{}
