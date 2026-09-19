using Microsoft.Extensions.DependencyInjection;
using RFL10n;

namespace RFDapper_es
{
    public static class Setup
    {
        public static void ConfigureDataRFDapperEs(IServiceProvider provider)
        {
            var l10n = provider.GetRequiredService<IL10n>();
            l10n.AddTranslationsFromFile("es", "", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rfdapper_es.txt"));
        }
    }
}
