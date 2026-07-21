namespace RFRBAC.DTO;

public class RPData
{
    public IEnumerable<long> RoleIds { get; set; } = [];
    public IEnumerable<string> RoleNames { get; set; } = [];
    public IEnumerable<string> PermissionNames { get; set; } = [];
}
