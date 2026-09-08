using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFEntities.Entities;

public abstract class OwnedEntity : CommonEntity
{

    [Required]
    [ForeignKey("Owner")]
    public long OwnerId { get; set; } = default;
    public User? Owner { get; set; } = default;

    public OwnedEntity() { }

    public OwnedEntity(OwnedEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        OwnerId = entity.OwnerId;
        Owner = entity.Owner;
    }
}
