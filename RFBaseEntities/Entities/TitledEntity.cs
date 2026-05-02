namespace RFBaseEntities.Entities
{
    public class TitledEntity : NominableEntity
    {
        public string Title { get; set; } = string.Empty;

        public TitledEntity() { }

        public TitledEntity(TitledEntity entity)
            : base(entity)
        {
            Title = entity.Title;
        }
    }
}
