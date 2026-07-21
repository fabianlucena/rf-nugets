using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Translations", Schema = "loc")]
    [Index(nameof(LanguageId), nameof(ContextId), nameof(SourceId), IsUnique = true)]
    public sealed class Translation
        : CommonEntity
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

        [Required]
        public string Text { get; set; } = string.Empty;

        public Translation() : base() { }

        public Translation(Translation? entity)
            : base(entity)
        {
            if (entity == null)
                return;

            LanguageId = entity.LanguageId;
            Language = entity.Language?.Clone();
            ContextId = entity.ContextId;
            Context = entity.Context?.Clone();
            SourceId = entity.SourceId;
            Source = entity.Source?.Clone();
            Text = entity.Text;
        }

        public override Translation Clone()
            => new(this);
    }
}