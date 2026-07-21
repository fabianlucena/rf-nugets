using RFEntities.Attributes;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Contexts", Schema = "loc")]
    [Index(nameof(Name), IsUnique = true)]
    public sealed class Context
        : NominableEntity
    {
        public Context() : base() { }

        public Context(Context? entity) : base(entity) { }

        public override Context Clone()
            => new(this);
    }
}