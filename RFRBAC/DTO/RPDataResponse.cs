using RFBase.Libs;

namespace RFRBAC.DTO;

public class RPDataResponse(RPData rpData)
{
    public IEnumerable<string> RoleNames { get; set; } = rpData.RoleNames;
    public IEnumerable<string> PermissionNames { get; set; } = rpData.PermissionNames;
}
