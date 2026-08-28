namespace RFEntities.Entities;

public abstract class NoIdEntity : Base
{
    public DateTime? DeletedAt { get; set; } = null;
    public long? DeletedById { get; set; } = null;
    public User? DeletedBy { get; set; } = null;

    public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
    public long UpdatedById { get; set; } = 0;
    public User? UpdatedBy { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    public long CreatedById { get; set; } = 0;
    public User? CreatedBy { get; set; } = null;

    public NoIdEntity() { }

    public NoIdEntity(NoIdEntity? entity = null)
        : base(entity)
    {
        if (entity == null)
            return;

        DeletedAt = entity.DeletedAt;
        DeletedById = entity.DeletedById;
        DeletedBy = entity.DeletedBy;

        UpdatedAt = entity.UpdatedAt;
        UpdatedById = entity.UpdatedById;
        UpdatedBy = entity.UpdatedBy;
        
        CreatedAt = entity.CreatedAt;
        CreatedById = entity.CreatedById;
        CreatedBy = entity.CreatedBy;
    }
}
