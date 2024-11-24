namespace RFService.IServices
{
    public interface IServiceDecorated
    {
        IPropertiesDecorators PropertiesDecorators { get; }

        public virtual async Task<IDictionary<string, object>?> DecorateAsync<T>(
            T data,
            IDictionary<string, object>? property,
            string name,
            string eventType = ""
        )
        {
            if (data == null)
                return property;

            var decorators = PropertiesDecorators.GetDecorators(name);
            if (decorators == null)
                return property;

            IDictionary<string, object> newProperty = property ?? new Dictionary<string, object>();
            foreach (var decorator in decorators)
                await decorator(data, newProperty, eventType);

            if (newProperty != property && newProperty.Count != 0)
                property = newProperty;

            return property;
        }

        public virtual async Task<IEnumerable<T>> DecorateAsync<T>(
            IEnumerable<T> list,
            string name,
            Action<T, IDictionary<string, object>> assign,
            string eventType = ""
        )
        {
            var decorators = PropertiesDecorators.GetDecorators(name);
            if (decorators == null)
                return list;

            list = list.ToList();
            foreach (var decorator in decorators)
            {
                foreach (var row in list)
                {
                    if (row != null)
                    {
                        IDictionary<string, object> newProperty = new Dictionary<string, object>();
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
