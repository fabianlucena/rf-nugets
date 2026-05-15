using RFAuthEntities.Entities;
using RFAuthEntities.QueryOptions;
using RFBaseIServices.IServices;

namespace RFAuthIServices.IServices
{
    public interface IDeviceService : IBaseService<Device>
    {
        Task<Device> CreateAsync();
        Task<Device> GetFirstOrCreateByTokenAsync(string deviceToken, DeviceQueryOptions? options = null);
    }
}
