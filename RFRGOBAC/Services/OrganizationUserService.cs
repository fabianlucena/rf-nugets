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
    //IRoleXUserService roleXUserService,
    IRoleXUserXOrganizationService roleXUserXOrganizationService //,
    //IOrganizationService organizationService
) : IOrganizationUserService
{
    public async Task<OrganizationUser> CreateAsync(OrganizationUser user)
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
                        OrganizationId = 10,
                        RolesId = user.RolesId,
                    },
                ],
                result.Id
            );
        }

        return result;
    }

    public Task<int> DeleteByUuidAsync(Guid uuid, OrganizationUserQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<OrganizationUser>> GetListAsync(OrganizationUserQueryOptions? options)
    {
        options ??= new OrganizationUserQueryOptions();
        var users = (await roleXUserXOrganizationService.GetUsersAsync(new RoleXUserXOrganizationQueryOptions
        {
            OrganizationId = options.OrganizationId,
        })).Select(user =>
            {
                var result = new OrganizationUser(user);

                if (options.IncludeRoles)
                {
                    result.Roles = roleXUserXOrganizationService.GetRolesAsync(new RoleXUserXOrganizationQueryOptions
                    {
                        OrganizationId = options.OrganizationId,
                        UserId = result.Id,
                    })
                        .GetAwaiter()
                        .GetResult();

                    result.RolesId = result.Roles.Select(r => r.Id);
                }

                return result;
            });

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

    public Task<int> RestoreByUuidAsync(Guid uuid, OrganizationUserQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public async Task<OrganizationUser> Translate(OrganizationUser user, string? context = null)
    {
        {
            user = user.Clone();

            if (user.Type is not null)
                user.Type = await userTypeService.Translate(user.Type!);

            if (user.Roles is not null)
                user.Roles = await roleService.Translate(user.Roles);

            return user;
        }
    }

    public async Task<IEnumerable<OrganizationUser>> Translate(IEnumerable<OrganizationUser> users, string? context = null)
        => await Task.WhenAll(users.Select(user => Translate(user, context)));

    public Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, OrganizationUserQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }
}
