using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFUserEmailVerified.Entities
{
    [Table("UsersEmailsVerified", Schema = "auth")]
    [Index(nameof(UserId), IsUnique = true)]
    public class UserEmailVerified : CommonEntity
    {
        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public bool IsVerified { get; set; } = false;

        public UserEmailVerified() { }

        public UserEmailVerified(UserEmailVerified? entity)
            : base(entity)
        {
            if (entity is null)
                return;

            UserId = entity.UserId;
            Email = entity.Email;
            IsVerified = entity.IsVerified;
        }

        public override UserEmailVerified Clone()
            => new(this);
    }
}
