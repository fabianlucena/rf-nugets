using RFService.Repo;

namespace RFService.IService
{
    public interface IServiceIdName<Entity>
        where Entity : class
    {
        Task<Int64> GetIdForNameAsync(string name, GetOptions? options = null, Func<string, Entity>? creator = null);
    }
}
