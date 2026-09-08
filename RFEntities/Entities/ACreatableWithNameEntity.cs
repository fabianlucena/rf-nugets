namespace RFEntities.Entities;

public abstract class ACreatableWithNameEntity : CreatableWithNameEntity
{
    public bool IsActive { get; set; } = true;

    public ACreatableWithNameEntity() { }

    public ACreatableWithNameEntity(ACreatableWithNameEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        IsActive = entity.IsActive;
    }
}
