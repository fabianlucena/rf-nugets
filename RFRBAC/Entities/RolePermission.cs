using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles_Permissions", Schema = "auth")]
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true)]
    public class RolePermission : Entity
    {
        [Required]
        [ForeignKey("Role")]
        public required Int64 RoleId { get; set; }
        public Role? Role { get; set; }

        [Required]
        [ForeignKey("Permission")]
        public required Int64 PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}