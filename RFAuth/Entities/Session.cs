using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Sessions", Schema = "auth")]
    [Index(nameof(Token), IsUnique = true)]
    [Index(nameof(AutoLoginToken), IsUnique = true)]
    public class Session : EntityTimestampsIdUuid
    {
        [Required]
        [ForeignKey("User")]
        public Int64 UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey("Device")]
        public Int64 DeviceId { get; set; }
        public Device? Device { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Token { get; set; }

        [MaxLength(255)]
        public required string AutoLoginToken { get; set; }

        public DateTime? ClosedAt { get; set; }
    }
}