using Microsoft.Extensions.Primitives;

namespace RFLocalizer.IServices
{
    public interface ILocalizerFactoryService
    {
        ILocalizerService GetLocalizer(string language, string context);

        ILocalizerService GetLocalizerForAcceptLanguage(StringValues acceptLanguage, string context);
    }
}
