using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;
using RFBaseServices.Exceptions;
using System.Reflection.PortableExecutable;

namespace RFBaseServices.Services
{
    public class EntityService<T>(IEntityRepository<T> repository)
        : BaseService<T>(repository),
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
            options = options is null ? new EntityQueryOptions() : options.Clone() as EntityQueryOptions;
            options!.Id = id;
            return await GetSingleAsync(options);
        }

        public async Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid, EntityQueryOptions? options = null)
        {
            options = options is null ? new EntityQueryOptions() : options.Clone() as EntityQueryOptions;
            options!.Uuid = uuid;
            return await GetFirstOrDefaultAsync(options);
        }

        public async Task<IEnumerable<long>> GetIdsAsync(EntityQueryOptions options)
            => await repository.GetIdsAsync(options);

        public async Task<long?> GetSingleOrDefaultIdAsync(EntityQueryOptions options)
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
            => await GetSingleOrDefaultIdAsync(options)
                ?? throw new NoEntityFoundMatchingTheSpecifiedCriteriaException();

        public async Task UpdateByIdAsync(long id, IDataDictionary data)
        {
            data = await ValidateForUpdate(data);
            int success = await repository.UpdateByIdAsync(id, data);
            if (success == 0)
            {
                throw new InvalidOperationException($"Failed to update entity with ID {id}.");
            }
        }
    }
}
