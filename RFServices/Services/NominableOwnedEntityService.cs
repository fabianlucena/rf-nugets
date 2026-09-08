using RFEntities.Entities;
using RFIServices.QueryOptions;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services;

public class NominableOwnedEntityService<T>(
    INominableOwnedEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : OwnedEntityService<T>(repository, serviceProvider),
    INominableOwnedEntityService<T>
    where T : NominableOwnedEntity, new()
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

    public Task<T?> GetSingleOrDefaultByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null)
    {
        options = (NominableOwnedEntityQueryOptions?)options?.Clone() ?? new NominableOwnedEntityQueryOptions();
        options.Name = name;
        return GetSingleOrDefaultAsync(options);
    }

    public async Task<T> GetSingleOrCreateByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
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

    public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null)
    {
        options = (NominableOwnedEntityQueryOptions?)options?.Clone() ?? new NominableOwnedEntityQueryOptions();
        options.Name = name;
        return GetSingleIdOrDefaultAsync(options);
    }

    public async Task<long> GetSingleIdByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null)
        => await GetSingleIdOrDefaultByNameAsync(name, options)
            ?? throw new NoEntityFoundForNameException(name);

    public async Task<long> GetSingleIdOrCreateByNameAsync(string name, NominableOwnedEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
    {
        var entity = await GetSingleOrCreateByNameAsync(name, options, createFactory);
        return entity.Id;
    }

    public async Task<IEnumerable<string>> GetNamesAsync(NominableOwnedEntityQueryOptions options)
        => await repository.GetNamesAsync(options);

    public async Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, NominableOwnedEntityQueryOptions? options = null)
    {
        options = new NominableOwnedEntityQueryOptions(options)
        {
            Ids = [.. ids]
        };

        return await GetNamesAsync(options);
    }

    public async Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, NominableOwnedEntityQueryOptions? options = null)
    {
        options = new NominableOwnedEntityQueryOptions(options)
        {
            Names = [.. names]
        };

        return await GetListIdAsync(options);
    }
}
