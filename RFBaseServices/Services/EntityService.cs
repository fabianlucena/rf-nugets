using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

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

        public async Task<IEnumerable<long>> GetListIdAsync(BaseQueryOptions? options = null)
        {
            return await repository.GetListIdAsync(options);
        }

        public async Task<T> GetSingleByIdAsync(long id, BaseQueryOptions? options = null)
        {
            return await repository.GetSingleByIdAsync(id, options);
        }

        public async Task<T?> GetFirstOrDefaultByUuidAsync(Guid uuid)
        {
            return await repository.GetFirstOrDefaultByUuidAsync(uuid);
        }

        public async Task UpdateByIdAsync(long id, IDataDictionary data)
        {
            data = await ValidateForUpdate(data);
            bool success = await repository.UpdateByIdAsync(id, data);
            if (!success)
            {
                throw new InvalidOperationException($"Failed to update entity with ID {id}.");
            }
        }
    }
}
