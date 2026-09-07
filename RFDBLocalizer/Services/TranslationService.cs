using RFDBLocalizer.Entities;
using RFDBLocalizer.IRepositories;
using RFDBLocalizer.IServices;
using RFDBLocalizer.QueryOptions;
using RFServices.Services;

namespace RFDBLocalizer.Services;

public class TranslationService(
    ITranslationRepository translationRepository,
    IServiceProvider serviceProvider
)
    : CommonEntityService<Translation>(translationRepository, serviceProvider),
    ITranslationService
{
    public async Task<string?> GetSingleTextOrDefaultAsync(TranslationQueryOptions options)
        => await translationRepository.GetSingleTextOrDefaultAsync(options);

    public async Task<string?> GetTranslationAsync(string language, string context, string source)
        => await GetSingleTextOrDefaultAsync(new TranslationQueryOptions
        {
            Language = language,
            Context = context,
            Source = source,
        });
}
