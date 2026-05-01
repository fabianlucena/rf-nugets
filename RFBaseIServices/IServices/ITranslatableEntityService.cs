using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface ITranslatableEntityService<T>
        : ICommonEntityService<T>
        where T : TranslatableEntity, new()
    {
    }
}