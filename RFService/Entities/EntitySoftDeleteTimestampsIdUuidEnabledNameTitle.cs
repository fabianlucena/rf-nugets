using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestampsIdUuidEnabledNameTitle
        : EntitySoftDeleteTimestampsIdUuidEnabledName
    {
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }
    }
}