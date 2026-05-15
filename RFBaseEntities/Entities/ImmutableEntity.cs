namespace RFBaseEntities.Entities
{
    public abstract class ImmutableEntity : CreatableEntity
    {
        public DateTime? DeletedAt { get; set; } = null;

        public long? DeletedById { get; set; } = 0;

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
