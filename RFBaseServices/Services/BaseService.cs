using RFBaseEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class BaseService<T>(IBaseRepository<T> repository)
        : IBaseService<T>
        where T : Base
    {
        public virtual async Task<T> ValidateForCreateAsync(T entity)
        {
            return entity;
        }

        public virtual async Task<IDataDictionary> ValidateForUpdate(IDataDictionary data)
        {
            return data;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            entity = await ValidateForCreateAsync(entity);
            return await repository.CreateAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(BaseQueryOptions? options = null)
        {
            return await repository.GetListAsync(options);
        }
    }
}
