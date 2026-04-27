using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuthEntities.Entities
{
    [Table("Devices", Schema = "auth")]
    public class Device : CreatableEntity
    {
        public string Token { get; set; } = string.Empty;
    }
}
