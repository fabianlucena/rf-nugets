using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestamps
        : EntitySoftDelete
    {
        [Required]
        public DateTime CreatedAt { get; set; } = default;

        [Required]
        public DateTime UpdatedAt { get; set; } = default;
    }
}