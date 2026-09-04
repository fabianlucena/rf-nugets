namespace RFRBAC.DTO;

public class RPDataResponse(RPData rpData)
{
    public IEnumerable<string> RolesName { get; set; } = rpData.RolesName;
    public IEnumerable<string> PermissionsName { get; set; } = rpData.PermissionsName;
}
