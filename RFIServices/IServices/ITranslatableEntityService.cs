using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface ITranslatableEntityService<T>
        : ICommonEntityService<T>
        where T : TranslatableEntity, new()
    {
    }
}