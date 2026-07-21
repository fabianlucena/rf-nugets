using RFDBLocalizer.Entities;
using RFDBLocalizer.QueryOptions;
using RFIRepositories.IRepositories;

namespace RFDBLocalizer.IRepositories
{
    public interface ITranslationRepository : ICommonEntityRepository<Translation>
    {
        Task<string?> GetSingleTextOrDefaultAsync(TranslationQueryOptions options);
    }
}