using RFEntities.Entities;

namespace RFIServices.IServices;

public interface ILocalizableEntityService<T>
    : ITitledEntityService<T>
    where T : LocalizableEntity, new()
{
    string? GetTranslationContext(T entity);

    public async Task<IEnumerable<T>> Translate(IEnumerable<T> entities, string? context = null)
        => await Task.WhenAll(entities.Select(entity => Translate(entity, context)));

    Task<T> Translate(T entity, string? context = null);
}