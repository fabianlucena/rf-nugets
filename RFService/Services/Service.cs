using RFService.Repo;
using RFService.IRepo;
using RFService.Entities;
using System.Data;
using RFService.Libs;
using RFService.ILibs;
using RFOperators;

namespace RFService.Services
{
    public abstract class Service<TRepo, TEntity>(TRepo _repo)
        where TRepo : IRepo<TEntity>
        where TEntity : Entity
    {
        protected readonly TRepo repo = _repo;

        public (IDbConnection, Action) OpenConnection(RepoOptions? options = null)
            => repo.OpenConnection(options);

        public virtual async Task<TEntity> ValidateForCreationAsync(TEntity data)
            => await Task.Run(() => data);

        public virtual async Task<TEntity> CreateAsync(TEntity data, GetOptions? options = null)
        {
            data = await ValidateForCreationAsync(data);
            return await repo.InsertAsync(data);
        }

        public virtual GetOptions SanitizeGetOptions(GetOptions options)
            => options;

        public virtual Task<int> GetCountAsync(GetOptions options)
            => repo.GetCountAsync(SanitizeGetOptions(options));

        public virtual Task<IEnumerable<TEntity>> GetListAsync(GetOptions options)
            => repo.GetListAsync(SanitizeGetOptions(options));

        public virtual Task<TEntity> GetSingleAsync(GetOptions options)
            => repo.GetSingleAsync(SanitizeGetOptions(options));

        public virtual Task<TEntity?> GetSingleOrDefaultAsync(GetOptions options)
            => repo.GetSingleOrDefaultAsync(SanitizeGetOptions(options));

        public virtual async Task<TEntity?> GetSingleOrCreateAsync(GetOptions options, Func<TEntity> dataFactory)
            => await GetSingleOrDefaultAsync(options)
                ?? await CreateAsync(dataFactory());

        public virtual Task<TEntity?> GetFirstOrDefaultAsync(GetOptions options)
            => repo.GetFirstOrDefaultAsync(SanitizeGetOptions(options));

        public virtual IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => data;

        public virtual Task<TEntity?> AutoGetFirstOrDefaultAsync(TEntity data)
        {
            IDataDictionary autoGetData = new DataDictionary();
            var entityType = typeof(TEntity);
            var properties = entityType.GetProperties();
            foreach (var pInfo in properties)
            {
                var pType = pInfo.PropertyType;
                if (pType.IsClass && pType.Name != "String")
                    continue;

                var column = pInfo.Name;
                var value = pInfo.GetValue(data);

                autoGetData.Add(column, value);
            }

            autoGetData = SanitizeDataForAutoGet(autoGetData);

            var filters = new Operators();
            foreach (var kv in autoGetData)
                filters.Add(kv.Key, kv.Value);

            return GetFirstOrDefaultAsync(new GetOptions { Filters = filters });
        }

        public virtual async Task<TEntity> GetOrCreateAsync(TEntity data)
        {
            var res = await AutoGetFirstOrDefaultAsync(data);
            if (res != null)
                return res;

            return await CreateAsync(data);
        }

        public virtual Task CreateIfNotExistsAsync(TEntity data)
            => GetOrCreateAsync(data);

        public virtual async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, GetOptions options)
            => await Task.Run(() => data);

        public virtual async Task<int> UpdateAsync(IDataDictionary data, GetOptions options)
        {
            data = await ValidateForUpdateAsync(data, options);
            return await repo.UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteAsync(GetOptions options)
            => repo.DeleteAsync(options);
    }
}