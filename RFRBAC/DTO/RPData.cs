namespace RFRBAC.DTO;

public class RPData
{
    public IEnumerable<long> RolesId { get; set; } = [];
    public IEnumerable<string> RolesName { get; set; } = [];
    public IEnumerable<string> PermissionsName { get; set; } = [];
}
