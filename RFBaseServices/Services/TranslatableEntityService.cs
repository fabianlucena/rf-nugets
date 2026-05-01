using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class TranslatableEntityService<T>(ITranslatableEntityRepository<T> repository)
        : CommonEntityService<T>(repository),
        ITranslatableEntityService<T>
        where T : TranslatableEntity, new()
    {
        public override async Task<T> ValidateForCreateAsync(T entity)
        {
            entity = await base.ValidateForCreateAsync(entity);

            return entity;
        }
    }
}
