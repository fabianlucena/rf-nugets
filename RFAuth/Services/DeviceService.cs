using Microsoft.Extensions.DependencyInjection;
using RFAuth.Entities;
using RFAuth.Exceptions;
using RFAuth.IRepositories;
using RFAuth.IServices;
using RFAuth.QueryOptions;
using RFBase.Libs;
using RFIServices.IServices;
using RFRegisterService.Attributes;
using RFServices.Services;

namespace RFAuth.Services;

[RegisterService]
public class DeviceService(
    IDeviceRepository deviceRepository,
    IServiceProvider serviceProvider
)
    : CreatableEntityService<Device>(deviceRepository, serviceProvider),
    IDeviceService
{
    public int TokenSize { get; set; } = 64;

    public override async Task<Device> ValidateForCreateAsync(Device device)
    {
        if (device.CreatedById == 0)
        {
            var userService = ServiceProvider.GetRequiredService<IUserService>();
            device.CreatedById = await userService.GetCurrentOrSystemUserIdAsync();
        }

        device = await base.ValidateForCreateAsync(device);

        if (string.IsNullOrEmpty(device.Token))
        {
            device.Token = await Token.GetString(TokenSize, async token => await GetFirstOrDefaultByTokenAsync(token) == null);
        } else if (await GetFirstOrDefaultByTokenAsync(device.Token) != null)
        {
            throw new ADeviceWithTheSameTokenAlreadyExistsException();
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
