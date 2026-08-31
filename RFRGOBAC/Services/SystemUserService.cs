using RFBase.ILibs;
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
    public Task<SystemUser> CreateAsync(SystemUser user)
    {
        throw new NotImplementedException();
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
