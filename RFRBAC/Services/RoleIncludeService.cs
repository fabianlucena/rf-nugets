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
    public async Task<IEnumerable<long>> GetAllRoleIdsByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
        => await roleIncludeRepository.GetAllRoleIdsByRoleIdsAsync(roleIds, options);

    public async Task<IEnumerable<string>> GetAllRoleNamesByRoleIdsAsync(IEnumerable<long> roleIds, RoleIncludeQueryOptions? options = null)
    {
        var allRoleIds = await GetAllRoleIdsByRoleIdsAsync(roleIds, options);
        var allRoleNames = await roleService.GetNamesByIdsAsync(allRoleIds);
        return allRoleNames;
    }
}
