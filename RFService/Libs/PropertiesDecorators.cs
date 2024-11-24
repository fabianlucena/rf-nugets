using RFService.IServices;

namespace RFService.Libs
{
    public class PropertiesDecorators : IPropertiesDecorators
    {
        static readonly Dictionary<string, List<PropertyDecorator>> attributesDecorators = [];

        public void AddDecorator(string name, PropertyDecorator decorator)
        {
            if (!attributesDecorators.TryGetValue(name, out var list))
                attributesDecorators[name] = list = [];

            list.Add(decorator);
        }

        public IEnumerable<PropertyDecorator>? GetDecorators(string name)
        {
            if (!attributesDecorators.TryGetValue(name, out var list))
                return null;

            return list;
        }
    }
}

