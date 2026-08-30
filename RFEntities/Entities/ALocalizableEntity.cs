namespace RFEntities.Entities;

public abstract class ALocalizableEntity : LocalizableEntity
{
    public bool IsActive { get; set; } = true;

    public ALocalizableEntity() { }

    public ALocalizableEntity(ALocalizableEntity? entity)
        : base(entity)
    {
        if (entity == null)
            return;

        IsActive = entity.IsActive;
    }
}
