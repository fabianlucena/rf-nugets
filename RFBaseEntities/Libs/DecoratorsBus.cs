using RFBaseEntities.Exceptions;
using RFBaseEntities.ILibs;

namespace RFBaseEntities.Libs
{
    public class DecoratorsBus
        : IDecoratorsBus
    {
        static public readonly DecoratorsBus Singleton = new();

        static private readonly Dictionary<string, List<Decorator>> decorators = [];

        public bool Add(string name, Decorator decorator)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new NullDecoratorNameException();

            name = name.Trim().ToLower();
            if (!decorators.TryGetValue(name, out var entitiesDecorators))
            {
                entitiesDecorators = [];
                decorators[name] = entitiesDecorators;
            }

            if (!decorators.TryGetValue(name, out var list))
                decorators[name] = list = [];

            list.Add(decorator);

            return true;
        }

        public IEnumerable<Decorator>? GetDecorators(string name)
        {
            if (!decorators.TryGetValue(name, out var list))
                return null;

            return list;
        }

        public async Task<object?> DecorateAsync(
            string name,
            object entity,
            IServiceProvider? serviceProvider = null,
            object? data = null
        )
        {
            object decorated = entity;

            name = name.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(name))
                return decorated;

            if (entity == null)
                return decorated;

            var decorators = GetDecorators(name);
            if (decorators == null)
                return decorated;

            foreach (var decorator in decorators)
                decorated = await decorator(decorated, name, serviceProvider, data);

            return decorated;
        }
    }
}

