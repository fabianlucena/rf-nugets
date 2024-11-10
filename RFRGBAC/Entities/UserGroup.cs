using RFAuth.Entities;
using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGBAC.Entities
{
    [Table("Users_Groups", Schema = "auth")]
    [Index(nameof(UserId), nameof(GroupId), IsUnique = true)]
    public class UserGroup : EntityTimestamps
    {
        [Required]
        [ForeignKey("User")]
        public required long UserId { get; set; }
        public User? User { get; set; }

        [Required]
        [ForeignKey("Group")]
        public required long GroupId { get; set; }
        public User? Group { get; set; }
    }
}