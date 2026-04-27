using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFPermissionsEntities.Entities
{
    [Table("Permissions", Schema = "auth")]
    public class Permission : ImmutableEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
