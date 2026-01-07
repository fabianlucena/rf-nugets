using RFService.Repo;
using RFService.IRepo;
using RFService.Entities;
using System.Data;
using RFService.Libs;
using RFService.ILibs;
using RFOperators;
using RFService.Exceptions;

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

        public virtual async Task<TEntity> CreateAsync(TEntity data, QueryOptions? options = null)
        {
            data = await ValidateForCreationAsync(data);
            return await repo.InsertAsync(data);
        }

        public virtual QueryOptions SanitizeQueryOptions(QueryOptions options)
            => options;

        public virtual Task<int> GetCountAsync(QueryOptions options)
            => repo.GetCountAsync(SanitizeQueryOptions(options));

        public virtual Task<IEnumerable<TEntity>> GetListAsync(QueryOptions options)
            => repo.GetListAsync(SanitizeQueryOptions(options));

        public virtual async Task<TEntity> GetSingleAsync(QueryOptions options)
        {
            options = new QueryOptions(options) { Top = 2 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                throw new NoRowsException();

            if (count > 1)
                throw new TooManyRowsException();

            return list.First();
        }

        public virtual async Task<TEntity?> GetSingleOrDefaultAsync(QueryOptions options)
        {
            options = new QueryOptions(options) { Top = 2 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                return null;

            if (count > 1)
                throw new TooManyRowsException();

            return list.First();
        }

        public virtual async Task<TEntity?> GetSingleOrCreateAsync(QueryOptions options, Func<TEntity> dataFactory)
            => await GetSingleOrDefaultAsync(options)
                ?? await CreateAsync(dataFactory());

        public virtual async Task<TEntity?> GetFirstOrDefaultAsync(QueryOptions options)
        {
            options = new QueryOptions(options) { Top = 1 };
            var list = await GetListAsync(options);
            var count = list.Count();
            if (count == 0)
                return null;

            return list.First();
        }

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

            return GetFirstOrDefaultAsync(new QueryOptions { Filters = filters });
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

        public virtual async Task<IDataDictionary> ValidateForUpdateAsync(IDataDictionary data, QueryOptions options)
            => await Task.Run(() => data);

        public virtual async Task<int> UpdateAsync(IDataDictionary data, QueryOptions options)
        {
            data = await ValidateForUpdateAsync(data, options);
            return await repo.UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteAsync(QueryOptions options, DataDictionary? data = null)
            => repo.DeleteAsync(options);

        public virtual Task<Int64?> GetInt64Async(QueryOptions options)
            => repo.GetInt64Async(options);
    }
}