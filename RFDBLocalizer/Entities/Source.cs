using RFService.Entities;
using RFService.Attributes;
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