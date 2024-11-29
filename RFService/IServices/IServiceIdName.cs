using RFService.Entities;
using RFService.Exceptions;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceIdName<TEntity>
        : IServiceName<TEntity>,
            IServiceId<TEntity>
        where TEntity : Entity
    {
        public async Task<Int64> GetIdForNameAsync(string name, GetOptions? options = null, Func<string, TEntity>? creator = null)
        {
            var item = await GetSingleOrDefaultForNameAsync(name, options);
            if (item == null)
            {
                if (creator != null)
                    item = creator(name);

                if (item == null)
                    throw new NamedItemNotFoundException(name);

                await CreateAsync(item);
            }

            return GetId(item);
        }

        public async Task<IEnumerable<TEntity>> GetListForNamesAsync(IEnumerable<string> names, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Name"] = names;

            return await GetListAsync(options);
        }

        public async Task<IEnumerable<Int64>> GetIdsForNamesAsync(IEnumerable<string> names, GetOptions? options = null)
            => (await GetListForNamesAsync(names, options)).Select(GetId);
    }
}
