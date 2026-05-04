using RFBaseEntities.Entities;
using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;
using RFBaseServices.Exceptions;

namespace RFBaseServices.Services
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

        public Task<long?> GetSingleOrDefaultIdByNameAsync(string name, NominableEntityQueryOptions? options = null)
        {
            options = (NominableEntityQueryOptions?)options?.Clone() ?? new NominableEntityQueryOptionsClonable();
            options.Name = name;
            return GetSingleOrDefaultIdAsync(options);
        }
    }
}
