namespace RFEntities.Entities;

public abstract class ACommonEntity : CommonEntity
{
    public bool IsActive { get; set; } = true;

    public ACommonEntity() { }

    public ACommonEntity(ACommonEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        IsActive = entity.IsActive;
    }
}
