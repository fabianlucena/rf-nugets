using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteCreatedAtIdUuid
        : EntitySoftDeleteCreatedAtId
    {
        [Required]
        public Guid Uuid { get; set; }
    }
}