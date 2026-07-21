namespace RFEntities.Entities
{
    public abstract class CreatableJoin : Join
    {
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
        public long CreatedById { get; set; }
        public User? CreatedBy { get; set; }

        public CreatableJoin() { }

        public CreatableJoin(CreatableJoin? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            CreatedAt = entity.CreatedAt;
            CreatedById = entity.CreatedById;
            CreatedBy = entity.CreatedBy;
        }
    }
}
