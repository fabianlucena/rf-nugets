namespace RFEntities.Entities
{
    public abstract class Entity : Base
    {
        public long Id { get; set; } = 0;
        public Guid Uuid { get; set; } = Guid.Empty;

        public Entity() { }

        public Entity(Entity? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            Id = entity.Id;
            Uuid = entity.Uuid;
        }
    }
}
