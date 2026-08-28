namespace RFEntities.Entities;

public abstract class NominableEntity : CommonEntity
{
    public string Name { get; set; } = string.Empty;

    public NominableEntity() { }

    public NominableEntity(NominableEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Name = entity.Name;
    }
}
