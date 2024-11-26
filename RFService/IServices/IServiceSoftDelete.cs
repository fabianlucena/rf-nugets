using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDelete<Entity>
        : IService<Entity>
        where Entity : class
    {
        Task<int> RestoreAsync(GetOptions options)
            => UpdateAsync(new Dictionary<string, object?>{{ "DeletedAt", null }}, options);
    }
}
