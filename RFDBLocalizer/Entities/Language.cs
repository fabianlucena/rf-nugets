using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Languages", Schema = "loc")]
    [Index(nameof(Name), IsUnique = true)]
    public sealed class Language
        : NominableEntity
    {
        public Language() : base() { }

        public Language(Language? entity) : base(entity) { }

        public override Language Clone()
            => new(this);
    }
}