using RFUserEmail.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFUserEmailVerified.Entities
{
    [Table("UsersEmailsVerified", Schema = "auth")]
    public class UserEmailVerified
        : UserEmail
    {
        [Required]
        public bool IsVerified { get; set; } = false;
    }
}
