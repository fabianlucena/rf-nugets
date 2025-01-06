using RFService.ILibs;
using RFService.Libs;

namespace RFService.IServices
{
    public interface IServiceTimestamps<TEntity>
        where TEntity : class
    {
        public IDataDictionary SanitizeTimestampsForAutoGet(IDataDictionary data)
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

            if (data.TryGetValue("UpdatedAt", out value))
            {
                if (value == null
                    || (DateTime)value == DateTime.MinValue
                )
                {
                    newData ??= new DataDictionary(data);
                    newData.Remove("UpdatedAt");
                }
            }

            return newData ?? data;
        }
    }
}
