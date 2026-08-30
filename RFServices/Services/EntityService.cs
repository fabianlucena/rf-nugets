using RFBase.ILibs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFServices.Exceptions;

namespace RFServices.Services;

public class EntityService<T>(
    IEntityRepository<T> repository,
    IServiceProvider serviceProvider
)
    : BaseService<T>(repository, serviceProvider),
    IEntityService<T>
    where T : Entity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.Id != 0)
        {
            throw new ArgumentException("Entity ID must be zero for new entries.");
        }

        if (entity.Uuid == Guid.Empty)
        {
            do
            {
                entity.Uuid = Guid.NewGuid();
            } while (await GetFirstOrDefaultByUuidAsync(entity.Uuid) != null);
        }
        else
        {
            throw new ArgumentException("An entity for the provided UUID already exists.");
        }

        return entity;
    }

    public async Task<T> GetSingleByIdAsync(long id, EntityQueryOptions? options = null)
    {
        options = (EntityQueryOptions?)options?.Clone() ?? new EntityQueryOptionsClonable();
        options.Id = id;
        return await GetSingleAsync(options);
    }

    public async Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
    {
        options = (EntityQueryOptions?)options?.Clone() ?? new EntityQueryOptionsClonable();
        options.Uuid = uuid;
        return await GetFirstOrDefaultAsync(options);
    }

    public async Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions options)
        => await repository.GetIdsAsync(options);

    public async Task<long?> GetSingleIdOrDefaultAsync(EntityQueryOptions options)
    {
        options = (EntityQueryOptions)options.Clone();
        options.Take = 2;
        var ids = await GetIdsAsync(options);

        if (!ids.Any())
            return null;


        if (ids.Count() > 1)
            throw new MultipleEntitiesFoundMatchingTheSpecifiedCriteriaException();

        return ids.First();
    }

    public async Task<long> GetSingleIdAsync(EntityQueryOptions options)
        => await GetSingleIdOrDefaultAsync(options)
            ?? throw new NoEntityFoundMatchingTheSpecifiedCriteriaException();

    public async Task<long> GetSingleIdByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
    {
        var id = await repository.GetSingleIdOrDefaultByUuidAsync(uuid, options);
        if (id == default)
            throw new NoEntityFoundForUuidException(uuid);

        return id;
    }

    public async Task<int> UpdateByIdAsync(long id, IDataDictionary data)
    {
        data = await ValidateForUpdate(data);
        int success = await repository.UpdateByIdAsync(id, data);
        if (success == 0)
            throw new InvalidOperationException($"Failed to update entity with ID {id}.");

        return success;
    }

    public async Task<int> UpdateByUuidAsync(Guid uuid, IDataDictionary data)
    {
        var id = await GetSingleIdByUuidAsync(uuid);
        data = await ValidateForUpdate(data);
        int success = await repository.UpdateByIdAsync(id, data);
        if (success == 0)
            throw new InvalidOperationException($"Failed to update entity with UUID {uuid}.");

        return success;
    }

    public virtual async Task<int> DeleteByIdAsync(long id)
    {
        int success = await repository.DeleteByIdAsync(id);
        if (success == 0)
            throw new InvalidOperationException($"Failed to delete entity with ID {id}.");

        return success;
    }

    public virtual async Task<int> DeleteByUuidAsync(Guid uuid)
    {
        int success = await repository.DeleteByUuidAsync(uuid);
        if (success == 0)
            throw new InvalidOperationException($"Failed to delete entity with UUID {uuid}.");

        return success;
    }
}
