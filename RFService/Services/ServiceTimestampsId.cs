using RFService.Exceptions;
using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;
using RFService.Libs;
using RFService.ILibs;

namespace RFService.Services
{
    public abstract class ServiceTimestampsId<TRepo, TEntity>(TRepo repo)
        : ServiceTimestamps<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntityTimestampsId
    {
        public Int64 GetId(TEntity item) => item.Id;

        public async Task<IEnumerable<Int64>> GetListIdAsync(GetOptions options)
            => (await GetListAsync(options)).Select(GetId);

        public override async Task<TEntity> ValidateForCreationAsync(TEntity data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.Id != 0)
            {
                throw new ForbiddenIdForCreationException();
            }

            return data;
        }

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Id", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && (Int64)value > 0
                )
                {
                    options.Filters = new DataDictionary { { "Id", value } };
                    return options;
                }
                else {
                    options.Filters.Remove("Id");
                }
            }

            return base.SanitizeForAutoGet(options);
        }

        public virtual Task<TEntity> GetSingleForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return GetSingleAsync(options);
        }

        public virtual Task<TEntity?> GetSingleOrDefaultForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return GetSingleOrDefaultAsync(options);
        }

        public virtual Task<IEnumerable<TEntity>> GetListForIdsAsync(IEnumerable<Int64> id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return GetListAsync(options);
        }

        public virtual Task<int> UpdateForIdAsync(IDataDictionary data, Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return UpdateAsync(data, options);
        }

        public virtual Task<int> DeleteForIdAsync(Int64 id, GetOptions? options = null)
        {
            options ??= new GetOptions();
            options.Filters["Id"] = id;
            return DeleteAsync(options);
        }
    }
}