using RFBaseEntities.Entities;

namespace RFBaseIServices.IServices
{
    public interface ILocalizableEntityService<T>
        : ITitledEntityService<T>
        where T : LocalizableEntity, new()
    {
        public async Task<IEnumerable<T>> Translate(IEnumerable<T> entities)
        {
            return await Task.WhenAll(entities.Select(entity => Translate(entity)));
        }

        public async Task<T> Translate(T entity)
        {
            if (entity.Title is not null)
            {
                entity = (T)entity.Clone();
                entity.Title = $"*** {entity.Title} ***";
            }

            return entity;
        }
    }
}