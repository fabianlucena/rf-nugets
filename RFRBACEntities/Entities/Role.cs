using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBACEntities.Entities
{
    [Table("Roles", Schema = "auth")]
    public class Role : LocalizableEntity
    {
    }
}
