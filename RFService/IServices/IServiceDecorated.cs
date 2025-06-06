using RFService.Libs;

namespace RFService.IServices
{
    public interface IServiceDecorated
    {
        IPropertiesDecorators PropertiesDecorators { get; }

        public virtual async Task<DataDictionary?> DecorateItemAsync<T>(
            T data,
            string name,
            DataDictionary? property,
            string eventType = ""
        )
        {
            if (data == null)
                return property;

            var decorators = PropertiesDecorators.GetDecorators(name);
            if (decorators == null)
                return property;

            DataDictionary newProperty = property ?? [];
            foreach (var decorator in decorators)
                await decorator(data, newProperty, eventType);

            if (newProperty != property && newProperty.Count != 0)
                property = newProperty;

            return property;
        }

        public virtual async Task<IEnumerable<T>> DecorateListAsync<T>(
            IEnumerable<T> list,
            string name,
            Action<T, DataDictionary> assign,
            string eventType = ""
        )
        {
            var decorators = PropertiesDecorators.GetDecorators(name);
            if (decorators == null)
                return list;

            list = [.. list];
            foreach (var decorator in decorators)
            {
                foreach (var row in list)
                {
                    if (row != null)
                    {
                        DataDictionary newProperty = [];
                        await decorator(row, newProperty, eventType);
                        if (newProperty.Count != 0)
                            assign(row, newProperty);
                    }
                }
            }

            return list;
        }
    }
}
