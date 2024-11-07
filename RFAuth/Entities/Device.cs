using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Devices", Schema = "auth")]
    [Index(nameof(Token), IsUnique = true)]
    public class Device : EntityTimestampsIdUuid
    {
        [MaxLength(255)]
        public required string Token{ get; set; }
    }
}