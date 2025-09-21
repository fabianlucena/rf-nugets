using RFService.Attributes;
using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    [Index(nameof(Uuid), IsUnique = true)]
    public abstract class EntitySoftDeleteCreatedAtUuid
        : EntitySoftDeleteCreatedAt
    {
        [Required]
        public Guid Uuid { get; set; } = default;
    }
}