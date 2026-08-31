using RFRGOBAC.DTO;
using RFRGOBAC.Entities;
using RFRGOBAC.QueryOptions;

namespace RFRGOBAC.IServices;

public interface ISystemUserService
{
    Task<IEnumerable<OrganizationUser>> GetListAsync(OrganizationUserQueryOptions? options = null);
    Task<OrganizationUser?> GetSingleOrDefaultAsync(OrganizationUserQueryOptions? options = null);
}
