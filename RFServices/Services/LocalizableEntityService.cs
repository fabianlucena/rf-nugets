using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFL10n;

namespace RFServices.Services;

public class LocalizableEntityService<T>(
    ILocalizableEntityRepository<T> repository,
    IL10n l10n,
    IServiceProvider serviceProvider
)
    : TitledEntityService<T>(repository, serviceProvider),
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

    public async Task<T> GetSingleByNameOrCreateAsync(string name, LocalizableEntityQueryOptions? options = null, Func<T, Task<T>>? completeCreateData = null)
    {
        var entity = await GetSingleOrDefaultByNameAsync(name, options);
        if (entity != null)
            return entity;

        entity = new T { Name = name };
        if (completeCreateData != null)
            entity = await completeCreateData(entity);

        entity = await CreateAsync(entity);

        return entity;
    }
}
