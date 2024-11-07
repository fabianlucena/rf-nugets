using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntityTimestampsIdUuidEnabledNameTitleTranslatable : EntityTimestampsIdUuidEnabledNameTitle
    {
        [Required]
        public bool? IsTranslatable { get; set; }
    }
}