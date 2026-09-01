using RFBase.ILibs;
using RFIServices.IServices;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.Services;


[RegisterService]
public class SystemUserService(
    IUserService userService,
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

        if (user.GlobalRolesId is not null)
        {
            await roleXUserService.SetAllRolesIdForUserIdAsync(user.GlobalRolesId, result.Id);
        }

        if (user.OrganizationsRolesId is not null)
        {
            await roleXUserXOrganizationService.SetAllOrganizationsRolesIdForUserIdAsync(user.OrganizationsRolesId, result.Id);
        }

        return result;
    }

    public async Task<IEnumerable<SystemUser>> GetListAsync(SystemUserQueryOptions? options)
    {
        options ??= new SystemUserQueryOptions();
        var users = (await userService
            .GetListAsync(options))
            .Select(user =>
            {
                return new SystemUser(user);
            });

        return users;
    }

    public async Task<SystemUser?> GetSingleOrDefaultAsync(SystemUserQueryOptions? options)
    {
        options ??= new SystemUserQueryOptions();
        var user = await userService.GetSingleOrDefaultAsync(options);
        if (user == null)
            return null;

        return new SystemUser(user);
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
