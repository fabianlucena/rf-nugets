using RFService.Exceptions;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceUniqueTitle<Entity>
        : IService<Entity>
        where Entity : class
    {
        public async Task<Entity> GetSingleForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Title"] = title;

            return await GetSingleAsync(options);
        }

        public async Task<Entity?> GetSingleOrDefaultForTitleAsync(string title, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Title"] = title;

            return await GetSingleOrDefaultAsync(options);
        }
    }
}
