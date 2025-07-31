using Microsoft.Extensions.DependencyInjection;
using RFL10n;

namespace RFService_es
{
    public static class Setup
    {
        public static void ConfigureDataRFServiceEs(IServiceProvider provider)
        {
            var l10n = provider.GetRequiredService<IL10n>();
            l10n.AddTranslationsFromFile("es", "", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rfservice_es.txt"));
        }
    }
}
