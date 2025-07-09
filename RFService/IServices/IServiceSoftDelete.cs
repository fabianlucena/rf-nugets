using RFService.ILibs;
using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceSoftDelete<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        public IDataDictionary SanitizeSoftDeleteForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("DeletedAt", out object? value))
            {
                if (value == null
                    || (DateTime)value == DateTime.MinValue
                )
                {
                    data = new DataDictionary(data);
                    data.Remove("DeletedAt");
                }
            }

            return data;
        }

        Task<int> RestoreAsync(GetOptions options, DataDictionary? data)
        {
            data ??= [];
            data["DeletedAt"] = null;
            return UpdateAsync(data, options);
        }
    }
}
