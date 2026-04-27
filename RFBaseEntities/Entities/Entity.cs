namespace RFBaseEntities.Entities
{
    public class Entity : Base
    {
        public long Id { get; set; } = 0;
        public Guid Uuid { get; set; } = Guid.Empty;
    }
}
