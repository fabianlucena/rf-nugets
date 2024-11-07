using RFService.IService;

namespace RFService.Services
{
    public abstract class ServiceDecorated(IPropertiesDecorators attributesDecorators)
    {
        public virtual async Task<IDictionary<string, object>?> DecorateAsync(object data, IDictionary<string, object>? property, string name)
        {
            var decorators = attributesDecorators.GetDecorators(name);
            if (decorators == null)
                return property;

            IDictionary<string, object> newProperty = property ?? new Dictionary<string, object>();
            foreach (var decorator in decorators)
                await decorator(data, newProperty);

            if (newProperty != property && newProperty.Count != 0)
                property = newProperty;

            return property;
        }
    }
}