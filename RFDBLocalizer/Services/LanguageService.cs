using RFDBLocalizer.Entities;
using RFDBLocalizer.IRepositories;
using RFDBLocalizer.IServices;
using RFServices.Services;

namespace RFDBLocalizer.Services;

public class LanguageService(
    ILanguageRepository languageRepository,
    IServiceProvider serviceProvider
)
    : NominableEntityService<Language>(languageRepository, serviceProvider),
    ILanguageService
{
}
