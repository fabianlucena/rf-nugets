using RFService.Services;
using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public abstract class EntityTimestampsIdUuidEnabledName : EntityTimestampsIdUuidEnabled
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}