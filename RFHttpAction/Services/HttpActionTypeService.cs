using RFHttpAction.Entities;
using RFHttpAction.IRepositories;
using RFHttpAction.IServices;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFHttpAction.Services;

[RegisterService]
public class HttpActionTypeService(
    IHttpActionTypeRepository httpActionTypeRepository,
    IServiceProvider serviceProvider
)
    : LocalizableEntityService<HttpActionType>(httpActionTypeRepository, serviceProvider),
        IHttpActionTypeService
{
}
