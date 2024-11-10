using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles_Parent", Schema = "auth")]
    [Index(nameof(RoleId), nameof(ParentId), IsUnique = true)]
    public class RoleParent : Entity
    {
        [Required]
        [ForeignKey("Role")]
        public required Int64 RoleId { get; set; }
        public Role? Role { get; set; }

        [Required]
        [ForeignKey("Parent")]
        public required Int64 ParentId { get; set; }
        public Role? Parent { get; set; }
    }
}