using Microsoft.EntityFrameworkCore;
using RFAuth.Entities;
using RFAuth.IRepositories;
using RFAuth.QueryOptions;
using RFEntitiesEF.Repositories;
using RFIServices.QueryOptions;
using RFServices.Attributes;

namespace RFAuthEF.Repositories
{
    [RegisterService]
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
