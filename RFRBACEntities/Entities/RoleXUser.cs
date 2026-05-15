using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBACEntities.Entities
{
    [Table("RolesXUsers", Schema = "auth")]
    public sealed class RoleXUser : CommonJoin
    {
        public long RoleId { get; set; }
        public Role? Role { get; set; }

        public long UserId { get; set; }
        public User? User { get; set; }

        public RoleXUser() { }

        public RoleXUser(RoleXUser? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            RoleId = entity.RoleId;
            Role = entity.Role;

            UserId = entity.UserId;
            User = entity.User;
        }

        public override RoleXUser Clone()
            => new(this);
    }
}
