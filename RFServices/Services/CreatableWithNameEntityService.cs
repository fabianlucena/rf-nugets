using RFEntities.Entities;
using RFIServices.QueryOptions;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services;

public class CreatableWithNameEntityService<T>(
    ICreatableWithNameEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<T>(repository, serviceProvider),
    ICreatableWithNameEntityService<T>
    where T : CreatableWithNameEntity, new()
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

    public Task<T?> GetSingleOrDefaultByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null)
    {
        options = (CreatableWithNameEntityQueryOptions?)options?.Clone() ?? new CreatableWithNameEntityQueryOptions();
        options.Name = name;
        return GetSingleOrDefaultAsync(options);
    }

    public async Task<T> GetSingleOrCreateByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
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

    public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null)
    {
        options = (CreatableWithNameEntityQueryOptions?)options?.Clone() ?? new CreatableWithNameEntityQueryOptions();
        options.Name = name;
        return GetSingleIdOrDefaultAsync(options);
    }

    public async Task<long> GetSingleIdByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null)
        => await GetSingleIdOrDefaultByNameAsync(name, options)
            ?? throw new NoEntityFoundForNameException(name);

    public async Task<long> GetSingleIdOrCreateByNameAsync(string name, CreatableWithNameEntityQueryOptions? options = null, Func<T, Task<T>>? createFactory = null)
    {
        var entity = await GetSingleOrCreateByNameAsync(name, options, createFactory);
        return entity.Id;
    }

    public async Task<IEnumerable<string>> GetNamesAsync(CreatableWithNameEntityQueryOptions options)
        => await repository.GetNamesAsync(options);

    public async Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, CreatableWithNameEntityQueryOptions? options = null)
    {
        options = new CreatableWithNameEntityQueryOptions(options)
        {
            Ids = [.. ids]
        };

        return await GetNamesAsync(options);
    }

    public async Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, CreatableWithNameEntityQueryOptions? options = null)
    {
        options = new CreatableWithNameEntityQueryOptions(options)
        {
            Names = [.. names]
        };

        return await GetListIdAsync(options);
    }
}
