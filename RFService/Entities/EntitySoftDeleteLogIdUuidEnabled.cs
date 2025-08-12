using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteLogIdUuidEnabled
        : EntitySoftDeleteLogIdUuid
    {
        [Required]
        public bool IsEnabled { get; set; } = true;
    }
}