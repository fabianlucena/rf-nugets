namespace RFBaseEntities.Entities
{
    public abstract class CommonJoin : CreatableJoin
    {
        public DateTime? DeletedAt { get; set; }
        public long? DeletedById { get; set; }
        public User? DeletedBy { get; set; }

        public CommonJoin() { }

        public CommonJoin(CommonJoin? entity = null)
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