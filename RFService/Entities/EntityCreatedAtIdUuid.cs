using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityCreatedAtIdUuid
        : EntityCreatedAtId
    {
        [Required]
        public Guid Uuid { get; set; }
    }
}