namespace RFEntities.Entities
{
    public abstract class TranslatableEntity : CommonEntity
    {
        public bool IsTranslatable { get; set; } = false;
        
        public TranslatableEntity() { }

        public TranslatableEntity(TranslatableEntity? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;

            IsTranslatable = entity.IsTranslatable;
        }
    }
}
