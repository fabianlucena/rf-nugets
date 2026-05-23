using RFHttpAction.Entities;
using RFHttpAction.IRepositories;
using RFHttpAction.IServices;
using RFL10n;
using RFServices.Services;

namespace RFHttpAction.Services
{
    public class HttpActionTypeService(IHttpActionTypeRepository httpActionTypeRepository, IL10n l10n)
        : LocalizableEntityService<HttpActionType>(httpActionTypeRepository, l10n),
            IHttpActionTypeService
    {
    }
}
