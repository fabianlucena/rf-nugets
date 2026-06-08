using RFBase.Libs;

namespace RFRBAC.DTO;

public class RPDataResponse
{
    public DataDictionary Data { get; set; } = [];

    public RPDataResponse(RPData orpgData)
    {
        if (orpgData.RoleNames is not null)
            Data["roles"] = orpgData.RoleNames;

        if (orpgData.PermissionNames is not null)
            Data["permissions"] = orpgData.PermissionNames;
    }
}
