using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteTimestampsId
        : EntitySoftDeleteTimestamps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; } = default;
    }
}