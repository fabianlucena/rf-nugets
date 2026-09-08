namespace RFEntities.Entities;

public abstract class CreatableWithNameEntity : CreatableEntity
{
    public string Name { get; set; } = string.Empty;

    public CreatableWithNameEntity() { }

    public CreatableWithNameEntity(CreatableWithNameEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Name = entity.Name;
    }
}
