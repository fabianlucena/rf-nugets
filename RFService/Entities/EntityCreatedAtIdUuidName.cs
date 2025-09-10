using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityCreatedAtIdUuidName
        : EntityCreatedAtIdUuid
    {
        [Required]
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}