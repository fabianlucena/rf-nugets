global using Decorator = System.Func<object, string, System.IServiceProvider?, object?, System.Threading.Tasks.Task<object>>;

namespace RFBaseEntities.ILibs
{
    public interface IDecoratorsBus
    {
        bool Add(string name, Decorator decorator);

        IEnumerable<Decorator>? GetDecorators(string name);

        Task<object?> DecorateAsync(string name, object entity, IServiceProvider? serviceProvider = null, object? data = null);
    }
}
