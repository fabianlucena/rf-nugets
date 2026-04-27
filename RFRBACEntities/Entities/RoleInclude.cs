using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBACEntities.Entities
{
    [Table("RolesIncludes", Schema = "auth")]
    public class RoleInclude : CommonJoin
    {
        public long RoleId { get; set; }

        public long IncludeId { get; set; }


        public Role? Role { get; set; }

        public Role? Include { get; set; }
    }
}
