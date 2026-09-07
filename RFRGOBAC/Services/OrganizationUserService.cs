using RFAuth.IServices;
using RFBase.ILibs;
using RFIServices.IServices;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.Exceptions;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.Services;

[RegisterService]
public class OrganizationUserService(
    IUserService userService,
    IUserPasswordService userPasswordService,
    IUserTypeService userTypeService,
    IRoleService roleService,
    IRoleXUserXOrganizationService roleXUserXOrganizationService
) : IOrganizationUserService
{
    public async Task<OrganizationUser> CreateAsync(long organizationId, OrganizationUser user)
    {
        user = user.Clone();
        if (user.TypeId <= 0)
        {
            user.TypeId = user.Type?.Id
                ?? await userTypeService.GetSingleIdByNameAsync("user");
        }

        var result = new OrganizationUser(await userService.CreateAsync(user));

        if (!string.IsNullOrWhiteSpace(user.Password))
            await userPasswordService.CreateOrUpdateByUserIdAsync(user.Password, result.Id);

        if (user.RolesId is not null)
        {
            await roleXUserXOrganizationService.SetOrganizationsRolesIdForUserIdAsync(
                [
                    new() {
                        OrganizationId = organizationId,
                        RolesId = user.RolesId,
                    },
                ],
                result.Id
            );
        }

        return result;
    }

    public async Task<IEnumerable<OrganizationUser>> GetListAsync(OrganizationUserQueryOptions? options)
    {
        options ??= new OrganizationUserQueryOptions();

        var roleXUserXOrganizationQueryOptions = new RoleXUserXOrganizationQueryOptions
        {
            OrganizationId = options.OrganizationId,
        };

        var userOptions = options.Clone();
        if (options.Uuid is not null)
            roleXUserXOrganizationQueryOptions.UserId = await userService.GetSingleIdByUuidAsync(options.Uuid.Value, userOptions);

        var usersId = (await roleXUserXOrganizationService.GetUsersIdAsync(roleXUserXOrganizationQueryOptions)).ToList();

        userOptions.Ids = usersId;
        var users = (await userService.GetListAsync(userOptions))
            .Select(user => new OrganizationUser(user));

        if (options.IncludeRoles)
        {
            var newUsers = new List<OrganizationUser>();
            foreach (var user in users)
            {
                user.Roles = await roleXUserXOrganizationService.GetRolesAsync(new RoleXUserXOrganizationQueryOptions
                {
                    OrganizationId = options.OrganizationId,
                    UserId = user.Id,
                });

                user.RolesId = user.Roles.Select(r => r.Id);
                newUsers.Add(user);
            }
            users = newUsers;
        }

        if (options.IncludeCanEdit)
        {
            var newUsers = new List<OrganizationUser>();
            foreach (var user in users)
            {
                var organizationId = await roleXUserXOrganizationService.GetOrganizationsIdAsync(new RoleXUserXOrganizationQueryOptions
                {
                    IncludeDeleted = true,
                    UserId = user.Id,
                    NotOrganizationId = options.OrganizationId,
                });

                user.CanEdit = !organizationId.Any();

                newUsers.Add(user);
            }
            users = newUsers;
        }

        return users;
    }

    public async Task<OrganizationUser?> GetSingleOrDefaultAsync(OrganizationUserQueryOptions? options)
    {
        options ??= new OrganizationUserQueryOptions();
        options.Take = 2;
        var users = await GetListAsync(options);
        if (users.Count() > 1)
            throw new ThereAreMultipleUsersMatchingTheGivenConditionsException();

        return users.FirstOrDefault();
    }

    public async Task<int> DeleteByUuidAsync(Guid uuid, OrganizationUserQueryOptions? options = null)
        => await userService.DeleteByUuidAsync(uuid, options);

    public async Task<int> RestoreByUuidAsync(Guid uuid, OrganizationUserQueryOptions? options = null)
        => await userService.RestoreByUuidAsync(uuid, options);

    public async Task<OrganizationUser> Translate(OrganizationUser user, string? context = null)
    {
        user = user.Clone();

        if (user.Type is not null)
            user.Type = await userTypeService.Translate(user.Type!);

        if (user.Roles is not null)
            user.Roles = await roleService.Translate(user.Roles);

        return user;
    }

    public async Task<IEnumerable<OrganizationUser>> Translate(IEnumerable<OrganizationUser> users, string? context = null)
        => await Task.WhenAll(users.Select(user => Translate(user, context)));

    public async Task<int> UpdateByUuidAsync(long organizationId, Guid uuid, IDataDictionary data, OrganizationUserQueryOptions? options = null)
    {
        var id = await userService.GetSingleIdByUuidAsync(uuid, options);

        await userService.UpdateByIdAsync(id, data.FilterKeys("DisplayName", "Username", "IsActive", "CanLogin"));

        if (data.TryGetString("Password", out var password) && !string.IsNullOrWhiteSpace(password))
            await userPasswordService.CreateOrUpdateByUserIdAsync(password, id);

        if (data.TryGetGuids("RolesUuid", out var rolesUuid))
            await SetRolesUuidByIdAsync(organizationId, id, rolesUuid, options);

        return 1;
    }
    
    public async Task<int> SetRolesUuidByIdAsync(long organizationId, long userId, IEnumerable<Guid> rolesUuid, OrganizationUserQueryOptions? options = null)
    {
        var organizationsRolesId = new OrganizationRolesId
        {
            OrganizationId = organizationId,
            RolesId = await roleService.GetListIdByUuidAsync(rolesUuid),
        };

        return await roleXUserXOrganizationService.SetOrganizationsRolesIdForUserIdAsync([organizationsRolesId], userId);
    }

    public async Task<int> SetRolesUuidByUuidAsync(long organizationId, Guid userUuid, IEnumerable<Guid> rolesUuid, OrganizationUserQueryOptions? options = null)
    {
        var userId = await userService.GetSingleIdByUuidAsync(userUuid, options);
        return await SetRolesUuidByIdAsync(organizationId, userId, rolesUuid, options);
    }
}
