namespace RFService.Entities
{
    public abstract class EntityCreatedAt
        : Entity
    {
        public DateTime CreatedAt { get; set; } = default;
    }
}