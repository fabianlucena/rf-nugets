using Microsoft.Extensions.DependencyInjection;
using RFDBLocalizer.IServices;

namespace RFDBLocalizer.Services
{
    public class DBTranslator
        : IDBTranslator
    {
        public async Task<string?> GetTranslationAsync(IServiceProvider provider, string text, string language, string context = "")
        {
            var translationService = provider.GetRequiredService<ITranslationService>();
            return await translationService.GetTranslationAsync(text, language, context);
        }
    }
}
