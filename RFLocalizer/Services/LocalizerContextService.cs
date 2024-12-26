using RFLocalizer.IServices;

namespace RFLocalizer.Services
{
    public class LocalizerContextService()
        : ILocalizerContextService
    {
        public ILocalizerService this[string context]
            => new LocalizerService(context);
    }
}
