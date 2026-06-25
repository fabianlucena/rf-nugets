using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFRBAC.Services;

[RegisterService]
public class RoleXUserService(
    IRoleXUserRepository roleXUserRepository,
    IRoleService roleService,
    IRoleIncludeService roleIncludeService,
    IServiceProvider serviceProvider
) : CommonJoinService<RoleXUser>(roleXUserRepository, serviceProvider),
    IRoleXUserService
{
    public IUserService UserService { get => ServiceProvider.GetRequiredService<IUserService>(); }

    public async Task<IEnumerable<long>> GetRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        => await roleXUserRepository.GetListRoleIdsByUserIdAsync(userId, options);

    public async Task<IEnumerable<string>> GetRoleNamesByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        => await roleXUserRepository.GetListRoleNamesByUserIdAsync(userId, options);

    public async Task<IEnumerable<long>> GetAllRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
    {
        var roleIds = await GetRoleIdsByUserIdAsync(userId, options);
        var allRoleIds = await roleIncludeService.GetAllRoleIdsByRoleIdsAsync(roleIds);
        return allRoleIds;
    }

    public async Task<long> SetAllRolesForUserIdAsync(IEnumerable<string> roles, long userId, RoleXUserQueryOptions? options = null)
    {
        var exisitingRoles = await GetRoleNamesByUserIdAsync(userId, options);
        var addRoles = roles.Except(exisitingRoles);
        var removeRoles = exisitingRoles.Except(roles);

        var updated = 0;
        var systemUserId = await UserService.GetSystemUserIdAsync();
        foreach (var role in addRoles)
        {
            var roleId = await roleService.GetIdOrCreateByNameAsync(
                role,
                createFactory: async r =>
                {
                    r.CreatedById = systemUserId;
                    r.UpdatedById = systemUserId;
                    return r;
                }
            );
            await CreateAsync(new RoleXUser
            {
                UserId = userId,
                RoleId = roleId,
                CreatedById = systemUserId,
            });
            updated++;
        }

        foreach (var role in removeRoles)
        {
            var roleId = await roleService.GetSingleIdByNameAsync(role);
            await DeleteAsync(new RoleXUserQueryOptions
            {
                RoleId = roleId,
                UserId = userId
            });
            updated++;
        }

        return updated;
    }

    public async Task<bool> UserIdHasRoleIdAsync(long userId, long roleId, RoleXUserQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserQueryOptions();
        options.UserId = userId;
        options.RoleId = roleId;
        var rows = await GetListAsync(options);
        return rows.Any();
    }
}
