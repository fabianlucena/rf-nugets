global using PropertyDecorator = System.Func<object, dynamic, System.Threading.Tasks.Task>;

namespace RFService.IService
{
    public interface IPropertiesDecorators
    {
        void AddDecorator(string name, PropertyDecorator decorator);

        IEnumerable<PropertyDecorator>? GetDecorators(string name);
    }
}
