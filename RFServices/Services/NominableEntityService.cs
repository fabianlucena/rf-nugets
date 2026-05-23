using RFEntities.Entities;
using RFIServices.QueryOptions;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFServices.Exceptions;

namespace RFServices.Services
{
    public class NominableEntityService<T>(INominableEntityRepository<T> repository)
        : CommonEntityService<T>(repository),
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

        public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, NominableEntityQueryOptions? options = null)
        {
            options = (NominableEntityQueryOptions?)options?.Clone() ?? new NominableEntityQueryOptionsClonable();
            options.Name = name;
            return GetSingleIdOrDefaultAsync(options);
        }

        public async Task<long> GetSingleIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
            => await GetSingleIdOrDefaultByNameAsync(name, options)
                ?? throw new NoEntityFoundForNameException(name);

        public async Task<long> GetSingleIdByNameOrCreateAsync(string name, NominableEntityQueryOptions? options = null, Func<Task<T>, T>? completeCreateData = null)
        {
            var id = await GetSingleIdOrDefaultByNameAsync(name, options);
            if (id != null)
                return id.Value;

            var entity = new T { Name = name };
            if (completeCreateData != null)
                entity = completeCreateData(Task.FromResult(entity));

            var createdEntity = await CreateAsync(entity);
            return createdEntity.Id;
        }
    }
}
