using RFAuth.Entities;
using RFAuth.IServices;
using RFService.Services;
using RFService.Repo;
using RFService.IRepo;

namespace RFAuth.Services
{
    public class UserService(IRepo<User> repo, IUserTypeService userTypeService)
        : ServiceTimestampsIdUuidEnabled<IRepo<User>, User>(repo),
            IUserService
    {
        public async Task<User> GetSingleForUsernameAsync(string username)
        {
            return await repo.GetSingleAsync(new GetOptions
            {
                Filters = { { "Username", username } }
            });
        }

        public async Task<User?> GetSingleOrDefaultForUsernameAsync(string username)
        {
            return await repo.GetSingleOrDefaultAsync(new GetOptions
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

        public override GetOptions SanitizeForAutoGet(GetOptions options)
        {
            if (options.Filters.TryGetValue("Username", out object? value))
            {
                options = new GetOptions(options);
                if (value != null
                    && !string.IsNullOrEmpty((string)value)
                )
                {
                    options.Filters = new Dictionary<string, object?> { { "Username", value } };
                    return options;
                }
                else
                {
                    options.Filters.Remove("Username");
                }
            }

            return base.SanitizeForAutoGet(options);
        }
    }
}
