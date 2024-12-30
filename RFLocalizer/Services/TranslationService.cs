using RFLocalizer.Entities;
using RFLocalizer.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFLocalizer.Services
{
    public class TranslationService(
        IRepo<Translation> repo,
        ILanguageService languageService,
        IContextService contextService,
        ISourceService sourceService
    )
        : ServiceId<IRepo<Translation>, Translation>(repo),
            ITranslationService
    {
        public async Task<string> GetTranslationAsync(
            string language,
            string context,
            string source
        )
        {
            var languageId = await languageService.GetSingleIdForNameOrCreateAsync(language, new Language { Name = language });
            var contextId = await contextService.GetSingleIdForNameOrCreateAsync(context, new Context { Name = language });
            var sourceId = await sourceService.GetSingleIdForNameOrCreateAsync(source, new Source { Name = language });

            var row = await GetSingleOrDefaultAsync(new GetOptions
            {
                Filters =
                {
                    { "LanguageId", languageId },
                    { "ContextId", contextId },
                    { "SourceId", sourceId },
                }
            });

            return row?.Text ?? source;
        }
    }
}
