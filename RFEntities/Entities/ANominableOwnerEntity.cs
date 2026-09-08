namespace RFEntities.Entities;

public abstract class ANominableOwnedEntity : NominableOwnedEntity
{
    public bool IsActive { get; set; } = true;

    public ANominableOwnedEntity() { }

    public ANominableOwnedEntity(ANominableOwnedEntity? entity)
        : base(entity)
    {
        if (entity == null)
            return;

        IsActive = entity.IsActive;
    }
}
