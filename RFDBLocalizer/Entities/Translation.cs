using RFLocalizer.Entities;
using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Translations", Schema = "loc")]
    [Index(nameof(LanguageId), nameof(ContextId), nameof(SourceId), IsUnique = true)]
    public class Translation
        : EntityIdUuid
    {
        [Required]
        [ForeignKey("Language")]
        public long LanguageId { get; set; }
        public Language? Language { get; set; }

        [Required]
        [ForeignKey("Context")]
        public long ContextId { get; set; }
        public Context? Context { get; set; }

        [Required]
        [ForeignKey("Source")]
        public long SourceId { get; set; }
        public Source? Source { get; set; }

        public required string Text { get; set; }
    }
}