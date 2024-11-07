using RFService.Entities;
using RFService.Repo;
using RFService.IRepo;

namespace RFService.Services
{
    public abstract class Service<Repo, Entity>(Repo _repo)
        where Repo : IRepo<Entity>
        where Entity : EntityTimestamps
    {
        public Repo repo = _repo;

        public virtual async Task<Entity> ValidateForCreationAsync(Entity data)
        {
            return await Task.Run(() => data);
        }

        public virtual async Task<Entity> CreateAsync(Entity data)
        {
            data = await ValidateForCreationAsync(data);
            return await repo.InsertAsync(data);
        }

        public virtual GetOptions SanitizeGetOptions(GetOptions options)
        {
            return options;
        }

        public virtual Task<IEnumerable<Entity>> GetListAsync(GetOptions options)
        {
            return repo.GetListAsync(SanitizeGetOptions(options));
        }

        public virtual Task<Entity> GetSingleAsync(GetOptions options)
        {
            return repo.GetSingleAsync(SanitizeGetOptions(options));
        }

        public virtual Task<Entity?> GetSingleOrDefaultAsync(GetOptions options)
        {
            return repo.GetSingleOrDefaultAsync(SanitizeGetOptions(options));
        }

        public virtual Task<Entity?> GetFirstOrDefaultAsync(GetOptions options)
        {
            return repo.GetFirstOrDefaultAsync(SanitizeGetOptions(options));
        }

        public virtual GetOptions SanitizeForAutoGet(GetOptions options)
        {
            return options;
        }

        public virtual Task<Entity?> AutoGetFirstOrDefaultAsync(GetOptions options)
        {
            return GetFirstOrDefaultAsync(SanitizeForAutoGet(options));
        }

        public virtual Task<Entity?> AutoGetFirstOrDefaultAsync(Entity data)
        {
            var filters = new Dictionary<string, object?>();
            var entityType = typeof(Entity);
            var properties = entityType.GetProperties();
            foreach (var pInfo in properties)
            {
                var pType = pInfo.PropertyType;
                if (pType.IsClass && pType.Name != "String")
                    continue;

                filters[pInfo.Name] = pInfo.GetValue(data);
            }

            return AutoGetFirstOrDefaultAsync(new GetOptions { Filters = filters });
        }

        public virtual async Task<Entity> GetOrCreateAsync(Entity data)
        {
            var res = await AutoGetFirstOrDefaultAsync(data);
            if (res != null)
                return res;

            return await CreateAsync(data);
        }

        public virtual Task CreateIfNotExistsAsync(Entity data)
        {
            return GetOrCreateAsync(data);
        }

        public virtual Task<int> UpdateAsync(object data, GetOptions options)
        {
            return repo.UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteAsync(GetOptions options)
        {
            throw new NotImplementedException();
        }
    }
}