using RFAuth.Entities;
using RFBase.ILibs;
using RFRBAC.DTO;
using RFRBAC.IServices;
using RFRegisterService.Attributes;
using System.Xml.Linq;

namespace RFRBAC.Services;

[RegisterService]
public class RPDataService : IRPDataService
{
    public async Task<RPData> GetSingleBySession(Session session)
    {
        var rpData = new RPData();
        rpData.RoleNames = [..rpData.RoleNames, "test"];
        return rpData;
    }

    public async Task<Session> DecorateSession(Session session)
    {
        session = new Session(session);

        var rpData = await GetSingleBySession(session);

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
