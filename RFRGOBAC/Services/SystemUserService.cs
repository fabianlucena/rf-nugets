using RFAuth.IServices;
using RFBase.ILibs;
using RFBase.Libs;
using RFEntities.Entities;
using RFIServices.IServices;
using RFRBAC.IServices;
using RFRBAC.Services;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.Exceptions;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.Services;


[RegisterService]
public class SystemUserService(
    IUserService userService,
    IUserPasswordService userPasswordService,
    IUserTypeService userTypeService,
    IRoleService roleService,
    IRoleXUserService roleXUserService,
    IRoleXUserXOrganizationService roleXUserXOrganizationService,
    IOrganizationService organizationService
) : ISystemUserService
{
    public async Task<SystemUser> CreateAsync(SystemUser user)
    {
        user = user.Clone();
        if (user.TypeId <= 0)
        {
            user.TypeId = user.Type?.Id
                ?? await userTypeService.GetSingleIdByNameAsync("user");
        }

        var result = new SystemUser(await userService.CreateAsync(user));

        if (!string.IsNullOrWhiteSpace(user.Password))
            await userPasswordService.CreateOrUpdateByUserIdAsync(user.Password, result.Id);

        if (user.SystemRolesId is not null)
            await roleXUserService.SetAllRolesIdForUserIdAsync(user.SystemRolesId, result.Id);

        if (user.OrganizationsRolesId is not null)
            await roleXUserXOrganizationService.SetOrganizationsRolesIdForUserIdAsync(user.OrganizationsRolesId, result.Id);

        return result;
    }

    public async Task<IEnumerable<SystemUser>> GetListAsync(SystemUserQueryOptions? options)
    {
        options ??= new SystemUserQueryOptions();
        var users = (await userService
            .GetListAsync(options))
            .Select(user =>
            {
                var result = new SystemUser(user);

                if (options.IncludeSystemRoles)
                {
                    result.SystemRoles = roleXUserService.GetRolesByUserIdAsync(user.Id)
                        .GetAwaiter()
                        .GetResult();

                    result.SystemRolesId = result.SystemRoles.Select(r => r.Id);
                }

                if (options.IncludeOrganizationsRoles)
                {
                    result.OrganizationsRoles = roleXUserXOrganizationService.GetOrganizationsRolesByUserIdAsync(user.Id)
                        .GetAwaiter()
                        .GetResult();

                    result.OrganizationsRolesId = result.OrganizationsRoles.Select(or => new OrganizationRolesId
                    {
                        OrganizationId = or.Organization!.Id,
                        RolesId = or.Roles!.Select(r => r.Id)
                    });

                    if (options.IncludeOrganizations)
                    {
                        result.Organizations = result.OrganizationsRoles
                            .Select(or => or.Organization)
                            .Where(o => o is not null)
                            .DistinctBy(o => o!.Id)!;
                        result.OrganizationsId = result.Organizations.Select(o => o.Id);
                    }
                }
                else if (options.IncludeOrganizations)
                {
                    result.Organizations = roleXUserXOrganizationService.GetOrganizationsByUserIdAsync(user.Id)
                        .GetAwaiter()
                        .GetResult()
                        .DistinctBy(o => o.Id);

                    result.OrganizationsId = result.Organizations.Select(o => o.Id);
                }

                return result;
            });

        return users;
    }

    public async Task<SystemUser?> GetSingleOrDefaultAsync(SystemUserQueryOptions? options)
    {
        options ??= new SystemUserQueryOptions();
        options.Take = 2;
        var users = await GetListAsync(options);
        if (users.Count() > 1)
            throw new ThereAreMultipleUsersMatchingTheGivenConditionsException();

        return users.FirstOrDefault();
    }

    public async Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, SystemUserQueryOptions? options = null)
    {
        data.TryGetGuids("GlobalRolesUuid", out var globalRolesUuid);

        List<OrganizationRolesId>? organizationsRolesId = null;
        data.TryGetValue("OrganizationsRolesUuid", out var organizationsRolesUuidDict);

        if (organizationsRolesUuidDict is not null)
        {
            organizationsRolesId = [];
            var rawList = (IEnumerable<object>)organizationsRolesUuidDict;

            foreach (var item in rawList)
            {
                var entry = (Dictionary<string, object?>)item;
                var organizationRolesUuid = new OrganizationRolesId
                {
                    OrganizationId = await organizationService.GetSingleIdByUuidAsync(Guid.Parse(entry["organizationUuid"]!.ToString()!)),
                    RolesId = await roleService.GetListIdByUuidAsync([.. ((IEnumerable<object>)entry["rolesUuid"]!).Select(x => Guid.Parse(x.ToString()!))]),
                };

                organizationsRolesId.Add(organizationRolesUuid);
            }
        }

        var id = await userService.GetSingleIdByUuidAsync(uuid, options);

        await userService.UpdateByIdAsync(id, data.FilterKeys("DisplayName", "Username", "IsActive", "CanLogin"));

        if (data.TryGetString("Password", out var password) && !string.IsNullOrWhiteSpace(password))
            await userPasswordService.CreateOrUpdateByUserIdAsync(password, id);

        if (globalRolesUuid is not null && globalRolesUuid.Any())
        {
            var globalRolesId = await roleService.GetListIdByUuidAsync(globalRolesUuid);
            await roleXUserService.SetAllRolesIdForUserIdAsync(globalRolesId, id);
        }

        if (organizationsRolesId is not null)
            await roleXUserXOrganizationService.SetOrganizationsRolesIdForUserIdAsync(organizationsRolesId, id);

        return 1;
    }

    public async Task<int> DeleteByUuidAsync(Guid uuid, SystemUserQueryOptions? options = null)
        => await userService.DeleteByUuidAsync(uuid);

    public async Task<int> RestoreByUuidAsync(Guid uuid, SystemUserQueryOptions? options = null)
        => await userService.RestoreByUuidAsync(uuid);

    public async Task<SystemUser> Translate(SystemUser user, string? context = null)
    {
        user = user.Clone();

        if (user.Type is not null)
            user.Type = await userTypeService.Translate(user.Type!);

        if (user.SystemRoles is not null)
            user.SystemRoles = await roleService.Translate(user.SystemRoles);

        if (user.OrganizationsRoles is not null)
        {
            user.OrganizationsRoles = await Task.WhenAll(user.OrganizationsRoles.Select(async or => new OrganizationRoles
            {
                OrganizationId = or.OrganizationId,
                RolesId = or.RolesId,
                Organization = or.Organization is not null ? await organizationService.Translate(or.Organization) : null,
                Roles = or.Roles is not null ? await roleService.Translate(or.Roles) : null
            }));
        }

        return user;
    }

    public async Task<IEnumerable<SystemUser>> Translate(IEnumerable<SystemUser> users, string? context = null)
        => await Task.WhenAll(users.Select(user => Translate(user, context)));
}
