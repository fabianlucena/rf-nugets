using System.ComponentModel.DataAnnotations.Schema;
using RFEntities.Attributes;
using RFEntities.Entities;

namespace RFLoggerProvider.Entities
{
    [Table("Actions", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public sealed class LogAction
        : NominableEntity
    {
        public LogAction() { }

        public LogAction(LogAction? entity) : base(entity) { }

        public override LogAction Clone()
            => new(this);
    }
}