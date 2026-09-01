using Microsoft.Extensions.DependencyInjection;
using RFIServices.IServices;
using RFRBAC.Entities;
using RFRBAC.Exceptions;
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
    public IRoleService RoleService { get => ServiceProvider.GetRequiredService<IRoleService>(); }
    public IUserService UserService { get => ServiceProvider.GetRequiredService<IUserService>(); }

    public async Task<IEnumerable<long>> GetRoleIdsByUserIdsAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserQueryOptions();
        options.UserIds = userIds;
        return await roleXUserRepository.GetRolesIdAsync(options);
    }

    public async Task<IEnumerable<long>> GetRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        => await GetRoleIdsByUserIdsAsync([userId], options);

    public async Task<IEnumerable<long>> GetRolesIdByUsersIdAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserQueryOptions();
        options.UserIds = userIds;
        return await roleXUserRepository.GetRolesIdAsync(options);
    }

    public async Task<IEnumerable<string>> GetRolesNameByUsersIdAsync(IEnumerable<long> userIds, RoleXUserQueryOptions? options = null)
    {
        options = options?.Clone() ?? new RoleXUserQueryOptions();
        options.UserIds = userIds;
        return await roleXUserRepository.GetRolesNameAsync(options);
    }

    public async Task<IEnumerable<long>> GetRolesIdByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        => await GetRolesIdByUsersIdAsync([userId], options);

    public async Task<IEnumerable<string>> GetRolesNameByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
        => await GetRolesNameByUsersIdAsync([userId], options);

    public async Task<IEnumerable<long>> GetAllRoleIdsByUserIdAsync(long userId, RoleXUserQueryOptions? options = null)
    {
        var roleIds = await GetRoleIdsByUserIdAsync(userId, options);
        var allRoleIds = await roleIncludeService.GetAllRoleIdsByRoleIdsAsync(roleIds);
        return allRoleIds;
    }

    public async Task<long> SetAllRolesForUserIdAsync(IEnumerable<string> roles, long userId, RoleXUserQueryOptions? options = null)
    {
        var exisitingRoles = await GetRolesNameByUserIdAsync(userId, options);
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

    public async Task<long> SetAllRolesIdForUserIdAsync(IEnumerable<long> rolesId, long userId, RoleXUserQueryOptions? options = null)
    {
        var exisitingRoles = await GetRolesIdByUserIdAsync(userId, options);
        var addRoles = rolesId.Except(exisitingRoles);
        var removeRoles = exisitingRoles.Except(rolesId);

        var updated = 0;
        var systemUserId = await UserService.GetSystemUserIdAsync();
        foreach (var roleId in addRoles)
        {
            await CreateAsync(new RoleXUser
            {
                UserId = userId,
                RoleId = roleId,
                CreatedById = systemUserId,
            });
            updated++;
        }

        foreach (var roleId in removeRoles)
        {
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

    public async Task<bool> CreateIfNotExistsAsync(IDictionary<string, IEnumerable<string>> usersRoles)
    {
        var creatorId = await UserService.GetCurrentOrSystemUserIdAsync();
        foreach (var kvp in usersRoles)
        {
            var username = kvp.Key;
            var roleNames = kvp.Value;
            var roleIds = await RoleService.GetIdsByNamesAsync(roleNames);
            if (roleIds.Count() != roleNames.Count())
                throw new SomeRolesDoNotExistException(roleNames.Except(await RoleService.GetNamesByIdsAsync(roleIds)));

            var userId = await UserService.GetSingleIdByUsernameAsync(username);

            var existentRoleIds = await this.GetRoleIdsByUserIdsAsync([userId]);
            var newRoleIds = roleIds.Except(existentRoleIds);

            foreach (var roleId in newRoleIds)
            {
                await CreateAsync(new RoleXUser
                {
                    UserId = userId,
                    RoleId = roleId,
                    CreatedById = creatorId,
                });
            }
        }

        return true;
    }
}
