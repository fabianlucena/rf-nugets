using RFAuth.Entities;
using RFService.IServices;

namespace RFAuth.IServices
{
    public interface IDeviceService
        : IService<Device>,
        IServiceId<Device>
    {
        Task<Device> GetSingleForTokenAsync(string token);

        Task<Device?> GetSingleOrDefaultForTokenAsync(string token);

        Task<Device> GetSingleForTokenOrCreateAsync(string? token);
    }
}
