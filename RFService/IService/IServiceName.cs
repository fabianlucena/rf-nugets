using RFService.Repo;

namespace RFService.IService
{
    public interface IServiceName<Entity>
        where Entity : class
    {
        Task<Entity> GetSingleForNameAsync(string name, GetOptions? options = null);

        Task<Entity?> GetSingleOrDefaultForNameAsync(string name, GetOptions? options = null);
    }
}
