using RFEntities.Entities;
using RFIServices.IServices;

namespace RFServices.Decorators
{
    public class TitledEntityServiceDecorator<T>(ITitledEntityService<T> titledEntityService)
        : NominableEntityServiceDecorator<T>(titledEntityService),
        ITitledEntityService<T>
        where T : TitledEntity, new()
    {
    }
}
