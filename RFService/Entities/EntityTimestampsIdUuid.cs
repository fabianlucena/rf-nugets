using RFService.Services;
using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    [Index(nameof(Uuid), IsUnique = true)]
    public abstract class EntityTimestampsIdUuid : EntityTimestampsId
    {
        [Required]
        public Guid? Uuid { get; set; }
    }
}