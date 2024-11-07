using RFAuth.Entities;
using RFService.IService;

namespace RFAuth.IServices
{
    public interface IDeviceService : IService<Device>
    {
        Task<Device> GetSingleForTokenAsync(string token);

        Task<Device?> GetSingleOrDefaultForTokenAsync(string token);

        Task<Device> GetSingleForTokenOrCreateAsync(string? token);
    }
}
