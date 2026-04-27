namespace RFBaseEntities.Entities
{
    public class AuditableEntity : CreatableEntity
    {
        public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
        public long UpdatedById { get; set; } = 0;
        public User? UpdatedBy { get; set; } = null;
    }
}
