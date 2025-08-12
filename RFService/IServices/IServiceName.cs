using RFService.ILibs;
using RFService.Libs;
using RFService.Repo;

namespace RFService.IServices
{
    public interface IServiceName<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        IDataDictionary SanitizeNameForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("Name", out object? value))
            {
                if (value != null
                    && !string.IsNullOrEmpty((string)value)
                )
                {
                    return new DataDictionary { { "Name", value } };
                }
                else
                {
                    data = new DataDictionary(data);
                    data.Remove("Name");
                }
            }

            return data;
        }

        Task<TEntity> GetSingleForNameAsync(string name, QueryOptions? options = null);

        Task<TEntity?> GetSingleOrDefaultForNameAsync(string name, QueryOptions? options = null);
    }
}
