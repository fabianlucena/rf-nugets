using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestampsIdUuidEnabled : EntitySoftDeleteTimestampsIdUuid
    {
        [Required]
        public bool? IsEnabled { get; set; }
    }
}