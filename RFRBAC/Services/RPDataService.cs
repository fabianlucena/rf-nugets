using RFAuth.Entities;
using RFBase.ILibs;
using RFRBAC.DTO;
using RFRBAC.Exceptions;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFRegisterService.Attributes;

namespace RFRBAC.Services;

[RegisterService]
public class RPDataService(
    IRoleXUserService roleXUserService,
    IRoleService roleService,
    IPermissionXRoleService permissionXRoleService
) : IRPDataService
{
    public async Task<RPData?> GetSingleOrDefaultBySession(Session session, RPDataQueryOptions? options = null)
    {
        var userId = session.UserId;
        if (userId <= 0)
            return null;

#pragma warning disable IDE0017 // Simplify object initialization
        var rpData = new RPData();
#pragma warning restore IDE0017 // Simplify object initialization

        rpData.RoleIds = await roleXUserService.GetAllRolesIdByUserIdAsync(userId);
        rpData.RoleNames = await roleService.GetNamesByIdsAsync(rpData.RoleIds);
        rpData.PermissionNames = await permissionXRoleService.GetPermissionNamesByRoleIdsAsync(rpData.RoleIds);

        return rpData;
    }

    public async Task<RPData> GetSingleBySession(Session session, RPDataQueryOptions? options = null)
        => await GetSingleOrDefaultBySession(session, options)
            ?? throw new NoRPDataFoundForSessionException(session.Id);

    public async Task<Session> DecorateSession(Session session, RPDataQueryOptions? options = null)
    {
        session = new Session(session);

        var rpData = await GetSingleBySession(session, options);

        CombineLongs(session.InternalData, "RolesIds", rpData.RoleIds);
        CombineStrings(session.InternalData, "RoleNames", rpData.RoleNames);
        CombineStrings(session.InternalData, "PermissionNames", rpData.PermissionNames);

        CombineStrings(session.ResponseData, "roles", rpData.RoleNames);
        CombineStrings(session.ResponseData, "permissions", rpData.PermissionNames);

        return session;
    }

    private static bool CombineLongs(IDataDictionary currentData, string dataName, IEnumerable<long> newData)
    {
        if (!newData.Any())
            return false;

        HashSet<long> set;
        if (currentData.TryGetValue(dataName, out var value) && value is not null)
        {
            var enumerable = value as IEnumerable<long>
                ?? throw new Exception($"Current data is not of type IEnumerable<long> for {dataName}");

            set = [.. enumerable];
        }
        else
        {
            set = [];
        }

        foreach (var item in newData)
            set.Add(item);

        currentData[dataName] = set.ToList();
        
        return true;
    }

    private static bool CombineStrings(IDataDictionary currentData, string dataName, IEnumerable<string> newData)
    {
        if (!newData.Any())
            return false;

        HashSet<string> set;
        if (currentData.TryGetValue(dataName, out var value) && value is not null)
        {
            var enumerable = value as IEnumerable<string>
                ?? throw new Exception($"Current data is not of type IEnumerable<string> for {dataName}");

            set = [.. enumerable];
        }
        else
        {
            set = [];
        }

        foreach (var item in newData)
            set.Add(item);

        currentData[dataName] = set.ToList();

        return true;
    }
}
