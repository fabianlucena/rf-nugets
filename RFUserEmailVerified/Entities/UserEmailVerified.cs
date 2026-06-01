using RFAuth.Entities;
using RFService.Entities;
using RFService.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFUserEmailVerified.Entities
{
    [Table("UsersEmailsVerified", Schema = "auth")]
    [Index(nameof(UserId), IsUnique = true)]
    public class UserEmailVerified
        : EntityTimestampsIdUuid
    {
        [Required]
        [ForeignKey("User")]
        public required Int64 UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public required string Email { get; set; }

        [Required]
        public bool IsVerified { get; set; } = false;
    }
}
