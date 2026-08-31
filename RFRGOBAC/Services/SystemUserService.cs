using RFIServices.IServices;
using RFRegisterService.Attributes;
using RFRGOBAC.DTO;
using RFRGOBAC.IServices;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.Services;


[RegisterService]
public class SystemUserService(
        IUserService userService
    )
    : ISystemUserService
{
    public async Task<IEnumerable<OrganizationUser>> GetListAsync(OrganizationUserQueryOptions? options)
    {
        options ??= new OrganizationUserQueryOptions();
        var users = (await userService
            .GetListAsync(options))
            .Select(user =>
            {
                return new OrganizationUser(user);
            });

        return users;
    }

    public async Task<OrganizationUser?> GetSingleOrDefaultAsync(OrganizationUserQueryOptions? options)
    {
        options ??= new OrganizationUserQueryOptions();
        var user = await userService.GetSingleOrDefaultAsync(options);
        if (user == null)
            return null;

        return new OrganizationUser(user);
    }
}
