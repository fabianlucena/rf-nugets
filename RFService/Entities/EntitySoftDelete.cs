namespace RFService.Entities
{
    public abstract class EntitySoftDelete : Entity
    {
        public DateTime? DeletedAt { get; set; }
    }
}