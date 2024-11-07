using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFAuth.Entities
{
    [Table("Users", Schema = "auth")]
    [Index(nameof(Username), IsUnique = true)]
    public class User : EntityTimestampsIdUuidEnabled
    {
        [Required]
        [ForeignKey("Type")]
        public required Int64 TypeId { get; set; }
        public UserType? Type { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public required string FullName { get; set; }
    }
}