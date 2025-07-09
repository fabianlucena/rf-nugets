using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityLogIdUuidEnabled
        : EntityLogIdUuid
    {
        [Required]
        public bool IsEnabled { get; set; } = true;
    }
}