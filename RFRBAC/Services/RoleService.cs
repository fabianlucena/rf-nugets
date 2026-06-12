using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
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
    public async Task<long> GetSingleIdOrCreateByNameAsync(string name, RoleQueryOptions? options = null)
        => (await GetSingleByNameOrCreateAsync(name, options)).Id;
}
