using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestampsIdUuidName
        : EntitySoftDeleteTimestampsIdUuid
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}