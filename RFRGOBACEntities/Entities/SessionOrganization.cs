using RFAuthEntities.Entities;
using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBACEntities.Entities
{
    [Table("SessionOrganizations", Schema = "auth")]
    public class SessionOrganization : NoIdEntity
    {
        [Key]
        public long SessionId { get; set; }
        public Session? Session { get; set; }

        public long OrganizationId { get; set; }
        public Organization? Organization { get; set; }
    }
}
