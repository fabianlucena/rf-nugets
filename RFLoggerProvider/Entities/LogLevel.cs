using System.ComponentModel.DataAnnotations.Schema;
using RFEntities.Attributes;
using RFEntities.Entities;

namespace RFLoggerProvider.Entities
{
    [Table("Levels", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public sealed class LogLevel : NominableEntity
    {
        public LogLevel() { }

        public LogLevel(LogLevel? entity) : base(entity) { }

        public override LogLevel Clone()
            => new(this);
    }
}