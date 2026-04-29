using RFRGOBACEntities.Entities;

namespace RFRGOBACIRepositories.DTO
{
    public class SessionData
    {
        public IEnumerable<Organization> Companies { get; set; } = [];
        public Organization? CurrentOrganization { get; set; }
        public IEnumerable<long>? RolesId { get; set; }
        public IEnumerable<string>? RolesNames { get; set; }
        public IEnumerable<string>? PermissionsNames { get; set; }
    }
}
