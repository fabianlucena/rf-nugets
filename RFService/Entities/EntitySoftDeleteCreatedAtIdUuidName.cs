using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteCreatedAtIdUuidName
        : EntitySoftDeleteCreatedAtIdUuid
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}