using Microsoft.Extensions.Localization;
using RFAuth.IServices;
using Resources;

namespace RFAuth_es
{
    public class LocalizerService: ILocalizerService
    {
        private readonly IStringLocalizer _localizer;
        
        public LocalizerService(IStringLocalizerFactory localizerFactory)
        {
            var type = typeof(Messages); // Clase generada a partir del archivo de recursos
            _localizer = localizerFactory.Create(type);
        }

        public LocalizedString this[string name]
        {
            get => _localizer[name];
        }
    }
}
