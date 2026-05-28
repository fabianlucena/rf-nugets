using RFPermissions.Entities;
using RFPermissions.IRepositories;
using RFPermissions.IServices;
using RFPermissions.QueryOptions;
using RFServices.Services;
using System.ComponentModel.DataAnnotations;

namespace RFPermissions.Services
{
    public class PermissionService(IPermissionRepository permissionRepository)
        : ImmutableEntityService<Permission>(permissionRepository),
        IPermissionService
    {
        public async Task<IEnumerable<long>> GetIdsByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null)
        {
            options = (PermissionQueryOptions?)(options?.Clone() ?? new PermissionQueryOptions());
            options!.Names = names;
            return await permissionRepository.GetIdsAsync(options);
        }

        public async Task<IEnumerable<long>> GetIdsOrCreateByNamesAsync(IEnumerable<string> names, PermissionQueryOptions? options = null)
        {
            var ids = new List<long>();
            foreach (var name in names)
            {
                if (string.IsNullOrWhiteSpace(name))
                    continue;

                var id = await GetSingleIdOrDefaultByNameAsync(name, options)
                    ?? (await CreateAsync(new Permission { Name = name })).Id;

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
    }
}
