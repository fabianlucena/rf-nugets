using RFAuth.Entities;
using RFRBAC.DTO;
using RFRBAC.IServices;
using RFRegisterService.Attributes;

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
        var rpData = await GetSingleBySession(session);

        dynamic sessionData = session.Data;

        sessionData.RoleIds ??= new List<long>();
        var roleIds = sessionData.RoleIds as List<long>;
        roleIds?.AddRange(rpData.RoleIds);

        sessionData.RoleNames ??= new List<string>();
        var roleNames = sessionData.RoleNames as List<string>;
        roleNames?.AddRange(rpData.RoleNames);

        sessionData.PermissionNames ??= new List<string>();
        var permissionNames = sessionData.PermissionNames as List<string>;
        permissionNames?.AddRange(rpData.PermissionNames);

        dynamic sessionDataResponse = session.DataResponse;

        sessionDataResponse.RoleNames ??= new List<string>();
        roleNames = sessionDataResponse.RoleNames as List<string>;
        roleNames?.AddRange(rpData.RoleNames);

        sessionDataResponse.PermissionNames ??= new List<string>();
        permissionNames = sessionDataResponse.PermissionNames as List<string>;
        permissionNames?.AddRange(rpData.PermissionNames);

        return session;
    }
}
