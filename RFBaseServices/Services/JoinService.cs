using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class JoinService<T>(IJoinRepository<T> repository)
        : BaseService<T>(repository),
        IJoinService<T>
        where T : Join, new()
    {
        public override async Task<T> ValidateForCreateAsync(T entity)
        {
            entity = await base.ValidateForCreateAsync(entity);

            return entity;
        }
    }
}
