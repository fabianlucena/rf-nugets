using RFService.Entities;
using RFService.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Users", Schema = "auth")]
    [Index(nameof(Username), IsUnique = true)]
    public class User
        : EntitySoftDeleteTimestampsIdUuidEnabled
    {
        [Required]
        [ForeignKey("Type")]
        public Int64 TypeId { get; set; } = default;
        public UserType? Type { get; set; } = default;

        [Required]
        [MaxLength(255)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string FullName { get; set; } = string.Empty;
    }
}