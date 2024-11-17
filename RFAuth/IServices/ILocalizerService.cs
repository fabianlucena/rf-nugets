using Microsoft.Extensions.Localization;

namespace RFAuth.IServices
{
    public interface ILocalizerService
    {
        LocalizedString this[string name] { get; }
    }
}
