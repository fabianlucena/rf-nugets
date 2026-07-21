using RFRGOBAC.Entities;

namespace RFRGOBAC.DTO;

public class ORGPData
{
    public IEnumerable<Organization> Organizations { get; set; } = [];
    public Organization? CurrentOrganization { get; set; }
    public IEnumerable<long>? GroupIds { get; set; }
    public IEnumerable<string>? GroupNames { get; set; }
    public IEnumerable<long>? RoleIds { get; set; }
    public IEnumerable<string>? RoleNames { get; set; }
    public IEnumerable<string>? PermissionNames { get; set; }
}
