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
public class SystemUserService(
    IUserService userService,
    IUserPasswordService userPasswordService,
    IUserTypeService userTypeService,
    IRoleXUserService roleXUserService,
    IRoleXUserXOrganizationService roleXUserXOrganizationService
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

        if (user.GlobalRolesId is not null)
            await roleXUserService.SetAllRolesIdForUserIdAsync(user.GlobalRolesId, result.Id);

        if (user.OrganizationsRolesId is not null)
            await roleXUserXOrganizationService.SetAllOrganizationsRolesIdForUserIdAsync(user.OrganizationsRolesId, result.Id);

        return result;
    }

    public async Task<IEnumerable<SystemUser>> GetListAsync(SystemUserQueryOptions? options)
    {
        options ??= new SystemUserQueryOptions();
        var users = await Task.WhenAll((await userService
            .GetListAsync(options))
            .Select(async user =>
            {
                var result = new SystemUser(user);
                if (options.IncludeGlobalRoles)
                    result.GlobalRoles = await roleXUserService.GetRolesByUserIdAsync(user.Id);

                if (options.IncludeOrganizationsRoles)
                    result.OrganizationsRoles = await roleXUserXOrganizationService.GetOrganizationsRolesByUserIdAsync(user.Id);

                return result;
            }));

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

    public Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data, SystemUserQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteByUuidAsync(Guid uuid, SystemUserQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> RestoreByUuidAsync(Guid uuid, SystemUserQueryOptions? options = null)
    {
        throw new NotImplementedException();
    }
}
