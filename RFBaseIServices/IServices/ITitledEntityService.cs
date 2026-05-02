using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;

namespace RFBaseIServices.IServices
{
    public interface ITitledEntityService<T>
        : INominableEntityService<T>
        where T : TitledEntity, new()
    {
    }
}