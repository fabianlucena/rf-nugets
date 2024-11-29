using RFService.Exceptions;
using RFService.Entities;
using RFService.IRepo;
using RFService.Repo;

namespace RFService.Services
{
    public abstract class ServiceSoftDeleteTimestampsId<TRepo, TEntity>(TRepo repo)
        : ServiceSoftDeleteTimestamps<TRepo, TEntity>(repo)
        where TRepo : IRepo<TEntity>
        where TEntity : EntitySoftDeleteTimestampsId
    {
        public Int64 GetId(TEntity item) => item.Id;

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
                    options.Filters = new Dictionary<string, object?> { { "Id", value } };
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

        public virtual Task<int> UpdateForIdAsync(IDictionary<string, object?> data, Int64 id, GetOptions? options = null)
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