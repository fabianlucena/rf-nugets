using RFService.ILibs;
using RFService.Libs;

namespace RFService.IServices
{
    public interface IServiceSoftDeleteCreatedAt<TEntity>
        : IServiceSoftDelete<TEntity>
        where TEntity : class
    {
        public IDataDictionary SanitizeSoftDeleteCreatedAtForAutoGet(IDataDictionary data)
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
