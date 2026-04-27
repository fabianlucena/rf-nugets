namespace RFBaseEntities.Entities
{
    public class ImmutableEntity : CreatableEntity
    {
        public DateTime? DeletedAt { get; set; } = null;

        public long? DeletedById { get; set; } = 0;

        public User? DeletedBy { get; set; } = null;
    }
}
