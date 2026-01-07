using RFAuth.Entities;
using RFAuth.IServices;
using RFService.Services;
using RFService.Repo;
using RFService.IRepo;
using RFService.IServices;
using RFService.Libs;
using RFService.ILibs;

namespace RFAuth.Services
{
    public class UserService(
        IRepo<User> repo,
        IUserTypeService userTypeService,
        IPropertiesDecorators propertiesDecorators
    )
        : ServiceSoftDeleteTimestampsIdUuidEnabled<IRepo<User>, User>(repo),
            IUserService,
            IServiceDecorated
    {
        public IPropertiesDecorators PropertiesDecorators { get; } = propertiesDecorators;

        public async Task<User> GetSingleForUsernameAsync(string username)
        {
            return await GetSingleAsync(new QueryOptions
            {
                Filters = { { "Username", username } }
            });
        }

        public async Task<User?> GetSingleOrDefaultForUsernameAsync(string username)
        {
            return await GetSingleOrDefaultAsync(new QueryOptions
            {
                Filters = { { "Username", username } }
            });
        }

        public override async Task<User> ValidateForCreationAsync(User data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (data.TypeId <= 0)
            {
                data.Type ??= await userTypeService.GetSingleForNameAsync("user");
                data.TypeId = data.Type.Id;
            }

            return data;
        }

        public override IDataDictionary SanitizeDataForAutoGet(IDataDictionary data)
        {
            if (data.TryGetValue("Username", out object? value))
            {
                if (value != null
                    && !string.IsNullOrEmpty((string)value)
                )
                {
                    return new DataDictionary { { "Username", value } };
                }
                else
                {
                    data = new DataDictionary(data);
                    data.Remove("Username");
                }
            }

            return base.SanitizeDataForAutoGet(data);
        }
    }
}
