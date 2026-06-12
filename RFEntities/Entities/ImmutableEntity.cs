namespace RFEntities.Entities
{
    public abstract class ImmutableEntity : CreatableEntity
    {
        public DateTime? DeletedAt { get; set; } = null;

        public long? DeletedById { get; set; } = null;

        public User? DeletedBy { get; set; } = null;

        public ImmutableEntity() { }

        public ImmutableEntity(ImmutableEntity? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            DeletedAt = entity.DeletedAt;
            DeletedById = entity.DeletedById;
            DeletedBy = entity.DeletedBy;
        }
    }
}
