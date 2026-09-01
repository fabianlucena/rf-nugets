using RFRBAC.Entities;
using RFRGOBAC.Entities;

namespace RFRGOBAC.DTO;

public class OrganizationRoles
{
    public long OrganizationId { get; set; }
    public Organization? Organization { get; set; }

    public IEnumerable<long> RolesId { get; set; } = [];
    public IEnumerable<Role>? Roles { get; set; }
}
