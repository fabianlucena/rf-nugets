using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityTimestampsIdUuidEnabledUniqueTitle
        : EntityTimestampsIdUuidEnabled
    {
        [Required]
        [MaxLength(255)]
        public required string Title { get; set; }
    }
}