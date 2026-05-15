using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class TitledEntityService<T>(ITitledEntityRepository<T> repository)
        : NominableEntityService<T>(repository),
        ITitledEntityService<T>
        where T : TitledEntity, new()
    {
        public override async Task<T> ValidateForCreateAsync(T entity)
        {
            entity = await base.ValidateForCreateAsync(entity);

            return entity;
        }
    }
}
