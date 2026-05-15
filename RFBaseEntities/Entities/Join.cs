namespace RFBaseEntities.Entities
{
    public abstract class Join : Base
    {
        public Join() { }

        public Join(Join? entity = null)
            : base(entity)
        {
            if (entity == null)
                return;
        }
    }
}
