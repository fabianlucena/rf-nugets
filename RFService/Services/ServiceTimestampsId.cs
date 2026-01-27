using RFService.Exceptions;
using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;
using RFService.ILibs;
using RFService.IServices;

namespace RFService.Services
{
    public abstract class ServiceTimestampsId<TRepo, TEntity>(TRepo repo)
        : ServiceTimestamps<TRepo, TEntity>(repo),
            IServiceId<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsId
    {
        public Int64 GetId(TEntity item) => item.Id;

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceId<TEntity>)this).SanitizeIdForAutoGet(data)
            );

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Id != 0)
            {
                throw new ForbiddenIdForCreationException();
            }

            return data;
        }

        public async Task<IEnumerable<Int64>> GetListIdAsync(QueryOptions options)
            => (await GetListAsync(options)).Select(GetId); public async Task<Int64> GetSingleIdAsync(QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            var item = await GetSingleAsync(options);
            return GetId(item);
        }

        public async Task<Int64> GetSingleIdOrDefaultAsync(QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            var item = await GetSingleOrDefaultAsync(options);
            if (item == null)
                return 0;

            return GetId(item);
        }

        public async Task<Int64?> GetSingleIdOrNullAsync(QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            var item = await GetSingleOrDefaultAsync(options);
            if (item == null)
                return null;

            return GetId(item);
        }

        public virtual Task<TEntity> GetSingleForIdAsync(Int64 id, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Id", id);
            return GetSingleAsync(options);
        }

        public virtual Task<TEntity?> GetSingleOrDefaultForIdAsync(Int64 id, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Id", id);
            return GetSingleOrDefaultAsync(options);
        }

        public virtual Task<IEnumerable<TEntity>> GetListForIdsAsync(IEnumerable<Int64> id, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Id", id);
            return GetListAsync(options);
        }

        public virtual Task<int> UpdateForIdAsync(IDataDictionary data, Int64 id, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Id", id);
            return UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteForIdAsync(Int64 id, QueryOptions? options = null)
        {
            options ??= new QueryOptions();
            options.AddFilter("Id", id);
            return DeleteAsync(options);
        }
    }
}