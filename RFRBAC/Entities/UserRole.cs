using RFAuth.Entities;
using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Users_Roles", Schema = "auth")]
    [Index(nameof(UserId), nameof(RoleId), IsUnique = true)]
    public class UserRole : EntityTimestamps
    {
        [Required]
        [ForeignKey("User")]
        public required Int64 UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey("Role")]
        public required Int64 RoleId { get; set; }
        public Role? Role { get; set; }
    }
}