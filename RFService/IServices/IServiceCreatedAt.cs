using RFService.ILibs;
using RFService.Libs;

namespace RFService.IServices
{
    public interface IServiceCreatedAt<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        public IDataDictionary SanitizeCreatedAtForAutoGet(IDataDictionary data)
        {
            IDataDictionary? newData = null;
            if (data.TryGetValue("CreatedAt", out object? value))
            {
                if (value == null
                    || (DateTime)value == DateTime.MinValue
                )
                {
                    newData = new DataDictionary(data);
                    newData.Remove("CreatedAt");
                }
            }

            return newData ?? data;
        }
    }
}
