using RFService.Entities;
using RFService.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFLoggerProvider.Entities
{
    [Table("Actions", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public class LogAction
        : EntityIdUuidName
    {
    }
}