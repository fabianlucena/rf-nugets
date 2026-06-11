using RFL10n;
using RFRBAC.Entities;
using RFRBAC.IRepositories;
using RFRBAC.IServices;
using RFRBAC.QueryOptions;
using RFServices.Services;

namespace RFRBAC.Services;

public class RoleService(
    IRoleRepository roleRepository,
    IL10n l10n,
    IServiceProvider serviceProvider
)
    : LocalizableEntityService<Role>(roleRepository, l10n, serviceProvider),
    IRoleService
{
    public async Task<long> GetSingleIdOrCreateByNameAsync(string name, RoleQueryOptions? options = null)
        => (await GetSingleByNameOrCreateAsync(name, options)).Id;
}
