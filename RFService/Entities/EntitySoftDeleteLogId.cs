using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFService.Entities
{
    public abstract class EntitySoftDeleteLogId
        : EntitySoftDelete
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; } = default;

        [Required]
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("UpdatedOf")]
        public Int64? UpdatedOfId { get; set; } = null;
        public EntityLogId? UpdatedOf { get; set; } = null;
    }
}