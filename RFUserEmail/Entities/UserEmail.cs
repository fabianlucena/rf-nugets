using RFAuth.Entities;
using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFUserEmail.Entities
{
    [Table("UsersEmails", Schema = "auth")]
    [Index(nameof(UserId), IsUnique = true)]
    public class UserEmail : EntityTimestampsIdUuid
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
