using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFTransactionLog.Entities
{
    [Table("Actions", Schema = "log")]
    [Index(nameof(Name), IsUnique = true)]
    public class LogAction
        : EntityIdUuidName
    {
    }

    public enum TLAction
    {
        GET = 0,
        ADD,
        EDIT,
        DELETE,
        RESTORE,
    }
}