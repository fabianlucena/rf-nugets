namespace RFService.Entities
{
    public abstract class EntitySoftDeleteCreatedAt
        : EntitySoftDelete
    {
        public DateTime CreatedAt { get; set; } = default;
    }
}