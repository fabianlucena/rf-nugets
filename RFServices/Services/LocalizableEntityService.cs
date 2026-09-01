using Microsoft.Extensions.DependencyInjection;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFL10n;

namespace RFServices.Services;

public class LocalizableEntityService<T>(
    ILocalizableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : TitledEntityService<T>(repository, serviceProvider),
    ILocalizableEntityService<T>
    where T : LocalizableEntity, new()
{
    public IL10n L10n { get => ServiceProvider.GetRequiredService<IL10n>(); }

    public virtual string? GetTranlationContext(T entity)
        => entity.TranslationContext;

    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        return entity;
    }

    public virtual async Task<T> Translate(T entity, string? context = null)
    {
        if (entity.Title is not null)
        {
            entity = (T)entity.Clone();
            entity.Title = await L10n._c(context ?? GetTranlationContext(entity) ?? "rfservices", entity.Title);
        }

        return entity;
    }

    public async Task<T> GetOrCreateByNameAsync(string name, LocalizableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
    {
        var entity = await GetSingleOrDefaultByNameAsync(name, options);
        if (entity != null)
            return entity;

        entity = new T { Name = name };
        if (createFactory != null)
            entity = await createFactory(entity);

        entity = await CreateAsync(entity);

        return entity;
    }
}
