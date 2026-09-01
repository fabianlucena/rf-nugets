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
    public async Task<IEnumerable<long>> GetAllRolesIdByRolesIdAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
        => await roleIncludeRepository.GetAllRolesIdByRolesIdAsync(roleIds, options);

    public async Task<IEnumerable<string>> GetAllRolesNameByRolesIdAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
    {
        var allRoleIds = await GetAllRolesIdByRolesIdAsync(roleIds, options);
        var allRoleNames = await roleService.GetNamesByIdsAsync(allRoleIds);
        return allRoleNames;
    }
}
