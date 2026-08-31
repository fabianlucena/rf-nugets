using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFEntities.Entities
{
    [Table("Users", Schema = "auth")]
    public class User : CommonEntity
    {
        [Required]
        [ForeignKey("Type")]
        public long TypeId { get; set; } = default;
        public UserType? Type { get; set; } = default;

        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool CanLogin { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }

        public User() { }

        public User(User? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            TypeId = entity.TypeId;
            Type = entity.Type;
            Username = entity.Username;
            DisplayName = entity.DisplayName;
            IsActive = entity.IsActive;
            CanLogin = entity.CanLogin;
            LastLoginAt = entity.LastLoginAt;
        }

        public override User Clone()
            => new(this);
    }
}
