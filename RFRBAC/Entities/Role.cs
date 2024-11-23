using RFService.Entities;
using RFService.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRBAC.Entities
{
    [Table("Roles", Schema = "auth")]
    [Index(nameof(Name), IsUnique = true)]
    public class Role
        : EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
    {
        [Required]
        public bool IsSelectable { get; set; } = false;
    }
}