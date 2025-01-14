using Microsoft.Extensions.DependencyInjection;
using RFLocalizer.IServices;

namespace RFAuth_es
{
    public static class Setup
    {
        public static void ConfigureDataRFAuthEs(IServiceProvider provider)
            => ConfigureDataRFAuthEsAsync(provider).Wait();

        public static async Task ConfigureDataRFAuthEsAsync(IServiceProvider provider)
        {
            var addTranslationService = provider.GetService<IAddTranslationService>();
            if (addTranslationService != null)
            {
                await addTranslationService.AddAsync("es", "exception", "Unknown username.", "Nombre de usuario desconocido.");
            }
        }
    }
}
