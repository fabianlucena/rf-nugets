using RFService.Libs;
using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    [Index(nameof(Uuid), IsUnique = true)]
    public abstract class EntitySoftDeleteLogIdUuid
        : EntitySoftDeleteLogId
    {
        [Required]
        public Guid Uuid { get; set; } = default;
    }
}