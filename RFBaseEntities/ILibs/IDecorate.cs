using Microsoft.Extensions.DependencyInjection;

namespace RFBaseEntities.ILibs
{
    public interface IDecorate
    {
        IDecoratorsBus DecoratorsBus { get; }

        public virtual async Task<object?> DecorateAsync<T, U>(
            string name,
            T entity,
            IServiceProvider? serviceProvider = null,
            object? data = null
        ) where T : class
        {
            if (entity == null)
                return default;

            var decorators = DecoratorsBus.GetDecorators(name);
            if (decorators == null)
                return default;

            object decorated = entity;
            foreach (var decorator in decorators)
                decorated = await decorator(decorated, name, serviceProvider, data);

            return decorated;
        }
    }
}
