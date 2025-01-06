using Microsoft.Extensions.DependencyInjection;
using RFLocalizer.IServices;

namespace RFAuth_es
{
    public static class Setup
    {
        public static void ConfigureRFAuthEs(IServiceProvider provider)
            => ConfigureRFAuthEsAsync(provider).Wait();

        public static async Task ConfigureRFAuthEsAsync(IServiceProvider provider)
        {
            var addTranslationService = provider.GetService<IAddTranslationService>();
            if (addTranslationService != null)
            {
                await addTranslationService.AddAsync("es", "exception", "Unknown username.", "Nombre de usuario desconocido.");
            }
        }
    }
}
