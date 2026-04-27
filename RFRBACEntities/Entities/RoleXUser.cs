using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBACEntities.Entities
{
    [Table("RolesXUsers", Schema = "auth")]
    public class RoleXUser : CommonJoin
    {
        public long RoleId { get; set; }
        public long UserId { get; set; }

        public Role? Role { get; set; }
        public User? User { get; set; }
    }
}
