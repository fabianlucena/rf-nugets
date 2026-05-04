using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuthEntities.Entities
{
    [Table("UserPasswords", Schema = "auth")]
    public sealed class UserPassword
        : NoIdEntity
    {
        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public UserPassword() { }

        public UserPassword(UserPassword? userPassword = null)
            : base(userPassword)
        {
            if (userPassword == null)
                return;

            UserId = userPassword.UserId;
            User = userPassword.User;

            PasswordHash = userPassword.PasswordHash;
        }

        public override UserPassword Clone()
            => new(this);
    }
}