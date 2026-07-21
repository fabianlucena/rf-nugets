using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Sources", Schema = "loc")]
    [Index(nameof(Text), IsUnique = true)]
    public sealed class Source
        : NominableEntity
    {
        [Required]
        public string Text { get; set; } = string.Empty;
        

        public Source() : base() { }

        public Source(Source? entity)
            : base(entity)
        {
            if (entity == null)
                return;

            this.Text = entity.Text;
        }

        public override Source Clone()
            => new(this);
    }
}