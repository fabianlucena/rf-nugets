using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("PermissionsXRoles", Schema = "auth")]
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true)]
    public sealed class PermissionXRole : CommonJoin
    {
        public long PermissionId { get; set; }
        public Role? Permission { get; set; }

        public long RoleId { get; set; }
        public Role? Role { get; set; }

        public PermissionXRole() { }

        public PermissionXRole(PermissionXRole? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            PermissionId = entity.PermissionId;
            Permission = entity.Permission;

            RoleId = entity.RoleId;
            Role = entity.Role;
        }

        public override PermissionXRole Clone()
            => new(this);
    }
}
