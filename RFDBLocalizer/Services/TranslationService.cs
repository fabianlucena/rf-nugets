using RFDBLocalizer.Entities;
using RFDBLocalizer.IServices;
using RFService.IRepo;
using RFService.Repo;
using RFService.Services;

namespace RFDBLocalizer.Services
{
    public class TranslationService(
        IRepo<Translation> repo
    )
        : ServiceIdUuid<IRepo<Translation>, Translation>(repo),
            ITranslationService
    {
        public async Task<string?> GetTranslationAsync(
            string language,
            string context,
            string source
        )
        {
            var options = new GetOptions();
            options.Include("Language", "l")
                .Include("Context", "c")
                .Include("Source", "s")
                .AddFilter("l.Name", language)
                .AddFilter("c.Name", context)
                .AddFilter("s.Text", source);

            var row = await GetSingleOrDefaultAsync(options);

            return row?.Text ?? null;
        }
    }
}
