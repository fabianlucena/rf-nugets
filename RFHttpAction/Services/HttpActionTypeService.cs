using RFHttpAction.Entities;
using RFHttpAction.IRepositories;
using RFHttpAction.IServices;
using RFL10n;
using RFServices.Services;

namespace RFHttpAction.Services;

public class HttpActionTypeService(
    IHttpActionTypeRepository httpActionTypeRepository,
    IServiceProvider serviceProvider
)
    : LocalizableEntityService<HttpActionType>(httpActionTypeRepository, serviceProvider),
        IHttpActionTypeService
{
}
