using Microsoft.Extensions.DependencyInjection;
using RFL10n;

namespace RFAuth_es
{
    public static class Setup
    {
        public static void ConfigureDataRFAuthEs(IServiceProvider provider)
        {
            var l10n = provider.GetRequiredService<IL10n>();
            l10n.AddTranslationsFromFile("es", "", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "rfauth_es.txt"));
        }
    }
}
