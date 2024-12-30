using RFLocalizer.Entities;
using RFLocalizer.IServices;
using RFService.Repo;

namespace RFLocalizer.Services
{
    public class AddTranslationService(
        ILanguageService languageService,
        IContextService contextService,
        ISourceService sourceService,
        ITranslationService translationService
    )
        : IAddTranslationService
    {
        private static readonly Dictionary<string, Int64> languadesId = [];
        private static readonly Dictionary<string, Int64> contextsId = [];

        public async Task<Int64> GetLanguageId(string name)
        {
            if (languadesId.TryGetValue(name, out var id))
                return id;

            id = await languageService.GetSingleIdForNameOrCreateAsync(name, new Language { Name = name });
            languadesId[name] = id;

            return id;
        }

        public async Task<Int64> GetContextId(string name)
        {
            if (contextsId.TryGetValue(name, out var id))
                return id;

            id = await contextService.GetSingleIdForNameOrCreateAsync(name, new Context { Name = name });
            contextsId[name] = id;

            return id;
        }

        public async Task<bool> AddAsync(
            string language,
            string context,
            string source,
            string translation
        )
        {
            var languageId = await GetLanguageId(language);
            var contextId = await GetContextId(context);
            var sourceId = await sourceService.GetSingleIdForNameOrCreateAsync(source, new Source { Name = source });

            var row = await translationService.GetSingleOrDefaultAsync(new GetOptions
            {
                Filters =
                {
                    { "LanguageId", languageId },
                    { "ContextId", contextId },
                    { "SourceId", sourceId },
                }
            });

            if (row != null)
                return false;

            row = await translationService.CreateAsync(new Translation {
                LanguageId = languageId,
                ContextId = contextId,
                SourceId = sourceId,
                Text = translation
            });

            return row != null;
        }
    }
}
