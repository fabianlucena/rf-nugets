using Microsoft.EntityFrameworkCore;
using RFAuthEntities.Entities;
using RFAuthIRepositories.Repositories;
using RFBaseEF.Repositories;

namespace RFAuthEF.Repositories
{
    public class DeviceRepository
        : CreatableEntityRepository<Device>,
        IDeviceRepository
    {
        public DeviceRepository(DbContext context) : base(context) { }

        public async Task<Device?> GetFirstOrDefaultByTokenAsync(string token)
        {
            var table = context.Set<Device>();
            var device = await table
                .Where(d => d.Token == token)
                .FirstOrDefaultAsync();

            return device;
        }
    }
}
