using RFService.Services;
using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestampsIdUuidEnabledName
        : EntitySoftDeleteTimestampsIdUuidEnabled
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}