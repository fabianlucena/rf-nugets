using RFService.ILibs;
using RFService.Libs;

namespace RFService.IServices
{
    public interface IServiceTitle<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        IDataDictionary SanitizeTitleForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("Title", out object? value))
            {
                if (string.IsNullOrEmpty((string?)value))
                {
                    data = new DataDictionary(data);
                    data.Remove("Title");
                }
            }

            return data;
        }
    }
}
