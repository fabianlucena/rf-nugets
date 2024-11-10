using RFService.Exceptions;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceIdName<Entity>
        : IServiceName<Entity>,
            IServiceId<Entity>
        where Entity : Entities.Entity
    {
        public async Task<Int64> GetIdForNameAsync(string name, GetOptions? options = null, Func<string, Entity>? creator = null)
        {
            var item = await GetSingleOrDefaultForNameAsync(name, options);
            if (item == null)
            {
                if (creator != null)
                {
                    item = creator(name);
                }

                if (item == null)
                {
                    throw new NamedItemNotFoundException(name);
                }

                await CreateAsync(item);
            }

            return GetId(item);
        }
    }
}
