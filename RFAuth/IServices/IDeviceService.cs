using RFAuth.Entities;
using RFAuth.QueryOptions;
using RFIServices.IServices;

namespace RFAuth.IServices
{
    public interface IDeviceService : ICreatableEntityService<Device>
    {
        Task<Device> GetSingleByTokenOrCreateAsync(string deviceToken, DeviceQueryOptions? options = null, Func<Device, Task<Device>>? creationData = null);

        Task<Device?> GetFirstOrDefaultByTokenAsync(string token, DeviceQueryOptions? options = null);
        Task<Device?> GetSingleOrDefaultByTokenAsync(string token, DeviceQueryOptions? options = null);
    }
}
