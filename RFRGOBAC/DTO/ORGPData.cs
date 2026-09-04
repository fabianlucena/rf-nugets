using RFRGOBAC.Entities;

namespace RFRGOBAC.DTO;

public class ORGPData
{
    public Organization? CurrentOrganization { get; set; }
    public IEnumerable<Organization> Organizations { get; set; } = [];
    public IEnumerable<long>? GroupsId { get; set; }
    public IEnumerable<string>? GroupsName { get; set; }
    public IEnumerable<long>? RolesId { get; set; }
    public IEnumerable<string>? RolesName { get; set; }
    public IEnumerable<string>? PermissionsName { get; set; }
}
