using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RFPermissions.Entities;
using RFPermissions.IRepositories;
using RFPermissions.IServices;
using RFPermissions.QueryOptions;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFPermissions.Services;

[RegisterService]
public class PermissionService(
    IPermissionRepository permissionRepository,
    IServiceProvider serviceProvider
)
    : ImmutableEntityService<Permission>(permissionRepository, serviceProvider),
    IPermissionService
{
    private IEnumerable<string>? _currentPermissionsName = null;

    public IEnumerable<string> CurrentPermissionsName {
        get
        {
            if (_currentPermissionsName is null)
            {
                var contextAccessor = ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                var items = contextAccessor.HttpContext?.Items;
                if (items?.TryGetValue("PermissionsName", out var permissionsNameRaw) == true
                    && permissionsNameRaw is IEnumerable<string> permissionsName
                )
                    _currentPermissionsName = permissionsName;
                else
                    _currentPermissionsName = [];
            }

            return _currentPermissionsName;
        }
    }

    public async Task<IEnumerable<string>> GetNamesAsync(PermissionQueryOptions options)
        => await permissionRepository.GetNamesAsync(options);

    public async Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null)
    {
        options = (PermissionQueryOptions?)(options?.Clone() ?? new PermissionQueryOptions());
        options!.Names = names;
        return await permissionRepository.GetListIdAsync(options);
    }

    public async Task<long> GetIdOrCreateByNameAsync(string name, PermissionQueryOptions? options = null, Func<Permission, Task<Permission>>? createFactory = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        var id = await GetSingleIdOrDefaultByNameAsync(name, options);
        if (id is not null)
            return id.Value;

        var permission = new Permission { Name = name };
        if (createFactory is not null)
            permission = await createFactory(permission);

        return (await CreateAsync(permission)).Id;
    }

    public async Task<IEnumerable<long>> GetIdsOrCreateByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null, Func<Permission, Task<Permission>>? createFactory = null)
    {
        var ids = new List<long>();
        foreach (var name in names)
        {
            if (string.IsNullOrWhiteSpace(name))
                continue;

            var id = await GetIdOrCreateByNameAsync(name, options, createFactory);
            ids.Add(id);
        }

        return ids;
    }

    public async Task<IEnumerable<string>> GetNamesByIdsAsync(IEnumerable<long> ids, PermissionQueryOptions? options = null)
    {
        options = (PermissionQueryOptions?)(options?.Clone() ?? new PermissionQueryOptions());
        options!.Ids = ids;
        return await permissionRepository.GetNamesAsync(options);
    }

    public Task<long?> GetSingleIdOrDefaultByNameAsync(string name, PermissionQueryOptions? options = null)
        => GetSingleIdOrDefaultAsync(options: new PermissionQueryOptions
        {
            Names = [name]
        });

    public bool HasCurrentPermission(string permissionName)
        => CurrentPermissionsName.Contains(permissionName);
}
