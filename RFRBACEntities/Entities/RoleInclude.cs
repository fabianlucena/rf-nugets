using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBACEntities.Entities
{
    [Table("RolesIncludes", Schema = "auth")]
    public sealed class RoleInclude : CommonJoin
    {
        public long RoleId { get; set; }
        public Role? Role { get; set; }

        public long IncludeId { get; set; }
        public Role? Include { get; set; }

        public RoleInclude() { }

        public RoleInclude(RoleInclude? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            RoleId = entity.RoleId;
            Role = entity.Role;

            IncludeId = entity.IncludeId;
            Include = entity.Include;
        }

        public override RoleInclude Clone()
            => new(this);
    }
}
