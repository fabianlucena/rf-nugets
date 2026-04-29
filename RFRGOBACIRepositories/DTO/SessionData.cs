using RFRGOBACEntities.Entities;

namespace RFRGOBACIRepositories.DTO
{
    public class SessionData
    {
        public IEnumerable<Company> Companies { get; set; } = [];
        public Company? CurrentCompany { get; set; }
        public IEnumerable<long>? RolesId { get; set; }
        public IEnumerable<string>? RolesNames { get; set; }
        public IEnumerable<string>? PermissionsNames { get; set; }
    }
}
