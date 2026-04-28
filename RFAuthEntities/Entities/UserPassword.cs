using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuthEntities.Entities
{
    [Table("UserPasswords", Schema = "auth")]
    public class UserPassword
        : NoIdEntity
    {
        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [MaxLength(255)]
        public string Hash { get; set; }
    }
}