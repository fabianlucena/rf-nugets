using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IGetCurrentAndSystemUserService
{
    T1 GetRequiredService<T1>() where T1 : notnull;

    public async Task<long> GetCurrentUserIdAsync()
        => await GetRequiredService<IUserService>().GetCurrentUserIdAsync();

    public async Task<User> GetSystemUserAsync()
        => await GetRequiredService<IUserService>().GetSystemUserAsync();

    public async Task<long> GetSystemUserIdAsync()
        => await GetRequiredService<IUserService>().GetSystemUserIdAsync();

    public async Task<User> GetCurrentUserAsync()
        => await GetRequiredService<IUserService>().GetCurrentUserAsync();

    public async Task<User> GetCurrentOrSystemUserAsync()
        => await GetRequiredService<IUserService>().GetCurrentOrSystemUserAsync();

    public async Task<long> GetCurrentOrSystemUserIdAsync()
        => await GetRequiredService<IUserService>().GetCurrentOrSystemUserIdAsync();
}
