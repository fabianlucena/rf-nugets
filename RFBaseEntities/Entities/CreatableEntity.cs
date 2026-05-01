namespace RFBaseEntities.Entities
{
    public class CreatableEntity : Entity
    {
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public long CreatedById { get; set; } = 0;
        public User? CreatedBy { get; set; } = null;

        public CreatableEntity() { }

        public CreatableEntity(CreatableEntity entity)
            : base(entity)
        {
            CreatedAt = entity.CreatedAt;
            CreatedById = entity.CreatedById;
            CreatedBy = entity.CreatedBy;
        }
    }
}
