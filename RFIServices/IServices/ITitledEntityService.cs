using RFEntities.Entities;

namespace RFIServices.IServices
{
    public interface ITitledEntityService<T>
        : INominableEntityService<T>
        where T : TitledEntity, new()
    {
    }
}