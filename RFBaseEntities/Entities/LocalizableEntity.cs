namespace RFBaseEntities.Entities
{
    public class LocalizableEntity : TitledEntity
    {
        public bool IsTranslatable { get; set; }

        public LocalizableEntity() { }

        public LocalizableEntity(LocalizableEntity entity)
            : base(entity)
        {
            IsTranslatable = entity.IsTranslatable;
        }

        public virtual LocalizableEntity Clone()
        {
            return new LocalizableEntity(this);
        }
    }
}
