using RFService.Repo;
using RFService.IRepo;
using RFService.Entities;

namespace RFService.Services
{
    public abstract class Service<TRepo, TEntity>(TRepo _repo)
        where TRepo : IRepo<TEntity>
        where TEntity : Entity
    {
        public TRepo repo = _repo;

        public virtual async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            return await Task.Run(() => data);
        }

        public virtual async Task<TEntity> CreateAsync(TEntity data)
        {
            data = await ValidateForCreationAsync(data);
            return await repo.InsertAsync(data);
        }

        public virtual GetOptions SanitizeGetOptions(GetOptions options)
        {
            return options;
        }

        public virtual Task<IEnumerable<TEntity>> GetListAsync(GetOptions options)
            => repo.GetListAsync(SanitizeGetOptions(options));

        public virtual Task<IEnumerable<TEntity>> GetListAsync<TIncluded1>(GetOptions options)
            => repo.GetListAsync<TIncluded1>(SanitizeGetOptions(options));

        public virtual Task<TEntity> GetSingleAsync(GetOptions options)
        {
            return repo.GetSingleAsync(SanitizeGetOptions(options));
        }

        public virtual Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options)
        {
            return repo.GetSingleOrDefaultAsync(SanitizeGetOptions(options));
        }

        public virtual Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options)
        {
            return repo.GetFirstOrDefaultAsync(SanitizeGetOptions(options));
        }

        public virtual GetOptions SanitizeForAutoGet(GetOptions options)
        {
            return options;
        }

        public virtual Task<TEntity?> AutoGetFirstOrDefaultAsync(GetOptions options)
        {
            return GetFirstOrDefaultAsync(SanitizeForAutoGet(options));
        }

        public virtual Task<TEntity?> AutoGetFirstOrDefaultAsync(TEntity data)
        {
            var filters = new Dictionary<string, object?>();
            var entityType = typeof(TEntity);
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

        public virtual async Task<TEntity> GetOrCreateAsync(TEntity data)
        {
            var res = await AutoGetFirstOrDefaultAsync(data);
            if (res != null)
                return res;

            return await CreateAsync(data);
        }

        public virtual Task CreateIfNotExistsAsync(TEntity data)
        {
            return GetOrCreateAsync(data);
        }

        public virtual async Task<IDictionary<string, object?>> ValidateForUpdateAsync(IDictionary<string, object?> data)
        {
            return await Task.Run(() => data);
        }

        public virtual async Task<int> UpdateAsync(IDictionary<string, object?> data, GetOptions options)
        {
            data = await ValidateForUpdateAsync(data);
            return await repo.UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteAsync(GetOptions options)
        {
            return repo.DeleteAsync(options);
        }
    }
}