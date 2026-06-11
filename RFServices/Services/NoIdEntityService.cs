using RFBase.ILibs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;

namespace RFServices.Services;

public class NoIdEntityService<T>(INoIdEntityRepository<T> repository)
    : BaseService<T>(repository),
    INoIdEntityService<T>
    where T : NoIdEntity, new()
{
    public override async Task<T> ValidateForCreateAsync(T entity)
    {
        entity = await base.ValidateForCreateAsync(entity);

        if (entity.CreatedById == 0)
        {
            throw new ArgumentException("CreatedById must be set for new entries.");
        }

        entity.CreatedAt = DateTime.UtcNow;

        if (entity.UpdatedById <= 0)
        {
            throw new ArgumentException("UpdatedById must be set for auditable entries.");
        }

        entity.UpdatedAt = DateTime.UtcNow;

        return entity;
    }

    public override async Task<IDataDictionary> ValidateForUpdate(IDataDictionary data)
    {
        data = await base.ValidateForUpdate(data);

        if (!data.TryGetValue("UpdatedById", out object? value) || value is null || (long)value <= 0)
        {
            throw new InvalidOperationException("UpdatedById must be set for auditable entities.");
        }

        data["UpdatedAt"] = DateTime.UtcNow;

        return data;
    }
}
