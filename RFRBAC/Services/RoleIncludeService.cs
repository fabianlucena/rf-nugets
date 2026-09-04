using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFRBAC.Services;

[RegisterService]
public class RoleIncludeService(
    IRoleIncludeRepository roleIncludeRepository,
    IRoleService roleService,
    IServiceProvider serviceProvider
)
    : CommonJoinService<RoleInclude>(roleIncludeRepository, serviceProvider),
    IRoleIncludeService
{
    public async Task<IEnumerable<long>> GetAllRolesIdByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null)
        => await roleIncludeRepository.GetAllRolesIdByRolesIdAsync(rolesId, options);

    public async Task<IEnumerable<string>> GetAllRolesNameByRolesIdAsync(IEnumerable<long> rolesId, RoleIncludeQueryOptions? options = null)
    {
        var allRolesId = await GetAllRolesIdByRolesIdAsync(rolesId, options);
        var allRolesName = await roleService.GetNamesByIdsAsync(allRolesId);
        return allRolesName;
    }
}
