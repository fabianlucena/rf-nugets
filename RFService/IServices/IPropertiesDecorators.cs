global using PropertyDecorator = System.Func<object, RFService.Libs.DataDictionary, string, System.Threading.Tasks.Task>;

namespace RFService.IServices
{
    public interface IPropertiesDecorators
    {
        void AddDecorator(string name, PropertyDecorator decorator);

        IEnumerable<PropertyDecorator>? GetDecorators(string name);
    }
}
