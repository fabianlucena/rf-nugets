using RFEntities.Entities;
using RFIRepositories.IRepositories;
using RFIServices.IServices;
using RFRegisterService.Attributes;

namespace RFServices.Services;

[RegisterService]
public class UserTypeService(
    IUserTypeRepository userTypeRepository,
    IServiceProvider serviceProvider
)
    : LocalizableEntityService<UserType>(userTypeRepository, serviceProvider),
    IUserTypeService
{
}
