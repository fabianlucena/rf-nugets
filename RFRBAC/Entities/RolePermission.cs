using RFService.Entities;
using RFService.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles_Permissions", Schema = "auth")]
    [Index(nameof(RoleId), nameof(PermissionId), IsUnique = true)]
    public class RolePermission
        : EntityTimestamps
    {
        [Required]
        [ForeignKey("Role")]
        public Int64 RoleId { get; set; }
        public Role? Role { get; set; }

        [Required]
        [ForeignKey("Permission")]
        public Int64 PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}