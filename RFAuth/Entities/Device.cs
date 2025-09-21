using RFService.Entities;
using RFService.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Devices", Schema = "auth")]
    [Index(nameof(Token), IsUnique = true)]
    public class Device
        : EntitySoftDeleteTimestampsIdUuid
    {
        [MaxLength(255)]
        public required string Token { get; set; }
    }
}