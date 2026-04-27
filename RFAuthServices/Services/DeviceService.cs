using Microsoft.Extensions.DependencyInjection;
using RFAuthEntities.Entities;
using RFAuthIRepositories.Repositories;
using RFAuthIServices.IServices;
using RFBaseIServices.IServices;
using RFBaseServices.Services;
using System.Security.Cryptography;

namespace RFAuthServices.Services
{
    public class DeviceService(
        IDeviceRepository deviceRepository,
        IServiceProvider serviceProvider
    )
        : BaseService<Device>(deviceRepository),
        IDeviceService
    {
        public int TokenSize { get; set; } = 64;

        public override async Task<Device> ValidateForCreateAsync(Device device)
        {
            device = await base.ValidateForCreateAsync(device);

            if (string.IsNullOrEmpty(device.Token))
            {
                int byteCount = (int)Math.Ceiling(TokenSize / 4.0) * 3;
                do
                {
                    byte[] bytes = RandomNumberGenerator.GetBytes(byteCount);
                    var token = Convert.ToBase64String(bytes)[..TokenSize];
                    device.Token = token;
                } while (await GetFirstOrDefaultByTokenAsync(device.Token) != null);
            } else if (await GetFirstOrDefaultByTokenAsync(device.Token) != null)
            {
                throw new InvalidOperationException("A device with the same token already exists.");
            }

            return device;
        }

        public async Task<Device> CreateAsync()
        {
            var userService = serviceProvider.GetRequiredService<IUserService>();
            var device = new Device
            {
                CreatedById = await userService.GetCurrentOrSystemUserIdAsync(),
            };
            return await CreateAsync(device);
        }

        public async Task<Device?> GetFirstOrDefaultByTokenAsync(string token)
        {
            return await deviceRepository.GetFirstOrDefaultByTokenAsync(token);
        }

        public async Task<Device> GetFirstOrCreateByTokenAsync(string token)
        {
            return await deviceRepository.GetFirstOrDefaultByTokenAsync(token)
                ?? await CreateAsync();
        }
    }
}
