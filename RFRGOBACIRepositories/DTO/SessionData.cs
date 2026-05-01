using RFRGOBACEntities.Entities;

namespace RFRGOBACIRepositories.DTO
{
    public class SessionData
    {
        public IEnumerable<Organization> Organizations { get; set; } = [];
        public Organization? CurrentOrganization { get; set; }
        public IEnumerable<long>? RoleIds { get; set; }
        public IEnumerable<string>? RoleNames { get; set; }
        public IEnumerable<string>? PermissionNames { get; set; }
    }
}
