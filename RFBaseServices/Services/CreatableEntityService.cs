using RFBaseEntities.Entities;
using RFBaseIRepositories.IRepositories;
using RFBaseIServices.IServices;

namespace RFBaseServices.Services
{
    public class CreatableEntityService<T>(ICreatableEntityRepository<T> repository)
        : EntityService<T>(repository),
        ICreatableEntityService<T>
        where T : CreatableEntity, new()
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
}
