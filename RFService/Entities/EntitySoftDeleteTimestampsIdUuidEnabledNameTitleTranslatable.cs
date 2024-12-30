using System.ComponentModel.DataAnnotations;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestampsIdUuidEnabledNameTitleTranslatable
        : EntitySoftDeleteTimestampsIdUuidEnabledNameTitle
    {
        [Required]
        public bool IsTranslatable { get; set; } = true;

        public string? TranslationContext { get; set; }
    }
}