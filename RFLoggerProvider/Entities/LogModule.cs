using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFLoggerProvider.Entities
{
    [Table("Modules", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public class LogModule
        : EntityIdUuidName
    {
    }
}