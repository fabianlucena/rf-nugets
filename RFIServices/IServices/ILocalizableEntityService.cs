using RFEntities.Entities;

namespace RFIServices.IServices;

public interface ILocalizableEntityService<T>
    : ITitledEntityService<T>
    where T : LocalizableEntity, new()
{
    public async Task<IEnumerable<T>> Translate(IEnumerable<T> entities)
        => await Task.WhenAll(entities.Select(entity => Translate(entity)));

    Task<T> Translate(T entity);
}