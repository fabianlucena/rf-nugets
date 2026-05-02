namespace RFBaseEntities.Entities
{
    public class NominableEntity : CommonEntity
    {
        public string Name { get; set; } = string.Empty;

        public NominableEntity() { }

        public NominableEntity(NominableEntity entity)
            : base(entity)
        {
            Name = entity.Name;
        }
    }
}
