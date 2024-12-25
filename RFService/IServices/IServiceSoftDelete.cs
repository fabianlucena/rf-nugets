using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDelete<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        Task<int> RestoreAsync(GetOptions options)
            => UpdateAsync(new DataDictionary { { "DeletedAt" , null } }, options);
    }
}
