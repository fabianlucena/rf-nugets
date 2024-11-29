using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceUniqueTitle<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        public async Task<TEntity> GetSingleForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Title"] = title;

            return await GetSingleAsync(options);
        }

        public async Task<TEntity?> GetSingleOrDefaultForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Title"] = title;

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
