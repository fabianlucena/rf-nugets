using RFBaseEntities.Entities;
using RFBaseIServices.IServices;

namespace RFBaseServices.Decorators
{
    public class TitledEntityServiceDecorator<T>(ITitledEntityService<T> titledEntityService)
        : NominableEntityServiceDecorator<T>(titledEntityService),
        ITitledEntityService<T>
        where T : TitledEntity, new()
    {
    }
}
