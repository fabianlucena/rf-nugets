using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFLoggerProvider.Entities
{
    [Table("Levels", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public class LogLevel
        : EntityIdUuidName
    {
    }
}