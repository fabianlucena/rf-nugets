using RFIServices.IServices;
using RFPermissions.Attributes;
using RFPermissions.Entities;
using RFPermissions.IServices;
using RFPermissions.QueryOptions;
using RFPermissions.Services;
using RFServices.Attributes;
using RFServices.Interfaces;
using System.Data;
using System.Reflection;

namespace RFPermissions;

[SeedData(true)]
public class RFPermissionsInitialDataSeeder(
    IPermissionService permissionService,
    IUserService userService
) : ISeeder
{
    public async Task Run()
    {
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetModules())
            .SelectMany(m => m.GetTypes());

        var classesPermissions = types
            .Where(t => t.IsClass && t.GetCustomAttribute<PermissionAttribute>() != null)
            .SelectMany(t => t.GetCustomAttribute<PermissionAttribute>()!.Permissions);

        var methodsPermissions = types
            .SelectMany(t => t.GetMethods())
            .Where(t => t.GetCustomAttribute<PermissionAttribute>() != null)
            .SelectMany(t => t.GetCustomAttribute<PermissionAttribute>()!.Permissions);

        var allPermissionsName = classesPermissions
            .Concat(methodsPermissions)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var currentPermissionsName = await permissionService
            .GetNamesAsync(new PermissionQueryOptions { Names = allPermissionsName });

        var newPermissionsName = allPermissionsName.Except(currentPermissionsName);

        if (newPermissionsName.Any())
        {
            var creatorId = await userService.GetCurrentOrSystemUserIdAsync();
            foreach (var permissionName in newPermissionsName)
            {
                await permissionService.CreateAsync(new Permission
                {
                    Name = permissionName,
                    CreatedById = creatorId,
                });
            }
        }
    }
}