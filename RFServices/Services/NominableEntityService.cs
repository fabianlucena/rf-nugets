using RFEntities.Entities;
using RFIServices.QueryOptions;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services;

public class NominableEntityService<T>(
    INominableEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CommonEntityService<T>(repository, serviceProvider),
    INominableEntityService<T>
    where T : NominableEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            throw new NameIsMandatoryForNewEntriesException();
        }

        return entity;
    }

    public Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
    {
        options = (NominableEntityQueryOptions?)options?.Clone() ?? new NominableEntityQueryOptionsClonable();
        options.Name = name;
        return GetSingleOrDefaultAsync(options);
    }

    public async Task<T> GetOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
    {
        var entity = await GetSingleOrDefaultByNameAsync(name, options);
        if (entity != null)
            return entity;

        entity = new T { Name = name };
        if (createFactory != null)
            entity = await createFactory(entity);

        var createdEntity = await CreateAsync(entity);
        return createdEntity;
    }

    public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
    {
        options = (NominableEntityQueryOptions?)options?.Clone() ?? new NominableEntityQueryOptionsClonable();
        options.Name = name;
        return GetSingleIdOrDefaultAsync(options);
    }

    public async Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
        => await GetSingleIdOrDefaultByNameAsync(name, options)
            ?? throw new NoEntityFoundForNameException(name);

    public async Task<long> GetIdOrCreateByNameAsync(string name, NominableEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
    {
        var entity = await GetOrCreateByNameAsync(name, options, createFactory);
        return entity.Id;
    }

    public async Task<IEnumerable<string>> GetNamesAsync(NominableEntityQueryOptions options)
        => await repository.GetNamesAsync(options);

    public async Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableEntityQueryOptions? options = null)
    {
        options = new NominableEntityQueryOptions(options)
        {
            Ids = [.. ids]
        };

        return await GetNamesAsync(options);
    }
}
