using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFLocalizer.Entities
{
    [Table("Contexts", Schema = "loc")]
    [Index(nameof(Name), IsUnique = true)]
    public class Context
        : EntityIdUuidName
    {
    }
}