namespace RFEntities.Entities;

public abstract class AuditableEntity : CreatableEntity
{
    public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
    public long UpdatedById { get; set; } = 0;
    public User? UpdatedBy { get; set; } = null;

    public AuditableEntity() { }

    public AuditableEntity(AuditableEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        UpdatedAt = entity.UpdatedAt;
        UpdatedById = entity.UpdatedById;
        UpdatedBy = entity.UpdatedBy;
    }
}
