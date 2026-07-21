namespace RFEntities.Entities
{
    public abstract class LocalizableEntity : TitledEntity
    {
        public bool IsTranslatable { get; set; }

        public LocalizableEntity() { }

        public LocalizableEntity(LocalizableEntity? entity)
            : base(entity)
        {
            if (entity == null)
                return;

            IsTranslatable = entity.IsTranslatable;
        }
    }
}
