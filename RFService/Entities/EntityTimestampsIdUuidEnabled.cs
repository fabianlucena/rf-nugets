using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityTimestampsIdUuidEnabled : EntityTimestampsIdUuid
    {
        [Required]
        public bool? IsEnabled { get; set; }
    }
}