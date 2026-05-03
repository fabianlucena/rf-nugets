using RFBaseEntities.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFBaseEntities.Entities
{
    [Table("Users", Schema = "auth")]
    public class User : CommonEntity
    {
        public string Username { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool CanLogin { get; set; } = true;
        public DateTime? LastLoginAt { get; set; }

        public User() { }

        public User(User entity)
            : base(entity)
        {
            Username = entity.Username;
            DisplayName = entity.DisplayName;
            IsActive = entity.IsActive;
            CanLogin = entity.CanLogin;
            LastLoginAt = entity.LastLoginAt;
        }

        public override User Clone()
        {
            return new User(this);
        }
    }
}
