using RFAuth.Entities;
using RFAuth.IServices;
using RFService.Services;
using RFAuth.Util;
using RFService.Repo;
using RFService.IRepo;

namespace RFAuth.Services
{
    public class DeviceService(IRepo<Device> repo)
        : ServiceTimestampsIdUuid<IRepo<Device>, Device>(repo),
            IDeviceService
    {
        public override async Task<Device> ValidateForCreationAsync(Device data)
        {
            data = await base.ValidateForCreationAsync(data);

            if (string.IsNullOrEmpty(data.Token))
            {
                data.Token = Token.GetString(64);
            };

            return data;
        }

        public async Task<Device> CreateAsync()
        {
            return await CreateAsync(new Device { Token = "" }); 
        }

        public async Task<Device?> GetSingleOrDefaultForTokenAsync(string token)
        {
            return await repo.GetSingleOrDefaultAsync(new GetOptions
            {
                Filters = { { "Token", token } }
            });
        }

        public async Task<Device> GetSingleForTokenAsync(string token)
        {
            return await repo.GetSingleAsync(new GetOptions
            {
                Filters = { { "Token", token } }
            });
        }

        public async Task<Device> GetSingleForTokenOrCreateAsync(string? token)
        {
            var device = string.IsNullOrWhiteSpace(token)?
                null:
                await GetSingleOrDefaultForTokenAsync(token);

            device ??= await CreateAsync();

            return device;
        }
    }
}
