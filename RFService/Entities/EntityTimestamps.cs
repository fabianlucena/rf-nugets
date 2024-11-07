using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityTimestamps : Entity
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}