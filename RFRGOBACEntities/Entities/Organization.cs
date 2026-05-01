using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBACEntities.Entities
{
    [Table("Organizations", Schema = "auth")]
    public class Organization : NominableEntity
    {
        public string Description { get; set; } = string.Empty;
    }
}
