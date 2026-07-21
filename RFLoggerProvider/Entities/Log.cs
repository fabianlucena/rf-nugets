using RFAuth.Entities;
using RFEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFLoggerProvider.Entities
{
    [Table("TransactionsLog", Schema = "log")]
    public sealed class Log
        : CommonEntity
    {
        [Required]
        public DateTime LogTimestamp { get; set; } = default;

        [ForeignKey("Level")]
        public long? LevelId { get; set; }
        public LogLevel? Level { get; set; }

        [ForeignKey("Module")]
        public long? ModuleId { get; set; }
        public LogModule? Module { get; set; }

        [ForeignKey("Action")]
        public long? ActionId { get; set; }
        public LogAction? Action { get; set; }

        [ForeignKey("Session")]
        public long? SessionId { get; set; }
        public Session? Session { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }
        public User? User { get; set; }

        [MaxLength(-1)]
        public string? Message { get; set; }

        [MaxLength(-1)]
        public string? JsonData { get; set; }

        public Log() { }

        public Log(Log? entity) : base(entity) { }

        public override Log Clone()
            => new(this);
    }
}