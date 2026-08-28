namespace RFEntities.Entities;

public abstract class TitledEntity : NominableEntity
{
    public string Title { get; set; } = string.Empty;

    public TitledEntity() { }

    public TitledEntity(TitledEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        Title = entity.Title;
    }
}
