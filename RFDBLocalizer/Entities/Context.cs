using RFService.Entities;
using RFService.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Contexts", Schema = "loc")]
    [Index(nameof(Name), IsUnique = true)]
    public class Context
        : EntityIdUuidName
    {
    }
}