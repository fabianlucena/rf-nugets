using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class LocalizableEntityService<T>(ILocalizableEntityRepository<T> repository)
        : TitledEntityService<T>(repository),
        ILocalizableEntityService<T>
        where T : LocalizableEntity, new()
    {
        public override async Task<T> ValidateForCreateAsync(T entity)
        {
            entity = await base.ValidateForCreateAsync(entity);

            return entity;
        }
    }
}
