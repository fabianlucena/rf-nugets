namespace RFEntities.Entities;

public abstract class NominableOwnedEntity : OwnedEntity
{
    public string Name { get; set; } = string.Empty;

    public NominableOwnedEntity() { }

    public NominableOwnedEntity(NominableOwnedEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Name = entity.Name;
    }
}
