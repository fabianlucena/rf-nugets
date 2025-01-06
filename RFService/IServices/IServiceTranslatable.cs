using RFService.ILibs;
using RFService.Libs;

namespace RFService.IServices
{
    public interface IServiceTranslatable<TEntity>
        : IService<TEntity>
        where TEntity : class
    {
        IDataDictionary SanitizeIsTranslatableForAutoGet(IDataDictionary data)
        {
            IDataDictionary? newData = null;
            if (data.TryGetValue("IsTranslatable", out object? value))
            {
                if (value == null)
                {
                    newData = new DataDictionary(data);
                    newData.Remove("IsTranslatable");
                }
            }

            if (data.TryGetValue("TranslationContext", out value))
            {
                if (value == null)
                {
                    newData ??= new DataDictionary(data);
                    newData.Remove("TranslationContext");
                }
            }

            return newData ?? data;
        }
    }
}
