using RFLocalizer.IServices;

namespace RFLocalizer.Services
{
    public class LocalizerService(string context)
        : ILocalizerService
    {
        public string this[string text]
            => text;
    }
}
