using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBACEntities.Entities
{
    [Table("PermissionsXRoles", Schema = "auth")]
    public class PermissionXRole : CommonJoin
    {
        public long PermissionId { get; set; }

        public long RoleId { get; set; }


        public Role? Permission { get; set; }

        public Role? Role { get; set; }
    }
}
