using RFBaseEntities.Entities;
using RFRBACEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBACEntities.Entities
{
    [Table("RolesXUsersXOrganizations", Schema = "auth")]
    public sealed class RoleXUserXOrganization : CommonJoin
    {
        public long RoleId { get; set; }
        public Role? Role { get; set; }

        public long UserId { get; set; }
        public User? User { get; set; }

        public long OrganizationId { get; set; }
        public Organization? Organization { get; set; }

        public RoleXUserXOrganization() { }

        public RoleXUserXOrganization(RoleXUserXOrganization? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            RoleId = entity.RoleId;
            Role = entity.Role;

            UserId = entity.UserId;
            User = entity.User;

            OrganizationId = entity.OrganizationId;
            Organization = entity.Organization;
        }

        public override RoleXUserXOrganization Clone()
            => new(this);
    }
}
