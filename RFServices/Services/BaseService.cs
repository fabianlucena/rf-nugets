using RFBase.ILibs;
using RFBase.Libs;
using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFIServices.QueryOptions;
using RFServices.Exceptions;

namespace RFServices.Services;

public class BaseService<T>(
    IBaseRepository<T> repository,
    IServiceProvider serviceProvider
)
    : IBaseService<T>
    where T : Base
{
    public IServiceProvider ServiceProvider { get => serviceProvider; }

    public virtual async Task<T> ValidateForCreateAsync(T entity)
    {
        return entity;
    }

    public virtual async Task<IDataDictionary> ValidateForUpdate(IDataDictionary data)
    {
        return data;
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        entity = await ValidateForCreateAsync(entity);
        return await repository.CreateAsync(entity);
    }

    public virtual async Task<IEnumerable<T>> GetListAsync(BaseQueryOptions options)
    {
        return await repository.GetListAsync(options);
    }

    public virtual async Task<T?> GetFirstOrDefaultAsync(BaseQueryOptions options)
    {
        options = (BaseQueryOptions)options.Clone();
        options.Take = 1;
        var list = await repository.GetListAsync(options);
        if (!list.Any())
            return null;

        return list.First();
    }

    public virtual async Task<T> GetFirstAsync(BaseQueryOptions options)
        => await GetFirstOrDefaultAsync(options)
            ?? throw new NoEntityFoundMatchingTheSpecifiedCriteriaException();

    public virtual async Task<T?> GetSingleOrDefaultAsync(BaseQueryOptions options)
    {
        options.Take = 2;
        var list = await repository.GetListAsync(options);

        if (!list.Any())
            return null;

        if (list.Count() > 1)
            throw new MultipleEntitiesFoundMatchingTheSpecifiedCriteriaException();

        return list.First();
    }

    public virtual async Task<T> GetSingleAsync(BaseQueryOptions options)
        => await GetSingleOrDefaultAsync(options)
            ?? throw new NoEntityFoundMatchingTheSpecifiedCriteriaException();

    public virtual async Task<int> UpdateAsync(DataDictionary data, BaseQueryOptions options)
        => await repository.UpdateAsync(data, options);

    public virtual async Task<int> DeleteAsync(BaseQueryOptions options)
        => await repository.DeleteAsync(options);
}
