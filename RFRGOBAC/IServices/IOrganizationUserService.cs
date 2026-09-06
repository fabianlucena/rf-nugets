using RFBase.ILibs;
using RFRGOBAC.DTO;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface IOrganizationUserService
{
    Task<OrganizationUser> CreateAsync(long organizationId, OrganizationUser user);
    Task<IEnumerable<OrganizationUser>> GetListAsync(OrganizationUserQueryOptions? options = null);
    Task<OrganizationUser?> GetSingleOrDefaultAsync(OrganizationUserQueryOptions? options = null);
    Task<int> UpdateByUuidAsync(long organizationId, Guid uuid, IDataDictionary data, OrganizationUserQueryOptions? options = null);
    Task<int> DeleteByUuidAsync(Guid uuid, OrganizationUserQueryOptions? options = null);
    Task<int> RestoreByUuidAsync(Guid uuid, OrganizationUserQueryOptions? options = null);
    Task<OrganizationUser> Translate(OrganizationUser user, string? context = null);
    Task<IEnumerable<OrganizationUser>> Translate(IEnumerable<OrganizationUser> users, string? context = null);

    Task<int> SetRolesUuidByUuidAsync(long organizationId, Guid userUuid, IEnumerable<Guid> rolesUuid, OrganizationUserQueryOptions? options = null);
    Task<int> SetRolesUuidByIdAsync(long organizationId, long userId, IEnumerable<Guid> rolesUuid, OrganizationUserQueryOptions? options = null);
}
