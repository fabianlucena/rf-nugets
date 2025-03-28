using RFAuth.Entities;
using RFService.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFTransactionLog.Entities
{
    [Table("TransactionsLog", Schema = "log")]
    public class TransactionLog
        : EntityIdUuid
    {
        [Required]
        public DateTime LogTimestamp { get; set; } = default;

        [ForeignKey("Level")]
        public long LevelId { get; set; }
        public LogLevel? Level { get; set; }

        [ForeignKey("Action")]
        public long? ActionId { get; set; }
        public LogAction? Action { get; set; }

        [ForeignKey("Session")]
        public long? SessionId { get; set; }
        public Session? Session { get; set; }

        [ForeignKey("User")]
        public User? User { get; set; }

        [MaxLength(-1)]
        public string? Message { get; set; }

        [MaxLength(-1)]
        public string? JsonData { get; set; }
    }
}