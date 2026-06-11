using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

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
