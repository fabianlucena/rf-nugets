using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class CreatableJoinService<T>(ICreatableJoinRepository<T> repository)
    : JoinService<T>(repository),
    ICreatableJoinService<T>
    where T : CreatableJoin, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.CreatedById == 0)
        {
            throw new ArgumentException("CreatedById must be set for new entries.");
        }

        entity.CreatedAt = DateTime.UtcNow;

        return entity;
    }
}
