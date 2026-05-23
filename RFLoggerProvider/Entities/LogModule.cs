using System.ComponentModel.DataAnnotations.Schema;
using RFEntities.Attributes;
using RFEntities.Entities;

namespace RFLoggerProvider.Entities
{
    [Table("Modules", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public sealed class LogModule
        : NominableEntity
    {
        public LogModule() { }

        public LogModule(LogModule? entity) : base(entity) { }

        public override LogModule Clone()
            => new(this);
    }
}