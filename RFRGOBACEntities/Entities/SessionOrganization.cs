using RFAuthEntities.Entities;
using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBACEntities.Entities
{
    [Table("SessionOrganizations", Schema = "auth")]
    public sealed class SessionOrganization : NoIdEntity
    {
        [Key]
        public long SessionId { get; set; }
        public Session? Session { get; set; }

        public long OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        public SessionOrganization() { }

        public SessionOrganization(SessionOrganization? session = null)
            : base(session)
        {
            if (session == null)
                return;

            SessionId = session.SessionId;
            Session = session.Session;

            OrganizationId = session.OrganizationId;
            Organization = session.Organization;
        }

        public override SessionOrganization Clone()
            => new(this);
    }
}
