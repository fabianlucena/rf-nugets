using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;
using RFBaseEntities.QueryOptions;

namespace RFAuthEF.Repositories
{
    public class DeviceRepository(DbContext context)
        : CreatableEntityRepository<Device>(context),
        IDeviceRepository
    {
        public override IQueryable<Device> CreateDBSet(BaseQueryOptions? options = null)
        {
            var queryable = base.CreateDBSet(options);

            if (options is DeviceQueryOptions deviceOptions)
            {
                if (deviceOptions.Token is not null)
                    queryable = queryable.Where(d => d.Token == deviceOptions.Token);
            }

            return queryable;
        }
    }
}
