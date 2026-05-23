using RFAuth.Entities;
using RFAuth.QueryOptions;
using RFAuth.IServices;
using RFAuth.IRepositories;
using RFServices.Services;
using RFBase.Libs;

namespace RFAuth.Services
{
    public class DeviceService(IDeviceRepository deviceRepository)
        : CreatableEntityService<Device>(deviceRepository),
        IDeviceService
    {
        public int TokenSize { get; set; } = 64;

        public override async Task<Device> ValidateForCreateAsync(Device device)
        {
            device = await base.ValidateForCreateAsync(device);

            if (string.IsNullOrEmpty(device.Token))
            {
                device.Token = await Token.GetString(TokenSize, async token => await GetFirstOrDefaultByTokenAsync(token) == null);
            } else if (await GetFirstOrDefaultByTokenAsync(device.Token) != null)
            {
                throw new InvalidOperationException("A device with the same token already exists.");
            }

            return device;
        }

        public async Task<Device?> GetFirstOrDefaultByTokenAsync(string token, DeviceQueryOptions? options = null)
        {
            options = (DeviceQueryOptions?)(options?.Clone() ?? new DeviceQueryOptions());
            options!.Token = token;
            return await GetFirstOrDefaultAsync(options);
        }

        public async Task<Device?> GetSingleOrDefaultByTokenAsync(string token, DeviceQueryOptions? options = null)
        {
            options = (DeviceQueryOptions?)(options?.Clone() ?? new DeviceQueryOptions());
            options!.Token = token;
            return await GetSingleOrDefaultAsync(options);
        }

        public async Task<Device> GetSingleByTokenOrCreateAsync(string token, DeviceQueryOptions? options = null, Func<Device, Task<Device>>? creationData = null)
        {
            Device? device;
            if (!string.IsNullOrWhiteSpace(token))
            {
                device = await GetSingleOrDefaultByTokenAsync(token, options);
                if (device != null)
                    return device;
            }

            device = new Device { Token = token };
            if (creationData != null)
                device = await creationData(device);

            return await CreateAsync(device);
        }
    }
}
