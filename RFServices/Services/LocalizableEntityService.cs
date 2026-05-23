using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFL10n;

namespace RFServices.Services
{
    public class LocalizableEntityService<T>(
        ILocalizableEntityRepository<T> repository,
        IL10n l10n
    )
        : TitledEntityService<T>(repository),
        ILocalizableEntityService<T>
        where T : LocalizableEntity, new()
    {
        public override async Task<T> ValidateForCreateAsync(T entity)
        {
            entity = await base.ValidateForCreateAsync(entity);

            return entity;
        }

        public async Task<T> Translate(T entity)
        {
            if (entity.Title is not null)
            {
                entity = (T)entity.Clone();
                entity.Title = await l10n._(entity.Title);
            }

            return entity;
        }
    }
}
