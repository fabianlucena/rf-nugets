using RFBaseEntities.QueryOptions;
using RFBaseIRepositories.IRepositories;
using RFRGOBACEntities.Entities;

namespace RFRGOBACIRepositories.IRepositories
{
    public interface IOrganizationRepository : ILocalizableEntityRepository<Organization>
    {
        public Task<long?> GetSingleOrDefaultIdByNameAsync(string name, BaseQueryOptions? options = null);
    }
}