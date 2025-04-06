using RFService.Entities;
using RFService.Libs;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFDBLocalizer.Entities
{
    [Table("Sources", Schema = "loc")]
    [Index(nameof(Text), IsUnique = true)]
    public class Source
        : EntityIdUuid
    {
        public required string Text { get; set; }
    }
}