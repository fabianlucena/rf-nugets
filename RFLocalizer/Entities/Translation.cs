using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Security;

namespace RFLocalizer.Entities
{
    [Table("Translations", Schema = "loc")]
    [Index(nameof(LanguageId), nameof(ContextId), nameof(SourceId), IsUnique = true)]
    public class Translation
        : EntityIdUuid
    {
        [Required]
        [ForeignKey("Language")]
        public Int64 LanguageId { get; set; }
        public Language? Language { get; set; }

        [Required]
        [ForeignKey("Context")]
        public Int64 ContextId { get; set; }
        public Context? Context { get; set; }

        [Required]
        [ForeignKey("Source")]
        public Int64 SourceId { get; set; }
        public Source? Source { get; set; }

        public required string Text { get; set; }
    }
}