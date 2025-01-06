using RFService.Entities;
using RFService.ILibs;
using RFService.IRepo;
using RFService.IServices;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceId<TRepo, TEntity>(TRepo repo)
        : Service<TRepo, TEntity>(repo),
            IServiceId<TEntity>
        where TRepo : IRepo<TEntity>
        where TEntity : EntityId
    {
        public Int64 GetId(TEntity item) => item.Id;

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
            => base.SanitizeDataForAutoGet(
                ((IServiceId<TEntity>)this).SanitizeIdForAutoGet(data)
            );

        public async Task<IEnumerable<Int64>> GetListIdAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetId);

        public virtual Task<TEntity> GetSingleForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Id", id);
            return GetSingleAsync(options);
        }

        public virtual Task<TEntity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Id", id);
            return GetSingleOrDefaultAsync(options);
        }

        public virtual Task<IEnumerable<TEntity>> GetListForIdsAsync(IEnumerable<Int64> id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Id", id);
            return GetListAsync(options);
        }

        public virtual Task<int> UpdateForIdAsync(IDataDictionary data, Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Id", id);
            return UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.AddFilter("Id", id);
            return DeleteAsync(options);
        }
    }
}
