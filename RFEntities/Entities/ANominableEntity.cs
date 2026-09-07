namespace RFEntities.Entities;

public abstract class ANominableEntity : NominableEntity
{
    public bool IsActive { get; set; } = true;

    public ANominableEntity() { }

    public ANominableEntity(ANominableEntity? entity)
        : base(entity)
    {
        if (entity == null)
            return;

        IsActive = entity.IsActive;
    }
}
