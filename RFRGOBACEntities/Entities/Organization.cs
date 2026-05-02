using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBACEntities.Entities
{
    [Table("Organizations", Schema = "auth")]
    public class Organization : LocalizableEntity
    {
        public bool IsActive { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
